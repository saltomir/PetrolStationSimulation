using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetrolStationAssessment
{ 
    abstract class Vehicle
    {
       //Array of fuel types
        public static string[] fuelTypes = new string[3] {"Diesel", "LPG", "Unleaded" };

        //Properties of vehicle
        public string VehicleType { get; set; }

        public string FuelType { get; set; }

        public double FuelTime { get; set; }

        public double FuelVolume { get; set; }

        public int MaxCapacity { get; set; }
        
        public bool IsWaiting
        {
            get { return isWaiting; }
            set
            {
                isWaiting = value;
            }
        }
        
        //Fields
        public static int nextVehicleID = 0;

        public int vehicleID;

        //Vehicle is waiting by default
        private static bool isWaiting = true;

        //Randomizer
        public static Random rnd = new Random();

       

        /// <summary>
        /// Randomizes fuel type from fuelTypes array.
        /// </summary>
        /// <param name="fuelTypeRange">Range of fuel types that are available for a particular type of vehicle</param>
        /// <returns>String from fuelTypes array/returns>
        protected string GetFuelType(int fuelTypeRange)
        {
            int num = rnd.Next(fuelTypeRange);
            string fuelType = fuelTypes[num];
            return fuelType;
        }

        /// <summary>
        /// Randomizes fuel volume, which is not more than a quarter of vehicle maximum tank capacity
        /// </summary>
        /// <param name="maxCapacity">Particular vehicle maximum tank capacity</param>
        /// <returns>Randomized and rounded double value, representing fuel volume</returns>
        protected double GetFuelVolume(int maxCapacity)
        {
            double fuelVolume = rnd.Next(1, (maxCapacity / 4));
            return fuelVolume;
        }

        /// <summary>
        /// Calculates fuel time depending on fuel volume needed to be dicpensed and 
        /// fuelling speed of 1.5 litres per second
        /// </summary>
        /// <param name="fuelVolume">Vehicle fuel volume, already in its tank</param>
        /// <param name="maxCapacity">Vehicle maximum tank capacity</param>
        /// <returns>Rounded fuel time in miliseconds</returns>
        protected double GetFuelTime(double fuelVolume, int maxCapacity)
        {
            //Calculate fuelling time
            double fuelTime = ((maxCapacity - fuelVolume) / 1.5) * 1000;
            //Round the value to 3 fractional didgits after point
            double roundedFuelTime = Math.Round(fuelTime, 3);
            return roundedFuelTime;
        }

        /// <summary>
        /// Increments nextvehicleID field to return vehicleID
        /// </summary>
        /// <returns>Integer representing vehicle ID</returns>
        protected int GetID()
        {
            vehicleID = nextVehicleID++;
            return vehicleID;
        }
    }
}
