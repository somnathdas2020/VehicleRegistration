using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRegistration.Models;

namespace VehicleRegistration.Abstract
{
    interface IVehicleRegistration
    {
        VehicleRegistrationList GetAllVehicleRegistration();
        VehicleRegistrationViewModel GetVehicleRegistrationDetail(int regID);
        bool PayRegistrationAmount(int regID, decimal paymentAmount);
    }
}
