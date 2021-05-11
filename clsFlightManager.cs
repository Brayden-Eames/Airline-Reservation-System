using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Reflection;
using Assignment6AirlineReservation;
using System.Windows.Controls;

namespace Assignment_6
{

    class clsFlightManager
    {
        public List<clsFlight> lstFlight { get; set; } // this creates a list of type clsFlight, getting values from that clsFlight class.
        //Dictionary<string, Label> SeatList;
        clsDataAccess db; //declare instance of class 'clsDataAccess' 
        string sSQL;    //Holds an SQL statement that fetches the passenger's    
        int iRet = 0;   //Number of return values
        DataSet ds = new DataSet(); //Holds the return values

        /// <summary>
        /// This method handles loading both flights from the database into the combo box. 
        /// </summary>
        public void getFlights()
        {
            try
            {
                db = new clsDataAccess();
                sSQL = "SELECT DISTINCT Flight.Flight_Id, Flight_number, Aircraft_Type " +
                    "FROM Flight, Flight_Passenger_Link " +
                    "WHERE Flight.Flight_ID = Flight_Passenger_Link.Flight_ID";

                //Extract the information and put it into the DataSet
                ds = db.ExecuteSQLStatement(sSQL, ref iRet);

                lstFlight = new List<clsFlight>();
                //Loop through the data and create the flight classes
                for (int i = 0; i < iRet; i++)
                {
                    lstFlight.Add(new clsFlight
                    {
                        sFlight_ID = ds.Tables[0].Rows[i]["Flight_ID"].ToString(),
                        sFlight_Number = ds.Tables[0].Rows[i]["Flight_Number"].ToString(),
                        sAircraft_Type = ds.Tables[0].Rows[i]["Aircraft_Type"].ToString()
                    });
                }
            }
            catch(Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        /// <summary>
        /// This method is what the window.xaml.cs calls to get the data from this class. 
        /// </summary>
        /// <returns></returns>
       public List<clsFlight> flightList() 
        {
            try
            {
                return lstFlight;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        


    }
}
