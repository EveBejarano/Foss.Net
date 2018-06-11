using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunTourDataLayer.Locality
{
    public class City
    {
        [Key]
        public int Id_City { get; set; }

        public string Name { get; set; }

        public string CP { get; set; }

        public Province Province { get; set; }
    }
}
