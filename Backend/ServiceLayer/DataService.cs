using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class DataService
    {

        public DataService() { }
        ///<summary>This method loads all persisted data.		 
        ///<para>		 
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method.		 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.		 
        ///</para>		 
        /// </summary>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public string LoadData()
        {
            throw new NotImplementedException();
        }

        ///<summary>This method deletes all persisted data.		 
        ///<para>		 
        ///<b>IMPORTANT:</b>		 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.		 
        ///</para>		 
        /// </summary>		 
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public string DeleteData()
        {
            throw new NotImplementedException();
        }
    }
}
