﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;



namespace IBL
{
    public partial class BL : IBL
    {
        public BL() { IDal dAL = new DalObject.DalObject();}
        BL bL;
        public BL() 
        { 
            IDal dAL = new DalObject.DalObject(); 
            double[] elecUsage = dAL.GetElectricUsage();
            double chargeSpeed = dAL.GetChargeSpeed();
            dAL.
        }

        
        
    }
}
