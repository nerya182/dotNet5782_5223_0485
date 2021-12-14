using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlApi
{
    /// <summary>
    /// returns an object BL
    /// </summary>
    public static class BlFactory
    {
        public static IBL GetBl()
        {
            return new BL.BL();
        }
    }
}
