using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlightMVC.Models
{
    public class mvcEmployeeModel
    {
        // cambié todo los que eran solo "employee" por lo del model
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public mvcEmployeeModel()
        {
            this.FlightPersonals = new HashSet<mvcFlightPersonalModel>();
        }

        public string idEmploy { get; set; }
        public string Employ_Name { get; set; }
        public int DNI { get; set; }
        public string Emp_Occupation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<mvcFlightPersonalModel> FlightPersonals { get; set; }
    }
}