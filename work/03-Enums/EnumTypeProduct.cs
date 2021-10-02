using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;
using System.Reflection;


namespace work.Enums
{
    [Flags]
    public enum EnumTypeProduct
    {
        [Display(Name = "Congelados")]
        Congelados = 1,

        [Display(Name = "Prateleira")]
        Prateleira = 2,

        [Display(Name = "Enlatados")]
        Enlatados = 3,
    }
}
