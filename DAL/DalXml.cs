using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DalApi;
using DalObject;
using DO;
namespace DalXml
{
    sealed class DalXml : IDal
    {
        static readonly IDal instance = new DalXml();
        public static IDal Instance { get => instance; }

        private DalXml()
        {
            DataSource.Initialize();
        }

        string dronespath = @"DronesXml.xml";

    }
}
