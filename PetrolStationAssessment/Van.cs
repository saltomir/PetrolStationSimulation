using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetrolStationAssessment
{
    class Van : Vehicle
    {
        //As specified, van can run on Diesel and LPG only, so range of fuel types is 2
        private static int rangeOfFuelTypes = 2;

        //Van constructor
        public Van()
        {
            this.MaxCapacity = 80;
            this.VehicleType = "Van";//as specified
            this.FuelType = GetFuelType(rangeOfFuelTypes);
            this.FuelVolume = GetFuelVolume(this.MaxCapacity);
            this.FuelTime = GetFuelTime(this.FuelVolume, this.MaxCapacity);
            this.vehicleID = GetID();

        }
    }
}
