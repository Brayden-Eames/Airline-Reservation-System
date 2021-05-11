using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Assignment_6;

namespace Assignment6AirlineReservation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        wndAddPassenger wndAddPass;
        clsDataAccess db; //declare instance of class 'clsDataAccess' 
        DataSet ds = new DataSet(); //Holds the return values
        clsFlightManager clsMyFlightManager;
        clsFlightPassengers clsMyFlightPassengers;
        int flag = 0;
        int seatAction = 0; //changed. incase it breaks, set back to nothing. 
        clsPassenger Passenger;
        List<clsPassenger> seatList; // list that grabs the passenger list returned to the main screen. 

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;            
                db = new clsDataAccess();
                clsMyFlightManager = new clsFlightManager();
                clsMyFlightPassengers = new clsFlightPassengers();      
                clsMyFlightManager.getFlights();
                cbChooseFlight.ItemsSource = clsMyFlightManager.flightList();               
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// This method will be called on by the flight selection changed method to highlight seats. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void highlightSeats(int flag)
        {
            try
            {
                string sSeatNumber; ///The seat number  
                seatList = clsMyFlightPassengers.passList(); ///This grabs the passenger list and stores it in the local newList object.

                if (flag == 0)
                {
                    ///flight 1
                    foreach (Label cLabel in this.cA380_Seats.Children.OfType<Label>())
                    {
                        ///loop through each label in cA380_Seats. Find all values that match a seat number from the list.
                        sSeatNumber = Convert.ToString(cLabel.Content);
                        foreach (clsPassenger pList in seatList)
                        {
                            if (sSeatNumber == pList.sSeat) ///the idea here is to check if the canvas label content is equal to the passenger's seat number.
                            {
                                cLabel.Background = Brushes.Red;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    ///flight 2
                    foreach (Label cLabel in this.c767_Seats.Children.OfType<Label>())
                    {
                        ///loop through each label in c767_Seats. Find all values that match a seat number from the list.
                        sSeatNumber = Convert.ToString(cLabel.Content);
                        foreach (clsPassenger pList in seatList)
                        {
                            if (sSeatNumber == pList.sSeat) ///the idea here is to check if the canvas label content is equal to the passenger's seat number.
                            {
                                cLabel.Background = Brushes.Red;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Method that handles when a combobox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbChooseFlight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cbChoosePassenger.IsEnabled = true;
                gPassengerCommands.IsEnabled = true;
                DataSet ds = new DataSet();                
                if (cbChooseFlight.SelectedIndex == 0)
                {
                    flag = 0;
                    ///if the first flight is selected, hide the second flight and display flight 1 data grid, and load passenger 
                    ///combo box with flight 1's passengers names. 
                    clsMyFlightPassengers.getPassengers(flag);                
                    Canvas767.Visibility = Visibility.Hidden;
                    CanvasA380.Visibility = Visibility.Visible;
                    cbChoosePassenger.ItemsSource = clsMyFlightPassengers.passList();                
                    highlightSeats(flag);          
                }
                else
                {
                    flag = 1;
                    clsMyFlightPassengers.getPassengers(flag);
                    CanvasA380.Visibility = Visibility.Hidden;
                    Canvas767.Visibility = Visibility.Visible;
                    cbChoosePassenger.ItemsSource = clsMyFlightPassengers.passList();
                    highlightSeats(flag);
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Method that handles the 'Choose Passenger' Combobox. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbChoosePassenger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {           
                seatList = clsMyFlightPassengers.passList(); ///This grabs the passenger list and stores it in the local newList object.
                highlightSeats(flag);

                if (flag == 0)
                {
                    ///Extract the selected Passenger object from the combo box
                    Passenger = (clsPassenger)cbChoosePassenger.SelectedItem;                  
                   
                    ///this method below finds the passenger that has been selected by the combobox, and highlights green. 
                    foreach (Label dLabel in cA380_Seats.Children.OfType<Label>())
                    {
                        if (Convert.ToString(dLabel.Content) == Passenger?.sSeat)
                        {
                            dLabel.Background = Brushes.Green;
                            lblPassengersSeatNumber.Content = dLabel.Content;
                            break;
                        }
                    }
                }
                else
                {
                    ///Extract the selected Passenger object from the combo box
                    Passenger = (clsPassenger)cbChoosePassenger.SelectedItem;  
                    foreach (Label dLabel in c767_Seats.Children.OfType<Label>())
                    {
                        if (Convert.ToString(dLabel.Content) == Passenger?.sSeat)
                        {
                            dLabel.Background = Brushes.Green;
                            lblPassengersSeatNumber.Content = dLabel.Content;
                            break;
                        }
                    }
                }          
            }
            catch (Exception ex)
            {
                //This actually handles the error by passing this information into the 'HandleError' method. 
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Method that sends data to back class to add a new passenger. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAddPassenger_Click(object sender, RoutedEventArgs e)
        {
            try
            {               
                wndAddPass = new wndAddPassenger(flag); /// passing in the value of flag indicates the flightID.
                wndAddPass.ShowDialog();

                ///need logic to go in and disable all elements of UI except for the available seats. 
                gbPassengerInformation.IsEnabled = false;
                cmdChangeSeat.IsEnabled = false;
                cmdDeletePassenger.IsEnabled = false;
                cmdAddPassenger.IsEnabled = false;              
                ///foreach loop to go through after disabling all ui elements. Loop through and disable any labels that are red.
                ///first, check which set of labels to loop through
                if(flag == 0)
                {
                    foreach (Label dLabel in cA380_Seats.Children.OfType<Label>())
                    {
                        if(dLabel.Background == Brushes.Red)
                        {
                            dLabel.IsEnabled = false;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    foreach (Label dLabel in c767_Seats.Children.OfType<Label>())
                    {
                        if (dLabel.Background == Brushes.Red)
                        {
                            dLabel.IsEnabled = false;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                seatAction = 1; /// flag tripped to 1, means a new passenger should be added. 
                ///at this point, this function needs to exit, leaving UI elements locked until a label is clicked. 
                return;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// This method handles label clicks. This method should only be called if the UI is locked and a new passenger needs to select a seat to sit in. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ///This method is where we should check the user's click to see if what they clicked was indeed an available seat.
                Label MyLabel = (Label)sender;  ///Get the label that the user clicked
                string cPassengerID;
                string cFlightID;
                string sSeatNumber;

                if(seatAction == 0)
                {
                   ///This method is used to allow for highlighting of squares. 
                    if (MyLabel.Background == Brushes.Blue)
                    {
                        return;
                    }
                    else
                    {
                        MyLabel.Background = Brushes.Green;
                        ///Get the seat number
                        sSeatNumber = (string)MyLabel.Content;
                        ///Loop through the items in the combo box
                        for (int i = 0; i < cbChoosePassenger.Items.Count; i++)
                        {
                            ///Extract the passenger from the combo box
                            clsPassenger hPassenger = (clsPassenger)cbChoosePassenger.Items[i];

                            ///If the seat number matches then select the passenger in the combo box
                            if (sSeatNumber == hPassenger.sSeat)
                            {
                                cbChoosePassenger.SelectedIndex = i;
                                seatAction = 0;
                                return;
                            }
                        }
                    }
                }
                
                else if (seatAction == 1) ///this will only be triggered it the intention is to insert a new passenger. 
                {
                    if (MyLabel.Background == Brushes.Red)
                    {
                        ///This checks if the label is red, indicating an already occupied seat. Maybe update a label here? 
                        return;
                    }
                    else
                    {
                        string seatChoice = Convert.ToString(MyLabel.Content); ///Stores seat number stored in the label into the string. 
                        clsMyFlightPassengers.updatePassSeat(seatChoice, flag); /// call the clsFlightPassengers method that inserts the data into pass_link
                        clsMyFlightPassengers.getPassengers(flag);
                        cbChoosePassenger.ItemsSource = clsMyFlightPassengers.passList();
                        
                        gbPassengerInformation.IsEnabled = true;
                        cmdChangeSeat.IsEnabled = true;
                        cmdDeletePassenger.IsEnabled = true;
                        cmdAddPassenger.IsEnabled = true;

                        ///Foreach loop that re-enables all labels again. 
                        if (flag == 0)
                        {
                            foreach (Label dLabel in cA380_Seats.Children.OfType<Label>())
                            {
                                dLabel.IsEnabled = true;
                            }
                        }
                        else
                        {
                            foreach (Label dLabel in c767_Seats.Children.OfType<Label>())
                            {
                                dLabel.IsEnabled = true;
                            }
                        }
                        highlightSeats(flag);
                        MyLabel.Background = Brushes.Green;
                        seatAction = 0;
                        return;
                    }
                }
                else if (seatAction == 2) /// this will only be hit if the intention is to change a seat. 
                {
                    if (MyLabel.Background == Brushes.Red)
                    {
                        ///This checks if the label is red, indicating an already occupied seat. Maybe update a label here? 
                        return;
                    }
                    else
                    {
                        string seatChoice = Convert.ToString(MyLabel.Content); ///Stores seat number stored in the label into the string. 
                        clsPassenger cPassenger = (clsPassenger)cbChoosePassenger.SelectedItem;
                        cPassengerID = cPassenger.sID;

                        if (flag == 0) ///this is necessary to convert the flag meaning 
                        {
                            cFlightID = Convert.ToString(1);
                        }
                        else
                        {
                            cFlightID = Convert.ToString(2);
                        }

                        ///This line below passes the extracted data to the change pass seat class. 
                        clsMyFlightPassengers.changePassSeat(cPassengerID, cFlightID, seatChoice);
                       clsMyFlightPassengers.getPassengers(flag);
                       cbChoosePassenger.ItemsSource = clsMyFlightPassengers.passList();
                        if (flag == 0)
                        {
                            foreach (Label dLabel in cA380_Seats.Children.OfType<Label>())
                            {
                                dLabel.Background = Brushes.Blue;
                            }
                        }
                        else
                        {
                            foreach (Label dLabel in c767_Seats.Children.OfType<Label>())
                            {
                                dLabel.Background = Brushes.Blue;
                            }
                        }
                        highlightSeats(flag);
                        MyLabel.Background = Brushes.Green;
                        seatAction = 0;
                        return;
                    }
                }           
                seatAction = 0;
            }
            catch (Exception ex)
            {
                //This actually handles the error by passing this information into the 'HandleError' method. 
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// this method handles the change seat button click. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdChangeSeat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                seatAction = 2;
            }
            catch (Exception ex)
            {
                //This actually handles the error by passing this information into the 'HandleError' method. 
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        ///  method that handles deleting a passenger from the database. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDeletePassenger_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ///method to grab the currently selected passenger. 
                clsPassenger dPassenger = (clsPassenger)cbChoosePassenger.SelectedItem;

                string dFlightID;
                string dPassengerID;
                if (flag == 0) ///this is necessary to convert the flag meaning 
                {
                    dFlightID = Convert.ToString(1);
                }
                else
                {
                    dFlightID = Convert.ToString(2);
                }

                dPassengerID = dPassenger.sID;
                ///call the manager class to delete the passenger from the db
                clsMyFlightPassengers.deletePassenger(dPassengerID, dFlightID);
                clsMyFlightPassengers.getPassengers(flag);
                cbChoosePassenger.ItemsSource = clsMyFlightPassengers.passList();

                if (flag == 0)
                {
                    foreach (Label dLabel in cA380_Seats.Children.OfType<Label>())
                    {
                        dLabel.Background = Brushes.Blue;
                    }
                }
                else
                {
                    foreach (Label dLabel in c767_Seats.Children.OfType<Label>())
                    {
                        dLabel.Background = Brushes.Blue;
                    }
                }
                highlightSeats(flag);
            }
            catch (Exception ex)
            {
                //This actually handles the error by passing this information into the 'HandleError' method. 
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
        /// <summary>
        /// This method handles errors.
        /// </summary>
        /// <param name="sClass"></param>
        /// <param name="sMethod"></param>
        /// <param name="sMessage"></param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        } 
    }
}
