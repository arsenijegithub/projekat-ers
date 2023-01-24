using LocalDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LocalController.Wrappers
{
    public class MyXmlWriter
    {

        public MyXmlWriter()
        {

        }
        public virtual void PisiUXml(LocalDeviceClass localDevice, string path)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(path);

            XmlNode item = doc.CreateElement("LocalDevice");
            XmlNode deviceID = doc.CreateElement("deviceID");
            deviceID.InnerText = localDevice.Id.ToString();

            item.AppendChild(deviceID);
            XmlNode deviceType = doc.CreateElement("deviceType");
            deviceType.InnerText = localDevice.Type;
            item.AppendChild(deviceType);


            XmlNode deviceCode = doc.CreateElement("deviceCode");
            deviceCode.InnerText = localDevice.LocalDeviceCode;
            item.AppendChild(deviceCode);

            XmlNode time = doc.CreateElement("timeStamp");
            time.InnerText = localDevice.Timestamp.ToString();
            item.AppendChild(time);

            XmlNode valueNode = doc.CreateElement("value");
            valueNode.InnerText = localDevice.Value.ToString();
            item.AppendChild(valueNode);

            XmlNode workTime = doc.CreateElement("workTime");
            workTime.InnerText = localDevice.WorkTime.ToString();
            item.AppendChild(workTime);

            XmlNode configuration = doc.CreateElement("configuration");
            configuration.InnerText = localDevice.Configuration;
            item.AppendChild(configuration);


            doc.DocumentElement.AppendChild(item);

            doc.Save(path);
        }
    }
}
