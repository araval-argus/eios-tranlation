using AutoMapper;
using eios_tranlation.businesslogic.Features.Label;
using eios_tranlation.businesslogic.Features.Label.ViewModels;
using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_tranlation.core.Common;
using eios_tranlation.core.Constants;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.ServiceInterfaces;
using eios_translation.core.Common;
using eios_translation.core.Wrappers;
using eios_translation.infrastructure.DbContext;
using eios_translation.infrastructure.EntityClass;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace eios_translation.infrastructure.ServiceImplementation
{
    public class LabelService : ILabelService
    {

        private readonly EIOSTranslationContext context;
        private readonly IMapper mapper;
        private readonly string key = CommonSettings.AzureTranslationSettings.Key;
        private readonly string endpoint = CommonSettings.AzureTranslationSettings.Endpoint;
        private readonly string location = CommonSettings.AzureTranslationSettings.Location;
        private readonly ILanguageService languageService;

        public LabelService(EIOSTranslationContext context, IMapper mapper, ILanguageService langService)
        {
            this.context = context;
            this.mapper = mapper;
            this.languageService = langService;
        }

        public async Task<List<LabelViewModel>> GetAllLabels()
        {
            var labels = await this.context.Labels.ToListAsync();
            return this.mapper.Map<List<LabelViewModel>>(labels);
        }

        public async Task<LabelViewModel> GetSelectedLabel(int LabelId)
        {
            var result = await this.context.Labels.FirstOrDefaultAsync(a => a.LabelId == LabelId);
            if (result == null)
            {
                throw new ApiException($"No label found with Id : {LabelId}");
            }
            return this.mapper.Map<LabelViewModel>(result);
        }

        public async Task<int> InsertLabel(InsertLabelCommand request)
        {
            try
            {
                // Get Default Language.
                Language baseLanguage = await this.context.Languages
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.IsDefault);

                if (baseLanguage == null)
                {
                    throw new ApiException($"Please add default english language first");
                }

                Label baseLabel = new Label(
                    resourceid: request.ResourceId,
                    fk_labelgroupid: request.FK_LabelGroupId,
                    fk_languageid: baseLanguage.LanguageId,
                    fK_BaseLabelId: null,
                    labelvalue: request.LabelValue,
                    labeltype: LabelType.Normal,
                    labeldescription: null,
                    labelsnapshotpath: null);

                // Get Non-Default Language.
                context.Labels.Add(baseLabel);
                await context.SaveChangesAsync();

                var languages = await this.context.Languages.AsNoTracking().Where(x => !x.IsDefault).ToListAsync();
                foreach (var language in languages)
                {
                    Label tranlatedLabel = new Label(
                    resourceid: request.ResourceId,
                    fk_labelgroupid: request.FK_LabelGroupId,
                    fk_languageid: language.LanguageId,
                    fK_BaseLabelId: baseLabel.LabelId,
                    labelvalue: null,
                    labeltype: LabelType.Normal,
                    labeldescription: null,
                    labelsnapshotpath: null);

                    string autoTranslation = await this.languageService.AzureTranslate(baseLabel.LabelValue, baseLanguage.LanguageCode, language.LanguageCode);
                    if (!string.IsNullOrEmpty(autoTranslation))
                    {
                        tranlatedLabel.SetMachineTranslation(language.LanguageId, autoTranslation);
                    }
                    context.Labels.Add(tranlatedLabel);
                    await context.SaveChangesAsync();

                    //}
                }
                return 1;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Something went wrong while adding the label: {ex.Message}");
            }
        }

        public async Task<LabelViewModel> UpdateLabel(UpdateLabelCommand request)
        {
            try
            {
                var dbLabel = await this.context.Labels.FirstOrDefaultAsync(x => x.LabelId == request.LabelId);
                if (dbLabel == null)
                {
                    throw new ApiException($"No Label found with Id:  {request.LabelId}");
                }


                var result = context.Labels.Update(dbLabel);
                await context.SaveChangesAsync();
                return this.mapper.Map<LabelViewModel>(dbLabel);
            }
            catch (Exception ex)
            {
                throw new ApiException($"Something went wrong while updating the label: {ex.Message}");
            }
        }
        public async Task<string> ExportLabelsByLanguageId(string languageCode)
        {
            string exportPath = string.Empty;
            try
            {
                var language = await this.context
                    .Languages
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.LanguageCode.ToLower().Trim() == languageCode.ToLower().Trim());
                if (language == null)
                {
                    throw new ApiException($"No Language Found by Lang Code : {languageCode}");
                }

                var allGroups = await this.context
                    .LabelGroups
                    .AsNoTracking()
                    .ToListAsync();

                var parentGroups = allGroups.Where(x => x.FK_ParentLableGroupId == null).ToList();

                var childGroups = allGroups.Where(x => x.FK_ParentLableGroupId != null).ToList();

                var allLabels = await this.context
                    .Labels
                    .AsNoTracking()
                    .Where(x => x.FK_LanguageId == language.LanguageId)
                    .ToListAsync();

                ExportViewModel viewModel = new ExportViewModel();
                foreach (var pGroup in parentGroups)
                {
                    // Current Group Labels.
                    var groupSpecificLabels = allLabels.Where(x => x.FK_LabelGroupId == pGroup.LabelGroupId).ToList();
                    var labelDict = new Dictionary<string, object>();
                    foreach (var lbl in groupSpecificLabels)
                    {
                        labelDict.Add(lbl.ResourceId, lbl.LabelValue ?? String.Empty);
                    }
                    bool hasChild = childGroups.Any(x => x.FK_ParentLableGroupId == pGroup.LabelGroupId);
                    if (hasChild)
                    {
                        BuildChildGroup(pGroup.LabelGroupId, childGroups, allLabels, labelDict);
                    }
                    viewModel.Model.Add(pGroup.GroupName, labelDict);

                }


                string jsonResult = JsonConvert.SerializeObject(viewModel.Model);
                if (!Directory.Exists(CommonSettings.AppSettings.ResoucePath))
                {
                    Directory.CreateDirectory(CommonSettings.AppSettings.ResoucePath);
                }
                exportPath = $"{CommonSettings.AppSettings.ResoucePath}{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString()}.json";
                File.WriteAllText(exportPath, jsonResult);
                if (!File.Exists(exportPath))
                {
                    throw new ApiException($"Unable to generate the file for the lanugage: {language.LanguageId}");
                }
            }
            catch (Exception ex)
            {
                exportPath = string.Empty;
            }
            return exportPath;
        }

        public async Task<bool> ImportLabelsByLanguageId(ImportLabelsByLanguageIdCommand request)
        {
            bool importSuccess = false;
            try
            {
                var dbLanguage = await this.context.Languages.FirstOrDefaultAsync(x => x.LanguageCode.ToLower().Trim() == request.LanguageCode.ToLower().Trim());
                if (dbLanguage == null)
                {
                    throw new ApiException($"No language by code:{request.LanguageCode} exists.");
                }

                if (!Directory.Exists(CommonSettings.AppSettings.ResoucePath))
                {
                    Directory.CreateDirectory(CommonSettings.AppSettings.ResoucePath);
                }
                string filePath = Path.Combine(CommonSettings.AppSettings.ResoucePath, $"Import_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString()}.json");
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.File.CopyToAsync(fileStream);
                }

                string viewModel = File.ReadAllText(filePath);
                bool validJson = Convenience.IsValidJson(viewModel);
                if (!validJson)
                {
                    throw new ApiException($"File does not contain valid json. Please upload the valid file.");
                }
                ImportViewModel importModel = new ImportViewModel();
                var dynamicDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel);

                var allDbGroups = await this.context.LabelGroups.ToListAsync();
                var allDbLabels = await this.context.Labels
                    .Where(x => x.FK_LanguageId == dbLanguage.LanguageId)
                    .ToListAsync();

                // Create a list and map with existing database values out of uploaded json file.
                foreach (KeyValuePair<string, object> entry in dynamicDictionary)
                {
                    var parentGroup = new ImportLabelGroup
                    {
                        GroupName = entry.Key,
                    };

                    var dbGroupexists = allDbGroups
                        .FirstOrDefault(x => x.FK_ParentLableGroupId == null && x.GroupName.ToLower().Trim() == parentGroup.GroupName.ToLower().Trim());
                    if (dbGroupexists != null)
                    {
                        parentGroup.LabelGroupId = dbGroupexists.LabelGroupId;
                    }
                    string value = entry.Value.ToString();

                    if (Convenience.IsValidJson(value))
                    {
                        var childDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(entry.Value?.ToString());
                        PopulateGroup(parentGroup, childDictionary, allDbGroups, allDbLabels, dbLanguage);
                    }
                    else
                    {
                        ImportLabels impLabel = new ImportLabels { LabelName = entry.Key, LabelValue = entry.Value.ToString() };
                        if (parentGroup.LabelGroupId > 0)
                        {
                            var dbLabelExists = allDbLabels
                                .FirstOrDefault(x => x.FK_LabelGroupId == parentGroup.LabelGroupId
                                && x.ResourceId.ToLower().Trim() == entry.Key.ToString().ToLower().Trim() && x.FK_LanguageId == dbLanguage.LanguageId);
                            if (dbLabelExists != null)
                            {
                                impLabel.LabelId = dbLabelExists.LabelId;
                            }
                        }
                        parentGroup.Labels.Add(impLabel);
                    }
                    importModel.ImportLabelGroups.Add(parentGroup);
                }


                // Database Operations Start.

                // Remove Parent Group Case.
                if (dbLanguage.IsDefault)
                {
                    List<int> requestGroupIds = importModel.ImportLabelGroups.Where(x => x.LabelGroupId > 0).Select(x => x.LabelGroupId).ToList();
                    var tobeRemovedGroups = allDbGroups.Where(x => x.FK_ParentLableGroupId == null && !requestGroupIds.Any(y => y == x.LabelGroupId));
                    this.context.LabelGroups.RemoveRange(tobeRemovedGroups);
                }
                foreach (var parentGroup in importModel.ImportLabelGroups)
                {
                    var dbGroupExists = allDbGroups.FirstOrDefault(x => x.LabelGroupId == parentGroup.LabelGroupId);

                    if (parentGroup.LabelGroupId > 0 && dbGroupExists != null)
                    {
                        // Update Parent Group Case. (Todo: This will never happen)
                        if (dbLanguage.IsDefault)
                        {
                            dbGroupExists.SetGroupName(parentGroup.GroupName);
                        }
                    }
                    else
                    {
                        // Add Parent Group Case.
                        if (dbLanguage.IsDefault)
                        {
                            dbGroupExists = new LabelGroup(groupname: parentGroup.GroupName, parentgroupid: null);
                            this.context.LabelGroups.Add(dbGroupExists);
                        }
                    }

                    // Handle Group Labels.
                    this.HandleGroupLabels(allDbLabels, parentGroup, dbGroupExists, dbLanguage);

                    // Loop Through the child Groups.
                    if (parentGroup.ChildGroups.Count > 0)
                    {
                        HandleChildGroup(allDbGroups, allDbLabels, parentGroup, dbGroupExists, dbLanguage);
                    }



                }

                // Database Operations End.

                await this.context.SaveChangesAsync();

                // Create/Update/Delete corresponding label languages.
                if (dbLanguage.IsDefault)
                {
                    await HandleLangugaeLabels(dbLanguage);
                }
                importSuccess = true;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Invalid Request: {ex.Message}.");

            }
            return importSuccess;
        }

        public async Task<string> ExportLabelsByLanguageAndGroup(string languageCode, int labelGroupId)
        {
            string exportPath = string.Empty;
            try
            {
                var language = await this.context
                    .Languages
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.LanguageCode.ToLower().Trim() == languageCode.ToLower().Trim());
                if (language == null)
                {
                    throw new ApiException($"No Language Found by Lang Code : {languageCode}");
                }

                var specificGroup = await this.context
                    .LabelGroups
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.LabelGroupId == labelGroupId);
                if (specificGroup == null)
                {
                    throw new ApiException($"No Group Found by Id : {labelGroupId}");
                }
                var childGroups = this.context.LabelGroups.AsNoTracking()
                    .Where(x => x.FK_ParentLableGroupId != null).ToList();

                var allLabels = await this.context
                    .Labels
                    .AsNoTracking()
                    .Where(x => x.FK_LanguageId == language.LanguageId)
                    .ToListAsync();

                ExportViewModel viewModel = new ExportViewModel();
                // Current Group Labels.

                var groupSpecificLabels = allLabels.Where(x => x.FK_LabelGroupId == specificGroup.LabelGroupId).ToList();
                var labelDict = new Dictionary<string, object>();
                foreach (var lbl in groupSpecificLabels)
                {
                    labelDict.Add(lbl.ResourceId, lbl.LabelValue ?? String.Empty);
                }
                bool hasChild = childGroups.Count > 0;
                if (hasChild)
                {
                    BuildChildGroup(specificGroup.LabelGroupId, childGroups, allLabels, labelDict);
                }
                viewModel.Model.Add(specificGroup.GroupName, labelDict);
                


                string jsonResult = JsonConvert.SerializeObject(viewModel.Model);
                if (!Directory.Exists(CommonSettings.AppSettings.ResoucePath))
                {
                    Directory.CreateDirectory(CommonSettings.AppSettings.ResoucePath);
                }
                exportPath = $"{CommonSettings.AppSettings.ResoucePath}{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString()}.json";
                File.WriteAllText(exportPath, jsonResult);
                if (!File.Exists(exportPath))
                {
                    throw new ApiException($"Unable to generate the file for the lanugage: {language.LanguageId}");
                }
            }
            catch (Exception ex)
            {
                exportPath = string.Empty;
            }
            return exportPath;
        }

        private async Task HandleLangugaeLabels(Language dbLanguage)
        {
            try
            {
                var allLabels = await this.context.Labels.ToListAsync();
                var allNonDefaultLanguages = await this.context.Languages
                    .AsNoTracking()
                    .Where(x => !x.IsDefault)
                    .ToListAsync();

                var defaultLabels = allLabels.Where(x => x.FK_LanguageId == dbLanguage.LanguageId).ToList();
                List<int> baseLabeldIds = defaultLabels.Select(x => x.LabelId).ToList();
                // Remove all language labels where corresponding default labels are removed.
                var tobeRemovedLabels = allLabels.Where(x =>
                x.FK_BaseLabelId != null
                && x.FK_LanguageId != dbLanguage.LanguageId
                && !baseLabeldIds.Any(y => y == x.FK_BaseLabelId)).ToList();

                context.Labels.RemoveRange(tobeRemovedLabels);

                foreach (var defLabel in defaultLabels)
                {
                    foreach (var nonDefLanguage in allNonDefaultLanguages)
                    {
                        var langaugeLabelExists = allLabels.FirstOrDefault(x => x.FK_BaseLabelId == defLabel.LabelId && x.FK_LanguageId == nonDefLanguage.LanguageId);
                        if (langaugeLabelExists == null)
                        {
                            langaugeLabelExists = new Label(
                            resourceid: defLabel.ResourceId,
                            fk_labelgroupid: defLabel.FK_LabelGroupId,
                            fk_languageid: nonDefLanguage.LanguageId,
                            fK_BaseLabelId: defLabel.LabelId,
                            labelvalue: null,
                            labeltype: LabelType.Normal,
                            labeldescription: null,
                            labelsnapshotpath: null);

                            string autoTranslation =
                                $"{nonDefLanguage.Name}_{defLabel.LabelValue}";
                            // (Todo: enable translation)
                            //await this.languageService.AzureTranslate(defLabel.LabelValue, dbLanguage.LanguageCode, nonDefLanguage.LanguageCode);
                            if (!string.IsNullOrEmpty(autoTranslation))
                            {
                                langaugeLabelExists.UpdateLabelValueAndSetStatus(autoTranslation, TranslationStatus.Started);
                                //langaugeLabelExists.SetMachineTranslation(nonDefLanguage.LanguageId, autoTranslation);
                            }
                            context.Labels.Add(langaugeLabelExists);
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(langaugeLabelExists.MachineTranslation))
                            {
                                string autoTranslation = $"{nonDefLanguage.Name}_{defLabel.LabelValue}";
                                // (Todo: enable translation)
                                //await this.languageService.AzureTranslate(defLabel.LabelValue, nonDefLanguage.LanguageCode, nonDefLanguage.LanguageCode);
                                if (!string.IsNullOrEmpty(autoTranslation))
                                {
                                    langaugeLabelExists.UpdateLabelValueAndSetStatus(autoTranslation, TranslationStatus.Started);
                                    //langaugeLabelExists.SetMachineTranslation(nonDefLanguage.LanguageId, autoTranslation);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            await this.context.SaveChangesAsync();
        }
        private void HandleGroupLabels(List<Label> allDbLabels, ImportLabelGroup importLabelGroup, LabelGroup dbGroup, Language uploadLanguage)
        {
            // Remove label Case.
            if (uploadLanguage.IsDefault)
            {
                List<int> requestLabelIds = importLabelGroup.Labels.Where(x => x.LabelId > 0).Select(x => x.LabelId).ToList();
                var tobeRemovedLabels = allDbLabels.Where(x => x.FK_LabelGroupId == importLabelGroup.LabelGroupId && !requestLabelIds.Any(y => y == x.LabelId));
                this.context.Labels.RemoveRange(tobeRemovedLabels);
            }

            foreach (var lbl in importLabelGroup.Labels)
            {
                var dbLabelExists = allDbLabels.FirstOrDefault(x =>
                x.FK_LabelGroupId == importLabelGroup.LabelGroupId
                && x.LabelId == lbl.LabelId);

                if (dbLabelExists != null)
                {
                    // Update label Case. 
                    dbLabelExists.UpdateLabelValueAndSetStatus(lbl.LabelValue, TranslationStatus.Started);
                }
                else
                {
                    // Add label Case.
                    if (uploadLanguage.IsDefault)
                    {
                        dbLabelExists = new Label(
                        resourceid: lbl.LabelName,
                        fk_languageid: uploadLanguage.LanguageId,
                        labelGroup: dbGroup,
                        labelvalue: lbl.LabelValue);
                        this.context.Labels.Add(dbLabelExists);
                    }
                }
            }

        }
        private void HandleChildGroup(List<LabelGroup> allDbGroups, List<Label> allDbLabels, ImportLabelGroup parentGroup, LabelGroup dbParentGrpup, Language uploadLanguage)
        {
            // Remove Group Case.
            if (uploadLanguage.IsDefault)
            {
                List<int> requestGroupIds = parentGroup.ChildGroups.Where(x => x.LabelGroupId > 0).Select(x => x.LabelGroupId).ToList();
                var tobeRemovedGroups = allDbGroups.Where(x => x.FK_ParentLableGroupId == parentGroup.LabelGroupId && !requestGroupIds.Any(y => y == x.LabelGroupId));
                this.context.LabelGroups.RemoveRange(tobeRemovedGroups);
            }

            foreach (var cGroup in parentGroup.ChildGroups)
            {
                var dbGroupExists = allDbGroups.FirstOrDefault(x =>
                x.FK_ParentLableGroupId == parentGroup.LabelGroupId
                && x.LabelGroupId == cGroup.LabelGroupId);

                if (dbGroupExists != null)
                {
                    // Update Group Case. (Todo: This will never happen)
                    dbGroupExists.SetGroupName(cGroup.GroupName);
                }
                else
                {
                    // Add Group Case.
                    if (uploadLanguage.IsDefault)
                    {
                        dbGroupExists = new LabelGroup(cGroup.GroupName, dbParentGrpup);
                        this.context.LabelGroups.Add(dbGroupExists);
                    }
                }
                // Handle Group Labels.
                this.HandleGroupLabels(allDbLabels, cGroup, dbGroupExists, uploadLanguage);

                // Loop Through the child Groups.
                if (cGroup.ChildGroups.Count > 0)
                {
                    HandleChildGroup(allDbGroups, allDbLabels, cGroup, dbGroupExists, uploadLanguage);
                }
            }
        }

        private void BuildChildGroup(int parentGroupId, List<LabelGroup> childGroups, List<Label> allLabels, Dictionary<string, object> node)
        {
            var childGrouups = childGroups.Where(x => x.FK_ParentLableGroupId == parentGroupId).ToList();
            foreach (var cGroup in childGrouups)
            {
                // Current Group Labels.
                var groupSpecificLabels = allLabels.Where(x => x.FK_LabelGroupId == cGroup.LabelGroupId).ToList();
                var labelDict = new Dictionary<string, object>();
                foreach (var lbl in groupSpecificLabels)
                {
                    labelDict.Add(lbl.ResourceId, lbl.LabelValue ?? String.Empty);
                }
                node.Add(cGroup.GroupName, labelDict);
                bool hasChild = childGroups.Any(x => x.FK_ParentLableGroupId == cGroup.LabelGroupId);
                if (hasChild)
                {
                    BuildChildGroup(cGroup.LabelGroupId, childGroups, allLabels, labelDict);
                }
            }
        }

        private void PopulateGroup(ImportLabelGroup parentGroup, Dictionary<string, object> childValues, List<LabelGroup> allDbGroups, List<Label> allDbLabels, Language uploadLanguage)
        {
            if (childValues != null && childValues.Count > 0)
            {
                foreach (KeyValuePair<string, object> entry in childValues)
                {
                    string value = entry.Value.ToString();
                    if (Convenience.IsValidJson(value))
                    {
                        var childGroup = new ImportLabelGroup
                        {
                            GroupName = entry.Key,
                        };
                        if (parentGroup.LabelGroupId > 0)
                        {
                            var dbGroupexists = allDbGroups.FirstOrDefault(x => x.FK_ParentLableGroupId == parentGroup.LabelGroupId
                            && x.GroupName.ToLower().Trim() == childGroup.GroupName.ToLower().Trim());
                            if (dbGroupexists != null)
                            {
                                childGroup.LabelGroupId = dbGroupexists.LabelGroupId;
                            }
                        }
                        var childDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(entry.Value?.ToString());
                        PopulateGroup(childGroup, childDictionary, allDbGroups, allDbLabels, uploadLanguage);
                        parentGroup.ChildGroups.Add(childGroup);
                    }
                    else
                    {
                        ImportLabels impLabel = new ImportLabels { LabelName = entry.Key, LabelValue = entry.Value.ToString() };
                        if (parentGroup.LabelGroupId > 0)
                        {
                            var dbLabelExists = allDbLabels
                                .FirstOrDefault(x => x.FK_LabelGroupId == parentGroup.LabelGroupId
                                && x.ResourceId.ToLower().Trim() == entry.Key.ToString().ToLower().Trim()
                                && x.FK_LanguageId == uploadLanguage.LanguageId);
                            if (dbLabelExists != null)
                            {
                                impLabel.LabelId = dbLabelExists.LabelId;
                            }
                        }
                        parentGroup.Labels.Add(impLabel);
                    }
                }
            }
        }


    }
}
