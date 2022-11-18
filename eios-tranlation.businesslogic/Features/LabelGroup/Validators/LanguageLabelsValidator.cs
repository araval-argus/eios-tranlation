using eios_tranlation.businesslogic.Features.LabelGroup.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_tranlation.businesslogic.Features.LabelGroup.Validators
{
    public class LanguageLabelsValidator : AbstractValidator<LabelWithLanguage>
    {
        public LanguageLabelsValidator()
        {
            this.RuleFor(x => x.FK_LanguageId).GreaterThan(0);
            this.RuleFor(x => x.FK_LabelGroupId).GreaterThan(0);
            this.RuleFor(x => x.LabelId).GreaterThan(0);
            this.RuleForEach(x => x.TranslatedLabels).SetValidator(new LanguageLabelsValidator());

        }
    }
}
