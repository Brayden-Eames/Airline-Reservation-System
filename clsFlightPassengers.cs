using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Reflection;
using Assignment6AirlineReservation;

namespace Assignment_6
{

     class clsFlightPassengers
    {
         public List<clsPassenger> lstPassengers{ get; set; }

        clsDataAccess db; //declare instance of class 'clsDataAccess' 
        string sSQL;    //Holds an SQL statement that fetches the passenger's    
        int iRet = 0;   //Number of return values
        DataSet ds = new DataSet(); //Holds the return values
        string passengerID;
        string pFirstName;
        string pLastName;
        string pFlightID;
        


        /// <summary>
        /// This method handles loading the passengers from the database into the combo box. 
        /// </summary>
        public  void getPassengers(int flag)
        {
            try
            {
                db = new clsDataAccess();
                //Create the SQL statement to extract the information from clsFlightManager

                if (flag == 0)
                {
                    sSQL = "SELECT PASSENGER.Passenger_ID, First_Name, Last_Name, Seat_Number " +
                  "FROM FLIGHT_PASSENGER_LINK, FLIGHT, PASSENGER " +
                  "WHERE FLIGHT.FLIGHT_ID = FLIGHT_PASSENGER_LINK.FLIGHT_ID AND " +
                  "FLIGHT_PASSENGER_LINK.PASSENGER_ID = PASSENGER.PASSENGER_ID AND " +
                  "FLIGHT.FLIGHT_ID = 1";
                }
                else if (flag == 1)
                {
                    sSQL = "SELECT PASSENGER.Passenger_ID, First_Name, Last_Name, Seat_Number " +
                  "FROM FLIGHT_PASSENGER_LINK, FLIGHT, PASSENGER " +
                  "WHERE FLIGHT.FLIGHT_ID = FLIGHT_PASSENGER_LINK.FLIGHT_ID AND " +
                  "FLIGHT_PASSENGER_LINK.PASSENGER_ID = PASSENGER.PASSENGER_ID AND " +
                  "FLIGHT.FLIGHT_ID = 2";
                }

                //Extract the information and put it into the DataSet
                ds = db.ExecuteSQLStatement(sSQL, ref iRet);

                lstPassengers = new List<clsPassenger>();
                //Loop through the data and create the flight classes
                for (int i = 0; i < iRet; i++)
                {
                    lstPassengers.Add(new clsPassenger
                    {
                        sID = ds.Tables[0].Rows[i][0].ToString(),
                        sFirstName = ds.Tables[0].Rows[i]["First_Name"].ToString(),
                        sLastName = ds.Tables[0].Rows[i]["Last_Name"].ToString(),
                        sFlight = ds.Tables[0].Rows[i][3].ToString(),
                        sSeat = ds.Tables[0].Rows[i][3].ToString()
                    });
                }
                return;
            }
            catch(Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// This method returns the list of passengers to the window.xaml.cs class. 
        /// </summary>
        /// <returns></returns>
        public List<clsPassenger>passList()
        {
            try
            {
                return lstPassengers;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        
        /// <summary>
        /// This method handles inserting a new passenger up until seat selection is required.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="flag"></param>
        public void insertNewPassenger(string firstName, string lastName, int flag)
        {
            try
            {
                db = new clsDataAccess();
                pFirstName = firstName;
                pLastName = lastName;

                ///the method below converts the flag's meaning into either flight 1 or 2.
                if (flag == 0)
                {
                    pFlightID = Convert.ToString(1);
                }
                else
                {
                    pFlightID = Convert.ToString(2);
                }
                string seatNum = "0"; ///sets the seat number to 0 by default so it can be passed into flight_passenger_link

                /// First, insert the firstname, lastname, and flightID into the passenger table. 
                sSQL = string.Format("INSERT INTO PASSENGER(First_Name, Last_Name) VALUES('{0}','{1}');", firstName, lastName);

                ///Executes the sSQL statement to insert the passenger into the table. 
                db.ExecuteNonQuery(sSQL);

                ///Next, query back out the Passenger_ID from the Passenger table, now that its been created (it's an autonumber). 
                sSQL = string.Format("SELECT Passenger_ID from Passenger where First_Name = '{0}' AND Last_Name = '{1}'", firstName, lastName);

                ///Executes the sSQL statement to retrieve the passenger's ID, and store the result in the passengerID variable. 
                passengerID = db.ExecuteScalarSQL(sSQL);

                sSQL = string.Format("INSERT INTO Flight_Passenger_Link(Flight_ID, Passenger_ID, Seat_Number) VALUES({0},{1},{2})",
                    Convert.ToInt16(pFlightID), Convert.ToInt16(passengerID), seatNum);

                db.ExecuteNonQuery(sSQL);
                return;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        /// <summary>
        /// This method handles the new passenger's seat selection. This is where the SQL to update the passenger seat choice happens.
        /// </summary>
        public void updatePassSeat(string seatNum, int flag)
        {
            try
            {
                string uPassengerID;
                string uFlightID;
                ///the method below converts the flag's meaning into either flight 1 or 2.
                if (flag == 0)
                {
                    uFlightID = Convert.ToString(1);
                }
                else
                {
                    uFlightID = Convert.ToString(2);
                }

                sSQL = "SELECT PASSENGER_ID FROM Flight_Passenger_Link WHERE Seat_Number = '0'";
                uPassengerID = db.ExecuteScalarSQL(sSQL); ///retrieves the passenger ID where a seat number is 0, since there's only ever one instance of this.

                sSQL = string.Format("UPDATE FLIGHT_PASSENGER_LINK " +
               "SET Seat_Number = {0} " +
               "WHERE FLIGHT_ID = {1} AND PASSENGER_ID = {2}", seatNum, uFlightID, uPassengerID);
                db.ExecuteNonQuery(sSQL);
                return;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// method to change passenger seat who already has an existing seat. 
        /// </summary>
        public void changePassSeat(string cPassengerID, string cFlightID, string cSeatChoice)
        {
            try
            {
                sSQL = string.Format("UPDATE FLIGHT_PASSENGER_LINK " +
              "SET Seat_Number = '{0}' " +
              "WHERE FLIGHT_ID = {1} AND PASSENGER_ID = {2}", cSeatChoice, Convert.ToInt32(cFlightID), Convert.ToInt32(cPassengerID));

                db.ExecuteNonQuery(sSQL);
                return;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// this method handles deletion of passengers from the database.
        /// </summary>
        /// <param name="dPassengerID"></param>
        /// <param name="dFlightID"></param>
        public void deletePassenger(string dPassengerID, string dFlightID)
        {
            try
            {
                //Deleting the link (Must be done first!)
                sSQL = string.Format("Delete FROM FLIGHT_PASSENGER_LINK " +
                           "WHERE FLIGHT_ID = {0} AND " +
                           "PASSENGER_ID = {1}", Convert.ToInt32(dFlightID), Convert.ToInt32(dPassengerID));

                db.ExecuteNonQuery(sSQL);

                //Delete the passenger(Must be done second!)
                sSQL = string.Format("Delete FROM PASSENGER " +
                    "WHERE PASSENGER_ID = {0}", Convert.ToInt32(dPassengerID));

                db.ExecuteNonQuery(sSQL);
                return;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}
