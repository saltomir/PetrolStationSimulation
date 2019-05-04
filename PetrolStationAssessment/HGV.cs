using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetrolStationAssessment
{
    class HGV : Vehicle
    {
        //As specified, HGV can run on Diesel, so range of fuel types is 1
        private static int rangeOfFuelTypes = 1;

        //HGV constructor
        public HGV()
        {
            this.MaxCapacity = 150;
            this.VehicleType = "HGV";//as specified
            this.FuelType = GetFuelType(rangeOfFuelTypes);
            this.FuelVolume = GetFuelVolume(this.MaxCapacity);
            this.FuelTime = GetFuelTime(this.FuelVolume, this.MaxCapacity);
            this.vehicleID = GetID();

        }
    }
}
