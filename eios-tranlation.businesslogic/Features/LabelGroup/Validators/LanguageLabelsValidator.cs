using eios_tranlation.businesslogic.Features.LabelGroup.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_tranlation.businesslogic.Features.LabelGroup.Validators
{
    public class LanguageLabelsValidator : AbstractValidator<LanguageAndLabelDetails>
    {
        public LanguageLabelsValidator()
        {
            this.RuleFor(x => x.LanguageId).GreaterThan(0);
            this.RuleForEach(x => x.Labels).SetValidator(new LabelDetailsValidator());

        }
    }
}
