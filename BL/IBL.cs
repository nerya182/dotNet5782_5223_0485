using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    interface  IBL
    {
        void AddStation(Station newStation);
        public IDAL.DO.Station GetClosestCustomer(IDAL.DO.Customer customerSender);
        public IDAL.DO.Station GetClosestStation(DroneToList drone);

    }
}
