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
        BL bL;
        public BL() { IDal dal = new DalObject.DalObject();}
        
    }
}