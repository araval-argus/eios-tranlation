using AutoMapper;
using eios_tranlation.businesslogic.Features.LabelGroup;
using eios_tranlation.businesslogic.Features.LabelGroup.ViewModels;
using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.core.Wrappers;
using eios_translation.infrastructure.DbContext;
using eios_translation.infrastructure.EntityClass;
using Google.Cloud.Translation.V2;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eios_tranlation.infrastructure.ServiceImplementation
{
    public class LabelGroupService : ILabelGroupService
    {
        private readonly EIOSTranslationContext context;
        private readonly IMapper mapper;

        public LabelGroupService(EIOSTranslationContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<List<LabelGroupViewModel>> GetAllLabelGroups(bool onlyParent = false)
        {

            var labelGroups = new List<LabelGroup>();
            if (!onlyParent)
            {
                labelGroups = await context.LabelGroups
                    .Include(x => x.ParentGroup)
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {
                labelGroups = await context.LabelGroups
                    .Where(x => x.FK_ParentLableGroupId == null)
                    .AsNoTracking()
                    .ToListAsync();
            }
            return this.mapper.Map<List<LabelGroupViewModel>>(labelGroups);
        }
        public async Task<LabelGroupDetailViewModel> GetLabelGroupDetailsById(int labelGroupId)
        {
            LabelGroupDetailViewModel response = new LabelGroupDetailViewModel();
            var dbGroup = await this.context.LabelGroups
               .AsNoTracking()
               .Include(x => x.ParentGroup)
               .FirstOrDefaultAsync(a => a.LabelGroupId == labelGroupId);
            if (dbGroup == null)
            {
                throw new ApiException($"No Group Found with the Id: {labelGroupId}");
            }

            // Fill Basic Details.
            response.LabelGroupId = dbGroup.LabelGroupId;
            response.FK_ParentLableGroupId = dbGroup.FK_ParentLableGroupId;
            response.GroupName = dbGroup.GroupName;

            // Fill Parent Group Details.
            if (dbGroup.ParentGroup != null)
            {
                response.ParentGroup = this.mapper.Map<LabelGroupViewModel>(dbGroup.ParentGroup);
            }

            // Fill Child Group Details.
            var dbChildGroups = await this.context.LabelGroups
               .AsNoTracking()
               .Where(a => a.FK_ParentLableGroupId == dbGroup.LabelGroupId)
               .ToListAsync();

            if (dbChildGroups.Count > 0)
            {
                response.ChildGroups = this.mapper.Map<List<LabelGroupViewModel>>(dbChildGroups);
            }

            // Fill all languages with Labels.
            var dbLanguages = await this.context.Languages
               .AsNoTracking()
               .ToListAsync();

            var dbLabels = await this.context.Labels
               .AsNoTracking()
               .Where(x => x.FK_LabelGroupId == dbGroup.LabelGroupId)
               .ToListAsync();

            var defaultLang = dbLanguages.FirstOrDefault(x => x.IsDefault);
            if (defaultLang?.LanguageId > 0)

            {
                int defaultLangId = defaultLang.LanguageId;
                foreach (var dbLabel in dbLabels.Where(x => x.FK_LanguageId == defaultLang.LanguageId))
                {
                    LabelWithLanguage label = new LabelWithLanguage { };
                    label.FK_LabelGroupId = dbLabel.FK_LabelGroupId;
                    label.LanguageId = dbLabel.FK_LanguageId;
                    label.LanguageName = defaultLang.Name;
                    label.LabelName = dbLabel.ResourceId;
                    label.LabelValue = dbLabel.LabelValue;
                    label.MachineTranslation = dbLabel.MachineTranslation;

                    foreach (var lang in dbLanguages.Where(x => !x.IsDefault))
                    {
                        var langLabel = dbLabels.FirstOrDefault(x=>x.FK_LanguageId == lang.LanguageId && x.FK_BaseLabelId == dbLabel.LabelId);
                        if (langLabel != null)
                        {
                            LabelWithLanguage translatedLabel = new LabelWithLanguage();
                            translatedLabel.FK_LabelGroupId = langLabel.FK_LabelGroupId;
                            translatedLabel.LanguageId = langLabel.FK_LanguageId;
                            translatedLabel.LanguageName = lang.Name;
                            translatedLabel.LabelName = langLabel.ResourceId;
                            translatedLabel.LabelValue = langLabel.LabelValue;
                            translatedLabel.MachineTranslation = langLabel.MachineTranslation;
                            label.TranslatedLabels.Add(translatedLabel);
                        }
                    }
                    response.Labels.Add(label);
                }
            }

            return response;
        }

        public async Task<LabelGroupDetailViewModel> SaveLabelGroupDetailsById(SaveLabelGroupDetailsByIdCommand request)
        {
            LabelGroupDetailViewModel response = await GetLabelGroupDetailsById(request.LabelGroupId);

            //var dbLabels = await this.context.Labels
            //  .Where(x => x.FK_LabelGroupId == request.LabelGroupId)
            //  .ToListAsync();

            //// TODO: Allow Default Lang Update??
            //foreach (var reqLang in request.LanguageLabels.Where(x => !x.IsDefault))
            //{
            //
            //    foreach (var reqLabel in reqLang.Labels.Where(x => !string.IsNullOrWhiteSpace(x.LabelValue)))
            //    {
            //        try
            //        {
            //            var findDbLabel = dbLabels.FirstOrDefault(x => x.LabelId == reqLabel.LabelId);
            //            if (findDbLabel != null)
            //            {
            //                findDbLabel.UpdateLabelValue(reqLabel.LabelValue);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //        }
            //    }
            //}
            //await context.SaveChangesAsync();
            return response;
        }

        public async Task<LabelGroupViewModel> GetSelectedLabelGroup(int LabelGroupId)
        {
            var result = await this.context.LabelGroups
                .Include(x => x.ParentGroup)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.LabelGroupId == LabelGroupId);
            if (result == null)
            {
                throw new ApiException($"No Group Found with the Id: {LabelGroupId}");
            }
            return this.mapper.Map<LabelGroupViewModel>(result);
        }

        public async Task<int> InsertLabelGroup(InsertLabelGroupCommand labelgroup)
        {
            try
            {
                LabelGroup group = new LabelGroup(labelgroup.GroupName, labelgroup.FK_ParentLableGroupId);
                this.context.LabelGroups.Add(group);
                await context.SaveChangesAsync();
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<LabelGroupViewModel> UpdateLabelGroup(UpdateLabelGroupCommand labelgroup)
        {
            try
            {
                var dbLabelGroup = await this.context.LabelGroups.FirstOrDefaultAsync(x => x.LabelGroupId == labelgroup.LabelGroupId);
                if (dbLabelGroup == null)
                {
                    throw new ApiException($"No Label Group found with Id:  {labelgroup.LabelGroupId}");
                }
                dbLabelGroup.UpdateLabelGroup(labelGroupId: labelgroup.LabelGroupId, groupName: labelgroup.GroupName, parentLableGroupId: labelgroup.FK_ParentLableGroupId);
                await context.SaveChangesAsync();
                return this.mapper.Map<LabelGroupViewModel>(dbLabelGroup);
            }
            catch (Exception ex)
            {
                throw new ApiException($"Something went wrong while updating the label group: {ex.Message}");
            }
        }
    }
}
