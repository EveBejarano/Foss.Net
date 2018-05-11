# Foss.Net

# Hoteles

Simula una API que devuelve listas de datos de hoteles, según el nombre de la ciudad que se pase como parámetro.
Permite ver los detalles completos de un hotel pasando su ID, incluyendo los tipos de habitaciones que tiene.
Funcionalidad que falta: ver disponibilidad de habitaciones según las fechas.
Permite dar de alta reservas, pasando los datos necesarios para su creación.

------

La base de datos está compuesta por:
-Agent: información de los agentes de turismo externos que pueden crear reservas en el sistema.
-Booking: información de las reservas de habitaciones de hotel, identifica agente que realizó la reserva,
huésped que se alojará, habitación que se reserva, estado de la reserva y fechas entre las cuales se reserva.
-BookingStatus: datos de los estados en los que puede estar una reserva.
-Country: información de los países en los cuales se tienen hoteles, incluyendo nombre del paí y la moneda que utiliza.
-Guest: información de los huéspedes que tuvieron reservas asociadas por lo menos una vez.
-Hotel: información de los hoteles incluyendo nombre, cadena a la que pertenece, dirección, país, 
ciudad, email, website, clasificación por estrellas y otros detalles del hotel. Tiene tipos de habitación asociados. 
-HotelChain: datos de las cadenas de hoteles con las que se trabaja, incluyendo su nombre y su logo.
-Room: información de cada habitación que se puede reservar, cada una tiene un tipo de habitación, número y otros posibles
detalles. Se asocia con su hotel a través del tipo de habitación.
-RoomType: información sobre los tipos de habitación que posee un hotel, con su descripción y tarifa estándar.
-StarRating: datos de las categorías de hotel por estrellas, con su representación gráfica.

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
