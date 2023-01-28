using LocalController.Wrappers;
using LocalDevice;
using LocalDevice.Wrappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace LocalController
{
    public class LocalControllerClass : ILocalControllerClass
    {

        public MyTcpListener MyServer { get; set; }
        public MyNetworkStream MyStream { get; set; }

        public MyNetworkStream MyAMSStream { get; set; }

        public MyXmlWriter XmlWriter { get; set; }

        public MyXmlReader XmlReader { get; set; }

        public MyTcpClient MyClient { get; set; }

        private string absolutePath;

        public LocalControllerClass()
        {
            MyClient = new MyTcpClient();
            XmlWriter = new MyXmlWriter();
            XmlReader = new MyXmlReader();
            MyAMSStream = new MyNetworkStream();
            string path = "data.xml";
            string dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            absolutePath = Path.Combine(dir, path);
        }

        public void ObrisiXml()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(absolutePath);
            XmlElement root = doc.DocumentElement;
            root.RemoveAll();
            doc.Save(absolutePath);


        }

        public void AMSMain()
        {
            while (true)
            {
                Thread.Sleep(5000);
                Start();
                PosaljiPodatke();
                ObrisiXml();
                Console.WriteLine("DATA SENT TO AMS");
            }
        }

        public void PokreniServer()
        {
            MyStream = new MyNetworkStream();
            MyAMSStream = new MyNetworkStream();
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            MyServer = new MyTcpListener(localAddr, 3560);
            MyServer.Start();

        }

        public void DeviceListener()
        {

        }
		
	    public bool PosaljiPodatke()
        {
            List<LocalDeviceClass> devices = XmlReader.UcitajXml(absolutePath);
            BinaryFormatter bf = new BinaryFormatter();
            byte[] objectBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, devices);
                objectBytes = ms.ToArray();
            }
            try
            {
                MyAMSStream.Write(objectBytes, 0, objectBytes.Length);
                MyAMSStream.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }		



    }
}
