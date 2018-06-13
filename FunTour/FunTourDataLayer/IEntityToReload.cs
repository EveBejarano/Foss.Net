using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunTourDataLayer
{
    public interface IEntityToReload
    {
        ////Genera un nuevo objeto que tiene que ser serializado para mandar la request a la API con los parametros ingresados
        //object NewEntity(string _parameters);

        ////Genera los parametros que se necesitan para la request y los devuelve en formato string
        //string GenerateParameters();

        //Toma el string de respuesta de una API y lo convierte en el tipo necesario para guardar en la BD.
        IEnumerable<object> DesearializeJson(string data);

        //Metodo llamado por DeserializeJson que recibe un objeto que contiene una lista de objetos y devuelve la lista de objetos contenida
        IEnumerable<object> MappingJson(object APIResponse);

        void ReLoadTable();
    }
}
