using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using LocalDevice;
namespace LocalController.Wrappers
{
    public class MyXmlReader
    {

        public MyXmlReader() 
        {
            
        }

        public virtual List<LocalDeviceClass> UcitajXml(string path) 
        {

            List<LocalDeviceClass> sviUredjaji = new List<LocalDeviceClass>();
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                string Id = node.ChildNodes[0].InnerText;
                string Type = node.ChildNodes[1].InnerText;
                string LocalDeviceCode = node.ChildNodes[2].InnerText;
                long Timestamp = Convert.ToInt64(node.ChildNodes[3].InnerText);
                string Value = node.ChildNodes[4].InnerText;
                double WorkTime = Convert.ToDouble(node.ChildNodes[5].InnerText);
                string Configuration = node.ChildNodes[6].InnerText;
                LocalDeviceClass device = new LocalDeviceClass(Id, Type, Timestamp, Value, WorkTime, Configuration);
                sviUredjaji.Add(device);
            }

            return sviUredjaji;
        }
    }
}
