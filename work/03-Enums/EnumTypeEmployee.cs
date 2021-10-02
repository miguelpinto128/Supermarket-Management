using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace work.Enums
{
    [Flags]
    public enum EnumTypeEmployee
    {
        [Display(Name = "Gerente")]
        Gerente = 1,

        [Display(Name = "Caixa")]
        Caixa = 2,

        [Display(Name = "Repositor")]
        Repositor = 3,
    }
}
