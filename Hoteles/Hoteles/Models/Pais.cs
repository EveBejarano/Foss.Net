using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hoteles.Models
{
    public class Pais
    {
        [Key]
        public string IDPais { get; set; }
        public string nombre_pais { get; set; }
        public string moneda_pais { get; set; }
        public decimal tasa_cambio { get; set; }
    }
}
