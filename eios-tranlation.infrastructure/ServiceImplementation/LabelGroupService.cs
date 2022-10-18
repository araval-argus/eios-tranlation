using AutoMapper;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_translation.businesslogic.Features.Label.ViewModels;
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
            var labelGroups = await context.LabelGroups.AsNoTracking().ToListAsync();
            return this.mapper.Map<List<LabelGroupViewModel>>(labelGroups);
        }

        public LabelGroupViewModel GetSelectedLabelGroup(int LabelGroupId)
        {
            var result = this.context.LabelGroups.First(a => a.LabelGroupId == LabelGroupId);
            return this.mapper.Map<LabelGroupViewModel>(result);
        }

        public int InsertLabelGroup(LabelGroupViewModel labelgroup)
        {
            try
            {
                this.context.LabelGroups.Add(this.mapper.Map<LabelGroup>(labelgroup));
                context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int UpdateLabelGroup(LabelGroupViewModel labelgroup)
        {
            try
            {
                this.context.LabelGroups.Update(this.mapper.Map<LabelGroup>(labelgroup));
                context.SaveChanges();
                return 1;
            }
            catch
            { 
                return 0;
            }
        }
    }
}
