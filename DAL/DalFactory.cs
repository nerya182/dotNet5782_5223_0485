using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;

namespace DalApi
{
    public static class DalFactory
    {
        public static IDal GetDal(string str)
        {
            switch(str)
            {
                case "List":
                    return DalObject.DalObject.Instance;
                default:
                    return null;
            }
        }
    }
}
