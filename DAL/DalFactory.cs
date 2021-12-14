using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using DO;

namespace DalApi
{
    public static class DalFactory
    {
        /// <summary>
        /// according to the string returns an object of DalObject
        /// </summary>
        /// <param name="str"></param>
        /// <returns>return obj of DalObject</returns>
        public static IDal GetDal(string str)
        {
            switch(str)
            {
                case "List":
                    return DalObject.DalObject.Instance;
                default:
                    throw new ItemNotFoundException(2);


            }
        }
    }
}
