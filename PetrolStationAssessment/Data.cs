using System;
using System.Collections.Generic;
using System.Timers;
using System.Collections.Concurrent;
using System.Threading.Tasks;


namespace PetrolStationAssessment
{
    class Data
    {
        private static Timer timer;
        public static ConcurrentQueue<Vehicle> vehicles;
        public static List<Pump> pumps = new List<Pump>();
        public static Random rnd = new Random();


        #region Initialization
        /// <summary>
        /// Initialize creation of vehicles queue and pumps list
        /// </summary>
        public static void Initialise()
        {
            InitialisePumps();
            InitialiseVehicles();
        }

        /// <summary>
        /// Initialize pumps list
        /// </summary>
        private static void InitialisePumps()
        {
            Pump p;

            for (int i = 0; i < 9; i++)
            {
                p = new Pump();
                pumps.Add(p);
            }
        }

        /// <summary>
        ///Timer calls CreateVehicle method continuously every random number of miliseconds
        ///between 1500 and 2200 as specified
        /// </summary>
        private static void InitialiseVehicles()
        {

            vehicles = new ConcurrentQueue<Vehicle>();


            // https://msdn.microsoft.com/en-us/library/system.timers.timer(v=vs.71).aspx
            //New Timers.Timer object instantiated
            timer = new Timer();
            timer.Interval = rnd.Next(1500, 2200);
            timer.AutoReset = true;
            timer.Elapsed += CreateVehicle;
            timer.Enabled = true;
            timer.Start();
        }

        /// <summary>
        /// Generate random vehicle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CreateVehicle(object sender, ElapsedEventArgs e)
        {
            // queue limit
            if (vehicles.Count == 5)
            {
                return;
            }
            //Local variable is assigned a random integer beween 1 and 3 inclusively
            int vehicleTypegenerator = rnd.Next(1, 4);
            //Depending on randomized integer create a vehicle type
            switch (vehicleTypegenerator)
            {
                case 1:
                    //Instantiate Car object
                    Car c = new Car();
                    //Add the object to the end of the queue
                    vehicles.Enqueue(c);
                    //Call method that tries to delete a vehicle from the queue after waiting time
                    TryToDeleteVehicleFromQueue(c);
                    break;
                case 2:
                    //Instantiate Van object
                    Van vn = new Van();
                    //Add the object to the end of the queue
                    vehicles.Enqueue(vn);
                    //Call method that tries to delete a vehicle from the queue after waiting time
                    TryToDeleteVehicleFromQueue(vn);
                    break;
                case 3:
                    //Instantiate HGV object
                    HGV hgv = new HGV();
                    //Add the object to the end of the queue
                    vehicles.Enqueue(hgv);
                    //Call method that tries to delete a vehicle from the queue after waiting time
                    TryToDeleteVehicleFromQueue(hgv);
                    break;

            }

        }

        /// <summary>
        /// Timer calls CheckIfAssignedVehicle method after randomly generated waiting time
        /// between 1000 and 2000 seconds as soecified
        /// </summary>
        /// <param name="v">Vehicle that needs to be either deleted or not from the queue</param>
        private static void TryToDeleteVehicleFromQueue(Vehicle v)
        {
            //Local variable assigned randomly generated integer between 1000 and 2000 exclusively
            int waitingTime = rnd.Next(1000, 2000);
            //New timer object instantiated
            timer = new Timer();
            timer.Interval = waitingTime;
            timer.AutoReset = false;
            timer.Elapsed += (sender, e) => CheckIfAssignedVehicle(sender, e, v);
            timer.Enabled = false;
            timer.Start();

        }
        /// <summary>
        /// Checks if status of vehicle is Assigned to pump or not, and deletes from the queue accourdingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="v">Vehicle being checked</param>
        private static void CheckIfAssignedVehicle(object sender, ElapsedEventArgs e, Vehicle v)
        {
            //Ensure that vehicles queue is not null
            if (vehicles.Count == 0) { return; }
           
            //If vehicle is assined to pump, return
            if (v.IsWaiting)
            {
                return;
            }
            else
            {
                //If vehicle was not assigned then delete the vehicle from the beginning of the queue
                vehicles.TryDequeue(out v);
                //Increase number of vehicles leaved by one
                Counters.CalculateNumOfVehiclesLeaved();
            }
        }

        #endregion

        /// <summary>
        /// Assign vehicle to the last available pump
        /// </summary>
        public static void AssignVehicleToPump()
        {
            //Local varibale representing a vehicle
            Vehicle v;
            // //Local varibale representing a pump
            Pump p;

            //Ensure vehicles queue is not null
            if (vehicles.Count == 0) { return; }

            //Loop through three pump lanes from top to bottom
            for (int i = 2; i < 9; i += 3)
            {
                //Assign pump from list to the local variable
                p = pumps[i];
                //Ensure vehicles queue is not null
                if (vehicles.Count == 0) { break; }
                //Check if in the lane all of the pumps are free, if true, assign vehicle to the furthest pump, otherwise go to the next check
                if (pumps[i].IsAvailable() && pumps[i - 1].IsAvailable() && pumps[i - 2].IsAvailable())
                {
                    //Ensure vehicles queue is not null
                    if (vehicles.Count == 0) { break; }
                    vehicles.TryPeek(out v); // get first vehicle
                    v.IsWaiting = false; //Change status of the vehicle to Assigned
                    vehicles.TryDequeue(out v); // remove vehicles from queue
                    pumps[i].AssignVehicle(v, p.pumpID); // assign it to the pump
                }
                //Check if in the same lane two closest pumps are free, if true, assign vehicle to the furthest pump, otherwise go to the next check
                if (pumps[i - 1].IsAvailable() && pumps[i - 2].IsAvailable())
                {
                    //Ensure vehicles queue is not null
                    if (vehicles.Count == 0) { break; }
                    vehicles.TryPeek(out v); // get first vehicle
                    v.IsWaiting = false; ;//Change status of the vehicle to Assigned
                    vehicles.TryDequeue(out v); // remove vehicles from queue
                    pumps[i - 1].AssignVehicle(v, p.pumpID); // assign it to the pump
                }
                //Check if in the same lane the closest pump is free, if true, assign vehicle to the pump, otherwise go to the next lane
                if (pumps[i - 2].IsAvailable())
                {
                    //Ensure vehicles queue is not null
                    if (vehicles.Count == 0) { break; }
                    vehicles.TryPeek(out v); // get first vehicle
                    v.IsWaiting = false; // Change status of the vehicle to Assigned
                    vehicles.TryDequeue(out v); // remove vehicles from queue
                    pumps[i - 2].AssignVehicle(v, p.pumpID); // assign it to the pump
                }
            }
        }
    }
}
