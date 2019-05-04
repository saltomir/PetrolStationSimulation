using System;
using System.Timers;
using System.Text;
using System.IO;

namespace PetrolStationAssessment
{
    class Pump
    {
        //Fields
        public Vehicle currentVehicle = null;
       
        public static int nextPumpID = 0;

        public int pumpID;

        //Pump constructor
        public Pump()
        {
            pumpID = nextPumpID++;
        }

        /// <summary>
        /// Returns TRUE if currentVehicle is NULL, meaning available, or FALSE if currentVehicle is NOT NULL, meaning busy
        /// </summary>
        /// <returns>Boolean value depending on value of currentVehicle value</returns>
        public bool IsAvailable()
        {
            return currentVehicle == null;
        }

       
        /// <summary>
        /// Timer once calls ReleaseVehicle method on interval of fuelling time of the vehicle
        /// being processed.
        /// </summary>
        /// <param name="v">vehicle object being processed</param>
        /// <param name="pumpID">pump ID being processed</param>
        public void AssignVehicle(Vehicle v, int pumpID)
        {
            //Assign vehicle to pump, so pump is not available
            currentVehicle = v;

            //Calculate number of litres to be dipensed
            double litresDispensed = currentVehicle.MaxCapacity - currentVehicle.FuelVolume;

            //New timer object
            Timer timer = new Timer();
            timer.Interval = v.FuelTime;
            timer.AutoReset = false; // don't repeat
            timer.Elapsed += (sender, e) => ReleaseVehicle(sender, e, pumpID, litresDispensed);
            timer.Enabled = true;
            timer.Start();
        }

        /// <summary>
        /// Record transaction, update counters and write transcation information to separatate csv file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="pumpID">Pump ID</param>
        /// <param name="litresDispensed">Number if litres dispensed</param>
        public void ReleaseVehicle(object sender, ElapsedEventArgs e, int pumpID, double litresDispensed)
        {
            //Record transaction
            Transaction t = new Transaction(currentVehicle, pumpID, DateTime.Now);
            Transaction.transactions.Add(t);

            //Update counters
            Counters.UpdateCounters(litresDispensed);

            #region // Print the transaction to csv file
            StringBuilder csvcontent = new StringBuilder();//new StringBuilder object
            string datePatt = @"M/d/yyyy hh:mm:ss tt";//Date and time format for ToString() method overl
            csvcontent.AppendLine("Vehicle ID,Vehicle type,Litres dispensed,Fuel type,Fuelling Time (s)," +
                                                                   "Pump number,Date and Time");//First line of each csv file
            csvcontent.AppendFormat("{0},{1},{2},{3},{4},{5},{6}", t.VehicleID, t.VehicleType, 
                                                                   t.NumOfLitres, t.FuelType, 
                                                                   t.FuellingTime, t.PumpNumber, 
                                                                   t.CurrDateTime.ToString(datePatt));//Second line of ech csv file generated           
            //csv file will be created with name "transaction-" and current transaction ID in the path provided by user:
            string filepath = String.Format("{0}transaction-{1}.csv", Program.folderPathTransactions, 
                                                                         t.transactionID.ToString());
            //Write contents of the StringBulider into specified file
            File.AppendAllText(filepath, csvcontent.ToString());
            #endregion

            //Set current vehicle of pump to null, so pump is available now
            currentVehicle = null;
        }

    }
}
