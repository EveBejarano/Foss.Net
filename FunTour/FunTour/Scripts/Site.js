function AddCities() {
    var _TocityId = $('#ToCities').val();
    var _FromcityId = $('#FromCities').val();
    var _packageId = $('#Id_TravelPackage').val();


    if ((_TocityId == '' && _FromcityId == '') || (_TocityId == _FromcityId)) {
        return false;
    }
    
    location.href = "http://localhost:60195/TravelPackages/AddPlacesReturn/?TravelPackageId=" +
        _packageId +
        "&ToCityId=" +
        _TocityId +
        "&FromCityId=" +
        _FromcityId;
};


function AddBus() {
    var _toGoBusId = $('#BusesToGo').val();
    var _toBackBusId = $('#BusesToBack').val();
    var _packageId = $('#Id_TravelPackage').val();

    if (_toGoBusId == '' || _toBackBusId == '') {
        return false;
    }
    debugger;
    location.href = "http://localhost:60195/TravelPackages/AddServicesToTravelReturn/?TravelPackageId=" +
        _packageId +
        " &ToGoId=" +
        _toGoBusId +
        " &ToBackId=" +
        _toBackBusId;
};

function AddFlight() {
    var _toGoFlightId = $('#FlightsToGo').val();
    var _toBackFlightId = $('#FlightsToBack').val();
    var _packageId = $('#Id_TravelPackage').val();

    if (_toGoFlightId == '' || _toBackFlightId == '') {
        return false;
    }
    debugger;
    location.href = "http://localhost:60195/TravelPackages/AddServicesToTravelReturn/?TravelPackageId=" +
        _packageId +
        " &ToGoId=" +
        _toGoFlightId +
        " &ToBackId=" +
        _toBackFlightId;
};

function AddServicesInPlace() {
    var _eventId = $('#Events').val();
    var _hotelId = $('#Hotels').val();
    var _packageId = $('#Id_TravelPackage').val();

    if (_hotelId == '') {
        return false;
    }
    if (_eventId == '') {
        return false;
    }
    debugger;
    location.href = "http://localhost:60195/TravelPackages/AddServicesInPlaceReturn/?TravelPackageId=" +
        _packageId +
        " &EventId=" +
        _eventId +
        " &HotelId=" +
        _hotelId;
}

