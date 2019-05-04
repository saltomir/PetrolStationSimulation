using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetrolStationAssessment
{
    class Car : Vehicle
    {
        //As specified, a van can run on all three fuel types, so range of fuel types is 3
        private static int rangeOfFuelTypes = 3;

        //Car constructor
        public Car()
        {
            this.VehicleType = "Car";
            this.MaxCapacity = 40;//as specified
            this.FuelType = GetFuelType(rangeOfFuelTypes);
            this.FuelVolume = GetFuelVolume(this.MaxCapacity);
            this.FuelTime = GetFuelTime(this.FuelVolume, this.MaxCapacity);
            this.vehicleID = GetID();

        }
        
    }
}
