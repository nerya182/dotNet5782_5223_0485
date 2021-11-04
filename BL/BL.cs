using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.BO;
using IBL.BO;
using IDAL;




namespace IBL
{
    public partial class BL : IBL
    {
        public BL()
        {
            IDal dAL = new DalObject.DalObject();
            double[] elecUsage = dAL.GetElectricUsage();
            double chargeSpeed = dAL.GetChargeSpeed();

  
            IEnumerable<IDAL.DO.Drone> drones = (IEnumerable<IDAL.DO.Drone>)dAL.ListDrone();
            IEnumerable<Drone> droneLogi = (IEnumerable<Drone>)(IBL)drones;
            foreach (IDAL.DO.Parcel objParcel in dAL.ListParcel())
            {
                if (objParcel.Delivered == default(DateTime))
                {
                    foreach (IDAL.DO.Drone objDrone in drones)
                    {
                        if (objDrone.Id == objParcel.DroneId)
                        {
                            
                        }
                    }
                }
            }

        }
        BL bL;

        public void AddStation(Station newStation)
        {
             
            
        }
    }
}
