using AutoMapper;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.ServiceInterfaces;
using eios_translation.infrastructure.DbContext;
using eios_translation.infrastructure.EntityClass;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_translation.infrastructure.ServiceImplementation
{
    public class LabelService : ILabelService
    {
        private readonly EIOSTranslationContext context;
        private readonly IMapper mapper;

        public LabelService(EIOSTranslationContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<LabelViewModel>> GetAllLabels(int languageId)
        {
            var labels = await this.context.Labels.AsNoTracking().ToListAsync();
            return this.mapper.Map<List<LabelViewModel>>(labels);
        }

        public async Task<LabelViewModel> GetSelectedLabel(int LabelId)
        {
            var result = await this.context.Labels.FirstAsync(a => a.LabelId == LabelId);
            return this.mapper.Map<LabelViewModel>(result);
        }

        public async Task<int> InsertLabel(LabelViewModel label)
        {
            try
            {
                var result =  context.Labels.Add(this.mapper.Map<Label>(label));
                await context.SaveChangesAsync();
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> UpdateLabel(LabelViewModel label)
        {
            try
            {
                var result = context.Labels.Update(this.mapper.Map<Label>(label));
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
