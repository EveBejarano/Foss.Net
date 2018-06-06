using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunTourDataLayer.Locality
{
    public class Province
    {
        public int Id_Province { get; set; }

        public string Name { get; set; }

        public IEnumerable<City> Cities { get; set; }
    }
}
