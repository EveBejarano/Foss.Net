# API_de_Vuelos
Este código emula una api de un sistema de aerolineas en la que se puede obtener todo tipo de informacón concerniente a los vuelos.

La base de datos tiene la siguiente composición:

>CommercialFlights : es la tabla "principal" correspondiente a los vuelos comerciales, tiene informacion como ser, Lugar de salida; Lugar de llegada; Fecha de Salida; Fecha de llegada; Avión del vuelo;

>Planes : Contiene información a cerca de los aviones como unidad que puede ser utilizada en un vuelo. Describe modelo, numero identificatorio, capcidad de pasajeros y rango de viaje del avión.

> Destinations: podría decirse que es la sagunda tabla mas importante, ya que conserva información proveniente de cada destino posible de un vuelo. Se identifica por el id del areopuerto en el que se produce el arrivo, a su vez incluye info de la localidad del aeropuerto.

>FlightPlaces :  Es una tabla que por cada entrada representa un asiento de un avión en un vuelo. Hay tantas entidades Places relacionadas a un vuelos como el valor de la capacidad del avión del vuelo. La tabla permite registrar información de quien reserva la plaza. Podría decirse que gracias a esta tabla se manejan las reservas de los vuelos.

>ScalesOnFligth: Contiene información de una escala intermedia (si tuviera) en un vuelo en la cual no se produjera trasbordo.

>Employees: información de los empleados

>FlightPersonal: Vincula los empleados al vuelos indicando el Rol que cumplen en el mismo.

:-------------------------------------------------------------------------------------------------;

En la aplicación actualmente se pueden utulizar los métodos CRUD para cada una de las tablas.
