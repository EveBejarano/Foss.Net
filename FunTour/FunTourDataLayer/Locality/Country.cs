using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FunTourDataLayer.Locality
{
    public class Country
    {
        [Key]
        public int Id_Country { get; set; }
        public string Name { get; set; }
        public IEnumerable<Province> Provinces { get; set; }
    }
}
