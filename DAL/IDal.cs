using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using DalObject;

namespace IDAL
{
    public interface IDal 
    {
        public  Station GetStation(int id);
        public Drone GetDrone(int droneId);
        public  Customer GetCustomer(int customerId);
        public  Parcel GetParcel(int parcelId);
        public  DateTime GetParcelCreating(int parcelId);



    }
}
