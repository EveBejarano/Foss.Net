# Foss.Net

#PruebaUsers
Es el sistema de Fantour que (hasta ahora) permite registrarse e iniciar sesión como usuario. Al registrarse, un usuario que es administrador puede crear/editar/listar/eliminar usuarios y roles. Posee las entidades necesarias para la administración de permisos de cada rol y de paquetes pero esto todavía no se encuentra implementado. 

La base de datos esta compuesta por:
- TravelPackage: Representa cada paquete que la empresa ofrece a los clientes y un cliente puede reservar.
- Reservation: Representa una reservacion/compra de un paquete que haya realizado un cliente.
 
- Bus: Representa los colectivos de la empresa que pueden ser asignados a un paquete.
- BusCompany: Representa las compañias de colectivos que brindan servicio de transporte a la empresa Fantour.
- BusReservedSeat: Representa el asiento de colectivo reservado por un cliente.

- Event:Representa los eventos de la empresa que pueden ser asignados a un paquete.
- ReservedTicket: Representa el ticker de un evento reservado por un cliente.

- Flight:Representa los vuelos de la empresa que pueden ser asignados a un paquete.
- FlightCompany: Representa las compañias de vuelos que brindan servicio de transporte a la empresa Fantour.
- ReservedSeat: Representa el asiento de avion reservado por un cliente.

- Hotel: Representa a los hoteles que brindan servicio de alojamiento a la empresa Fantour.
- Reservation Room: Representa la habitación reservada por un cliente.

- IdentityUsers: Representa los datos necesarios para el registro y el logueo de un usuario.
- UserDetails:Representa los datos necesarios de un usuario.

- AspNetUserRoles:Representa la tabla intermedia entre los roles y los usuarios ya que un usuario puede tener muchos roles y un rol puede estar asignado a multiples usuarios.
 
- AspNetRoles: Representa el nombre y el id de un rol.
- RoleDetails: Posee información adicional de cada Rol.
- Permission: Representa los permisos que puede llegar a poseer un rol.

# API Hoteles

Simula una API que devuelve listas de datos de hoteles, según el nombre de la ciudad que se pase como parámetro.
Permite ver los detalles completos de un hotel pasando su ID, incluyendo los tipos de habitaciones que tiene.
Funcionalidad que falta: ver disponibilidad de habitaciones según las fechas.
Permite dar de alta reservas, pasando los datos necesarios para su creación.

------

La base de datos está compuesta por:

- Agent:
  - Información de los agentes de turismo externos que pueden crear reservas en el sistema.
- Booking:
  - Información de las reservas de habitaciones de hotel, identifica agente que realizó la reserva,
    huésped que se alojará, habitación que se reserva, estado de la reserva y fechas entre las cuales se reserva.
- Booking Status:
  - Datos de los estados en los que puede estar una reserva.
- Country: 
  - Información de los países en los cuales se tienen hoteles, incluyendo nombre del paí y la moneda que utiliza.
- Guest: 
  - Información de los huéspedes que tuvieron reservas asociadas por lo menos una vez.
- Hotel: 
  - información de los hoteles incluyendo nombre, cadena a la que pertenece, dirección, país, 
    ciudad, email, website, clasificación por estrellas y otros detalles del hotel. Tiene tipos de habitación asociados. 
- HotelChain:
  - Datos de las cadenas de hoteles con las que se trabaja, incluyendo su nombre y su logo.
- Room:
  - Información de cada habitación que se puede reservar, cada una tiene un tipo de habitación, número y otros posibles
    detalles. Se asocia con su hotel a través del tipo de habitación.
- RoomType:
  - Información sobre los tipos de habitación que posee un hotel, con su descripción y tarifa estándar.
- StarRating:
  - Datos de las categorías de hotel por estrellas, con su representación gráfica.

------

# API Vuelos

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

# API Eventos

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

---

# BusAPI

API que permite simular la gestión de clientes, reservas y viajes de colectivos.

## Entidades

- Booking
	- Contiene información sobre las reservas de pasajes.
	- Conoce el cliente que realiza la reserva, el asiento asignado, y el viaje al cual se asigna.

- Client
	- Contiene información básica de los clientes.
	
- City
	- Contiene información básica de las ciudades en las que opera FunTour.

- Bus
	- Contiene información sobre las unidades de colectivos con las que trabaja FunTour.

- Seat
	- Contiene información sobre los asientos en cada colectivo.

- Trip
	- Contiene información sobre cada viaje programado.
	- Conoce el colectivo asignado, y las ciudades de origen y destino.

---

Contiene CRUD para todas las entidades, y vistas que aportan información pertinente sobre reservas y viajes.
Falta: Filtrar viajes por rango de fechas. Filtrar viajes por companía.