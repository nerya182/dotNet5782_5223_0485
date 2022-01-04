using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DalXml
{
    class XMLTools
    {
        static string dir = @"..\..\..\..\Data\";
        static XMLTools()
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
        public static void SaveListToXMLSerializer<T>(List<T> list, string filepath)
        {
            try
            {
                FileStream file = new FileStream(dir + filepath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch(Exception e)
            {
                //throw new DO.XMLFileLoadCreateException(filepath, $"fail to create xml file: {filepath}", e);
            }
        }

        public static List<T> LoadListFromXMLSerializer<T>(string filepath)
        {
            try
            {
                if (File.Exists(dir + filepath))
                {
                    List<T> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(dir + filepath, FileMode.Open);
                    list = (List<T>)x.Deserialize(file);
                    file.Close();
                    return list;
                }
                else
                    return new List<T>();
            }
            catch(Exception e)
            {
                throw;
                //throw new DO.XMLFileLoadCreateException(filepath, $"fail to load xml file: {filepath}", e);
            }
        }

        public static void SaveListToXMLElement(XElement rootElem, string filePath)
        {
            try
            {
                rootElem.Save(dir + filePath);
            }
            catch (Exception ex)
            {
                //throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        public static XElement LoadListFromXMLElement(string filePath)
        {
            try
            {
                if (File.Exists(dir + filePath))
                {
                    return XElement.Load(dir + filePath);
                }
                else
                {
                    XElement rootElem = new XElement(dir + filePath);
                    rootElem.Save(dir + filePath);
                    return rootElem;
                }
            }
            catch (Exception ex)
            {
                throw;
                //throw new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
    }
}
