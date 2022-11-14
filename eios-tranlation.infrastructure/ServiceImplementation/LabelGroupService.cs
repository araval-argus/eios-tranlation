using AutoMapper;
using eios_tranlation.businesslogic.Features.LabelGroup;
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
                    .Where(x=>x.FK_ParentLableGroupId == null)
                    .AsNoTracking()
                    .ToListAsync();
            }
            return this.mapper.Map<List<LabelGroupViewModel>>(labelGroups);
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
                dbLabelGroup.UpdateLabelGroup(labelGroupId: labelgroup.LabelGroupId,groupName: labelgroup.GroupName, parentLableGroupId: labelgroup.FK_ParentLableGroupId);
                await context.SaveChangesAsync();
                return this.mapper.Map<LabelGroupViewModel>(dbLabelGroup);
            }
            catch(Exception ex)
            {
                throw new ApiException($"Something went wrong while updating the label group: {ex.Message}");
            }
        }
    }
}
