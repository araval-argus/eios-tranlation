using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_translation.core.Common
{
    public enum TranslationStatus
    {
        Started = 0,
        UnderReview = 1,
        Reviewed = 2,
        Published = 3
    }

    public enum LabelType
    {
        Normal = 0,
        PreLabel = 1,
        PostLabel = 2
    }

    public enum ToleranceType
    {
        Absolute = 0,
        Relative = 1
    }
}
