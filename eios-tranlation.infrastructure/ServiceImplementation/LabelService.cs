using AutoMapper;
using eios_tranlation.businesslogic.Features.Label;
using eios_tranlation.businesslogic.Features.Label.ViewModels;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_tranlation.core.Common;
using eios_tranlation.core.Constants;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.ServiceInterfaces;
using eios_translation.core.Common;
using eios_translation.core.Wrappers;
using eios_translation.infrastructure.DbContext;
using eios_translation.infrastructure.EntityClass;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Label = eios_translation.infrastructure.EntityClass.Label;

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
        public async Task<string> ExportLabelsByLanguageId(int languageId)
        {
            string exportPath = string.Empty;
            try
            {
                var language = await this.context
                    .Languages
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.LanguageId == languageId);
                if (language == null)
                {
                    throw new ApiException($"No Language Found by Id : {languageId}");
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
                    .Where(x => x.FK_LanguageId == languageId)
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
                    throw new ApiException($"Unable to generate the file for the lanugage: {languageId}");
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
                    .Where(x => x.FK_LanguageId == request.LanguageId)
                    .ToListAsync();

                foreach (KeyValuePair<string, object> entry in dynamicDictionary)
                {
                    // do something with entry.Value or entry.Key
                    var parentGroup = new ImportLabelGroup
                    {
                        GroupName = entry.Key,
                    };

                    var dbGroupexists = allDbGroups.FirstOrDefault(x => x.FK_ParentLableGroupId == null && x.GroupName.ToLower().Trim() == parentGroup.GroupName.ToLower().Trim());
                    if (dbGroupexists != null)
                    {
                        parentGroup.LabelGroupId = dbGroupexists.LabelGroupId;
                    }
                    string value = entry.Value.ToString();

                    if (Convenience.IsValidJson(value))
                    {
                        var childDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(entry.Value?.ToString());
                        PopulateGroup(parentGroup, childDictionary, allDbGroups, allDbLabels);
                    }
                    else
                    {
                        ImportLabels impLabel = new ImportLabels { LabelName = entry.Key, LabelValue = entry.Value.ToString() };
                        if (parentGroup.LabelGroupId > 0)
                        {
                            var dbLabelExists = allDbLabels
                                .FirstOrDefault(x => x.FK_LabelGroupId == parentGroup.LabelGroupId 
                                && x.ResourceId.ToLower().Trim() == entry.Key.ToString().ToLower().Trim());
                            if (dbLabelExists != null)
                            {
                                impLabel.LabelId = dbLabelExists.LabelId;
                            }
                        }
                        parentGroup.Labels.Add(impLabel);
                    }
                    importModel.ImportLabelGroups.Add(parentGroup);
                }
                string finalJson = JsonConvert.SerializeObject(importModel);
                importSuccess = true;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Invalid Request: {ex.Message}.");

            }
            return importSuccess;
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
        private void PopulateGroup(ImportLabelGroup parentGroup, Dictionary<string, object> childValues, List<LabelGroup> allDbGroups, List<Label> allDbLabels)
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
                        PopulateGroup(childGroup, childDictionary, allDbGroups, allDbLabels);
                        parentGroup.ChildGroups.Add(childGroup);
                    }
                    else
                    {
                        ImportLabels impLabel = new ImportLabels { LabelName = entry.Key, LabelValue = entry.Value.ToString() };
                        if (parentGroup.LabelGroupId > 0)
                        {
                            var dbLabelExists = allDbLabels
                                .FirstOrDefault(x => x.FK_LabelGroupId == parentGroup.LabelGroupId
                                && x.ResourceId.ToLower().Trim() == entry.Key.ToString().ToLower().Trim());
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
