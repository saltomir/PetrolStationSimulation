using System;
using System.Collections.Generic;


namespace PetrolStationAssessment
{
    class Counters
    {
        //Counters fields
        public static double costPerLitter = 1.2; // free to choose any sensible cost
        public static double totalLitersDispensed = 0;
        public static double totalIncome;
        public static int numOfVehiclesServed = 0;
        public static double commission;
        public static int numOfVehiclesLeaved = 0;
        
        /// <summary>
        /// Calculate counters
        /// </summary>
        /// <param name="litresPerTransaction">Value represents how many litres where dispensed per transaction</param>
        public static void UpdateCounters(double litresPerTransaction)
        {
            totalLitersDispensed += litresPerTransaction;
            totalIncome = totalLitersDispensed * costPerLitter;
            commission = 0.01 * totalLitersDispensed;
            numOfVehiclesServed++;
        }
        
        /// <summary>
        /// Updates variable that tracks how many vehicles leaved the queue
        /// before being served.
        /// </summary>
        public static void CalculateNumOfVehiclesLeaved()
        {
            numOfVehiclesLeaved++;
        }

        
        
    }
}
