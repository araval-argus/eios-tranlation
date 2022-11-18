using AutoMapper;
using eios_tranlation.businesslogic.Features.LabelGroup.ViewModels;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.infrastructure.EntityClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_translation.infrastructure
{
    public class AutoMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMappingProfile"/> class.
        /// </summary>
        public AutoMappingProfile()
        {
            this.CreateMap<Label, LabelViewModel>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            this.CreateMap<LabelViewModel, Label>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            this.CreateMap<Language, LanguageViewModel>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            this.CreateMap<LanguageViewModel, Language>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            this.CreateMap<LabelGroup, LabelGroupViewModel>()
           .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            this.CreateMap<LabelGroupViewModel, LabelGroup>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
