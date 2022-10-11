﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_translation.infrastructure.EntityClass
{
    public class Language
    {
        [Key]
        public int LanguageId { get; protected set; }
        public string Name { get; protected set; }
        public string LanguageCode { get; protected set; }
        public double? ToleranceRelative { get; protected set; }
        public int? ToleranceAbsolute { get; protected set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}