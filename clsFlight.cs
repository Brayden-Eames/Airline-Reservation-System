using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_6
{
    class clsFlight
    {
        //Class variables
        public string sFlight_ID { get; set; }
        public string sFlight_Number { get; set; }
        public string sAircraft_Type { get; set; }


        /// <summary>
        /// Override the ToString method so that this string is displayed in the combo box.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            try
            {
                return sFlight_Number + "-" + sAircraft_Type;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
            

        }
    }
}
