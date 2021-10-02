using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace work.Enums
{
    [Flags]
    public enum EnumProductAvailability
    {
        [Display(Name = "Disponivel")]
        Disponivel = 1,

        [Display(Name = "Indisponivel")]
        Indisponivel = 2,
    }
}
