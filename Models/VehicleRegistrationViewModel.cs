using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VehicleRegistration.Models
{
    public class VehicleRegistrationViewModel
    {
        public int VehicleRegistrationID { get; set; }
        public string CustomerName { get; set; }
        public string VehicleName { get; set; }
        public decimal RegistrationAmount { get; set; }
        public int RegistrationYear { get; set; }
        public bool AllAmountPaid { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
    }

    public class VehicleRegistrationList
    {
        public List<VehicleRegistrationViewModel> _RegistrationList { get; set; }

        public VehicleRegistrationList()
        {
            _RegistrationList = new List<VehicleRegistrationViewModel>();
        }
    }
}