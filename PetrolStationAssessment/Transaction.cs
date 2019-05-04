using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetrolStationAssessment
{
    class Transaction
    {
        //Properties
        public Vehicle Vehicle { get; }
        public int VehicleID { get; }
        public string VehicleType { get; }
        public double NumOfLitres { get; }
        public string FuelType { get; }
        public double FuellingTime { get; }
        public int PumpNumber { get; }
        public DateTime CurrDateTime { get; }

        //Fields
        public static int nextTransactionID = 0;

        public int transactionID;

        //list of transactions
        public static List<Transaction> transactions = new List<Transaction>();

        //Transcation constructor
        public Transaction(Vehicle v, int pumpNumber, DateTime dtm)
        {
            transactionID = nextTransactionID++;
            this.Vehicle = v;
            this.VehicleID = v.vehicleID;
            this.VehicleType = v.VehicleType;
            this.NumOfLitres = (v.MaxCapacity-v.FuelVolume);
            this.FuelType = v.FuelType;
            this.FuellingTime = Math.Round((v.FuelTime / 1000), 3);//Convert fuel time in miliseconds to seconds and round the value to 3 fractional didgits after point
            this.PumpNumber = pumpNumber;
            this.CurrDateTime = dtm;

        }
    }
}
