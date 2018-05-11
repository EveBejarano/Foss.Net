using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hoteles.Models;

namespace Hoteles.Data
{
    public class DbInitializer
    {
        public static void Initialize(HotelContext context)
        {
            context.Database.EnsureCreated();

            // Look for any hotels.
            if (context.Hotel.Any())
            {
                return;   // DB has been seeded
            }

            var hoteles = new Hotel[]
            {
            new Hotel{nombre_hotel="Exe Hotel Colón",direccion_hotel="Carlos Pellegrini, 507, C1009ABK Buenos Aires, Argentina",ciudad_hotel="Buenos Aires"},
            new Hotel{nombre_hotel="Wilton Hotel Buenos Aires",direccion_hotel="Av. Callao, 1162, Recoleta, C1023AAR Buenos Aires, Argentina", ciudad_hotel="Buenos Aires"},
            new Hotel{nombre_hotel="Épico Recoleta Hotel",direccion_hotel="Laprida 1910, Recoleta, 1425 Buenos Aires, Argentina",ciudad_hotel="Buenos Aires"},
            new Hotel{nombre_hotel="Ker Recoleta Hotel & Spa",direccion_hotel="Marcelo T. de Alvear, 1368, 1097 Buenos Aires, Argentina",ciudad_hotel="Buenos Aires"},
            new Hotel{nombre_hotel="A Hotel",direccion_hotel="Azcuenaga, 1268, Recoleta, 1115 Buenos Aires, Argentina",ciudad_hotel="Buenos Aires"},
            new Hotel{nombre_hotel="Globales República",direccion_hotel="Cerrito, 370, C1010AAH Buenos Aires, Argentina",ciudad_hotel="Buenos Aires"},
            new Hotel{nombre_hotel="Callao Suites",direccion_hotel="Av. Callao 1062, Recoleta, 1023 Buenos Aires, Argentina",ciudad_hotel="Buenos Aires"},
            new Hotel{nombre_hotel="Claridge-Hotel",direccion_hotel="Tucumán, 535, 1049 Buenos Aires, Argentina",ciudad_hotel="Buenos Aires"}
            };
            foreach (Hotel s in hoteles)
            {
                context.Hotel.Add(s);
            }

            context.SaveChanges();

        }
    }
}
