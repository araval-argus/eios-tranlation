using AutoMapper;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.core.Wrappers;
using eios_translation.infrastructure.DbContext;
using eios_translation.infrastructure.EntityClass;
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
        public async Task<List<LabelGroupViewModel>> GetAllLabelGroups()
        {
            var labelGroups = await context.LabelGroups
                .Include(x => x.ParentGroup)
                .AsNoTracking().ToListAsync();
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

        public async Task<int> InsertLabelGroup(LabelGroupViewModel labelgroup)
        {
            try
            {
                this.context.LabelGroups.Add(this.mapper.Map<LabelGroup>(labelgroup));
                await context.SaveChangesAsync();
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> UpdateLabelGroup(LabelGroupViewModel labelgroup)
        {
            try
            {
                this.context.LabelGroups.Update(this.mapper.Map<LabelGroup>(labelgroup));
                await context.SaveChangesAsync();
                return 1;
            }
            catch
            { 
                return 0;
            }
        }
    }
}
