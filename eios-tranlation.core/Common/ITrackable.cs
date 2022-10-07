using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_translation.core.Common
{
    public interface ITrackable
    {
        /// <summary>
        /// Gets or sets CreatedAt.
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets CreatedBy.
        /// </summary>
        int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets LastUpdatedAt.
        /// </summary>
        DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets LastUpdatedBy.
        /// </summary>
        int UpdatedBy { get; set; }
    }
}
