# Foss.Net

API_de_Vuelos

Este código emula una api de un sistema de aerolineas en la que se puede obtener todo tipo de informacón concerniente a los vuelos.

La base de datos tiene la siguiente composición:

    CommercialFlights : es la tabla "principal" correspondiente a los vuelos comerciales, tiene informacion como ser, Lugar de salida; Lugar de llegada; Fecha de Salida; Fecha de llegada; Avión del vuelo;
    
    Planes : Contiene información a cerca de los aviones como unidad que puede ser utilizada en un vuelo. Describe modelo, numero identificatorio, capcidad de pasajeros y rango de viaje del avión.
    
    Destinations: podría decirse que es la sagunda tabla mas importante, ya que conserva información proveniente de cada destino posible de un vuelo. Se identifica por el id del areopuerto en el que se produce el arrivo, a su vez incluye info de la localidad del aeropuerto.
    
    FlightPlaces : Es una tabla que por cada entrada representa un asiento de un avión en un vuelo. Hay tantas entidades Places relacionadas a un vuelos como el valor de la capacidad del avión del vuelo. La tabla permite registrar información de quien reserva la plaza. Podría decirse que gracias a esta tabla se manejan las reservas de los vuelos.
    
    ScalesOnFligth: Contiene información de una escala intermedia (si tuviera) en un vuelo en la cual no se produjera trasbordo.
    
    Employees: información de los empleados
    
    FlightPersonal: Vincula los empleados al vuelos indicando el Rol que cumplen en el mismo.

:-------------------------------------------------------------------------------------------------;

En la aplicación actualmente se pueden utilizar los métodos CRUD para cada una de las tablas.

---

# Api Eventos

Representacion de la API correspondiente a un servicio de organizacion de eventos de terceros, el cual cuenta con:

- Events:
  - Representa los distintos aconteciemientos que son organziados por la empresa.
  - Conoce el medio de tranporte con el que cuenta, al igual que la ciudad, y las entradas con las que cuenta.
- Transports:
  - Representa los distitnos medios de transporte con los que cuenta la empresa.
- Cities:
  - Representa Las ciudades en las cuales se llevan a cabo los eventos.
  - Conoce  al pais con elq ue se encuentra asociado.
- Persons: 
  - Representa los clientes que compraron las entradas 
  - Conocer al Ticket que compro
- Tickets: 
  - Representa las entradas a los diferentes eventos.
  - Conoce tanto a la Persona que los compro como al evento al cual pertenecen.

------

Esta API permite no solo la carga de Tickets comprados para un determinado evento, sino tambien la obtencion de la informacion de los Eventos disponibles, ya sea todos, o bien filtrados por ID, Ciudad, por un precio maximo, y por fecha, sea la misma una especifica o un rango entre dos fechas.

Cuenta tambien con la posibilidad de realizar altas, bajas y modificaciones de: Eventos, Ciudades, Clientes, Transportes, Ciudades, y Paises, aprovechando las ventajas de las vistas Web. 

------

### Listado de Eventos

Route/api/Events => Devuelve la lista completa de eventos con toda su informacion

Route/api/Events/{id} => Devuelve los detalles del evento corresponiente con el id

Route/api/EventsWithTickets => Devuelve el listado de todos los eventos con tickets disponibles

Route/api/EventsInCity => Devuelve el listado de eventos en una ciudad

Route/api/EventsByDate => Devuelve el lisrado de eventos en una fecha

Route/api/Events/{Date1}/{Date2} => Devuelve el listado de eventos etre dos fechas

Route/api/Events/{maxPrice} => Devuelve el listado de eventos con un costo menor a un precio maximo

------

### Model de Json de compra de tickets

```json
{
  "TicketID": 1,
  "Price": 2.1,
  "EventWithTicketID": 1,
  "DNI": "39186190",
  "Person": {
    "DNI": "39186190",
    "Name": "Wenceslao",
    "Surname": "Marquez"
  }
}
```

Route/api/Tickets