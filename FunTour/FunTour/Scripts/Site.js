function AddCities() {
            var _TocityId = $('#ToCities').val();
            var _FromcityId = $('#FromCities').val();
            var _packageId = $('#Id_TravelPackage').val();
            
            if (_TocityId == '' && _FromcityId == '') {
                return false;
            }
            
            var _parameters = { ToCityId: _TocityId, FromCityId: _FromcityId, TravelPackageId: _packageId };
            $.ajax({
                url: "/TravelPackages/AddPlacesReturn",
                type: "GET",
                data: _parameters,
                    success:
                    function redirect(){
                        location.href = "http://localhost:60195/TravelPackages/AddServicesToTravel/?idTravelPackage="+ _packageId;
                        }
            });
        };
