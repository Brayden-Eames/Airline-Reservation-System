using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_6
{
    class clsPassenger
    {
        //class variables
        public string sID { get; set; }
        public string sFirstName { get; set; }
        public string sLastName { get; set; }
        public string sSeat { get; set; }
        public string sFlight { get; set; }


        /// <summary>
        /// This method handles overriding the ToString method to display the data in the comboboxes properly. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            try
            {
                return sFirstName + " " + sLastName;
            }
            catch(Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);

            }

        }
    }
}
