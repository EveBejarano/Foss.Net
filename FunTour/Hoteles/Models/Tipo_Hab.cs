using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hoteles.Models
{
    public class Tipo_Hab
    {
        [Key]
        public string IDTipohab { get; set; }
        public decimal tarifa_estandar { get; set; }
        public string descripcion_hab { get; set; }
    }
}
