using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetrolStationAssessment
{
    class Display
    {
        /// <summary>
        /// Display menu options on the console
        /// </summary>
        public static void DrawMenu()
        {
            Console.WriteLine("*                 WELCOME TO THE PETROL STATION APP!                   *");
            Console.WriteLine("*                               Menu:                                  *");
            Console.WriteLine("*                       1) Run Petrol Station                          *");
            Console.WriteLine("*                       2) Read transactions                           *");
            Console.WriteLine("*                       3) Read counters                               *");
            Console.WriteLine("*                       4) Exit                                        *");
            Console.WriteLine();
        }

        /// <summary>
        /// Display vehicles queue on the console
        /// </summary>
        public static void DrawVehicles()
        {
            //Local variable representing a vehicle
            Vehicle v;

            Console.WriteLine("Vehicles Queue:");
            //Loop through vehicles queue
            for (int i = 0; i < Data.vehicles.Count; i++)
            {
                //Assign vehicle from the queue to the local variable
                v = Data.vehicles.ElementAt(i);
                //Convert fuel time in miliseconds to seconds and round the value to 3 fractional didgits after point
                double roundedFuelTime = Math.Round((v.FuelTime / 1000), 3);
                //Write the vehicle information on line
                Console.WriteLine("#{0} {1} | Fuel Type: {2} | FuelTime: {3} sec | To be dispensed: {4} litres ", v.vehicleID, v.VehicleType, v.FuelType, roundedFuelTime, (v.MaxCapacity-v.FuelVolume));
                
            }
        }

        /// <summary>
        /// Display pump statuses on the console
        /// </summary>
        public static void DrawPumps()
        {
            ///Local variable representing a pump
            Pump p;

            Console.WriteLine("Pumps Status:");
            //Loop through initialized list of pumps
            for (int i = 0; i < 9; i++)
            {
                //Assign pump from list to the local variable
                p = Data.pumps[i];
                //Write pump number on the console
                Console.Write("  #{0} ", i + 1);
                //If status of the pump is available, write the FREE status on the same line as the pump on the console
                if (p.IsAvailable()) { Console.Write("*******FREE"); }
                //If status is not available, wrie the BUSY status of the pump
                else
                {
                    //Changing background colour of BUSY status to red 
                    string busyText = "*******BUSY";
                    ShowRedText(busyText);
                    Console.Write(busyText);
                   
                }
                Console.Write("*******");
                //Stops making background colour of the pump status red
                Console.ResetColor();
                //If numebr of pumps in line reaches 3, go to next line
                if (i % 3 == 2) { Console.WriteLine(); }
            }

        }

        /// <summary>
        /// Makes red background colour of text 
        /// </summary>
        /// <param name="value">text that needs to be highlighted red</param>
        public static void ShowRedText(string value)
        {
            Console.BackgroundColor = ConsoleColor.Red;
           
        }

        /// <summary>
        /// Display counters information
        /// </summary>
        public static void DrawCounters()
        {
            Console.WriteLine("Counters status:");
            Console.WriteLine();
            Console.WriteLine("Total litres dispensed: {0}",Counters.totalLitersDispensed);
            Console.WriteLine("Total income: {0}", Counters.totalIncome);
            Console.WriteLine("Commission: {0}", Counters.commission);
            Console.WriteLine("Number of vehicles served: {0}", Counters.numOfVehiclesServed);
            Console.WriteLine("Number of vehicles leaved: {0}", Counters.numOfVehiclesLeaved);
            
        }

    }
}
