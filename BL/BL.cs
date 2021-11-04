using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IBL.BO;
using IDAL;
using DalObject;


namespace IBL
{
    public partial class BL : IBL
    {
        public List<Drone> lstDrone = new List<Drone>();
        public BL()
        {
            Random R = new Random();
            IDal dAL = new DalObject.DalObject();
            double[] elecUsage = dAL.GetElectricUsage();
            double chargeSpeed = dAL.GetChargeSpeed();
            IEnumerable<IDAL.DO.Parcel> parcels = dAL.ListParcel();
            IEnumerable<IDAL.DO.Drone> drones = dAL.ListDrone();
            Drone temp = new Drone();
            foreach(var objDrone in drones)
            {
                temp.Id = objDrone.Id;
                temp.Model = objDrone.Model;
                temp.MaxWeight = (WeightCategories)objDrone.MaxWeight;
                lstDrone.Add(temp);
            }
            foreach (IDAL.DO.Parcel objParcel in parcels)
            { 
                if(objParcel.Delivered == DateTime.MinValue)
                {
                    foreach (var drn in lstDrone)
                    {
                        if(drn.Id == objParcel.DroneId)
                        {
                            drn.Status = DroneStatuses.Delivery;
                            drn.Battery = R.Next(0, 100);  // צריך לשנות את ה0

                        }
                    }
                }
            }
           
        }
              
        
    }
}
