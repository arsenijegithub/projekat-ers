// LocalController - ponasa se i kao server i kao klijent

using System.Net.Sockets;
using System;
using System.Xml;
using System.Text;
using System.Timers;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace LocalController
{
    class Program
    {
        public static class Globals
        {
            public const string IP = "127.0.0.1";
            public const int PORT = 8001;
        }

        static void SendXmlFile(Object state)
        {
            // neka po default-u bude podeseno na slanje kontroleru
            TcpClient client = new TcpClient("127.0.0.1", 8002);

            // client stream za citanje i pisanje
            NetworkStream stream = client.GetStream();

            Console.WriteLine("Uspostavljanje konekcije sa AMS-om...");

            // poruka za server
            string message = "Podaci o novom lokalnom uredjaju su poslati.";

            string path = @"..\..\..\..\data.xml";
            string dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string absolutePath = Path.Combine(dir, path);

            byte[] data = System.Text.Encoding.ASCII.GetBytes(File.ReadAllText(absolutePath)); // ovde prosledjujem ono sta ce da posalje serveru

            stream.Write(data, 0, data.Length);

            Console.WriteLine("[LK->AMS] Poslato: {0}", data);

            data = new byte[4096];
            int bytes = stream.Read(data, 0, data.Length);
            string response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("[LK->AMS] Primljeno: {0}", response);

            stream.Close();
            client.Close();
        }

        public static List<string> IsolateValues(string fullMessage)
        {
            List<string> vrednosti = new List<string>();

            string all, type, val, config;
            int id, code, tempTime, worktime;

            all = Convert.ToString(fullMessage);

            string[] lines = all.Split('\n');

            id = Convert.ToInt32(lines[1].Trim().Remove(0, 4));
            vrednosti.Add(Convert.ToString(id));

            type = lines[2].Trim().Remove(0, 6);
            vrednosti.Add(type);

            code = Convert.ToInt32(lines[3].Trim().Remove(0, 17));
            vrednosti.Add(Convert.ToString(code));

            tempTime = Convert.ToInt32(lines[4].Trim().Remove(0, 11));
            vrednosti.Add(Convert.ToString(tempTime));

            val = lines[5].Trim().Remove(0, 7);
            vrednosti.Add(val);

            worktime = Convert.ToInt32(lines[6].Trim().Remove(0, 10));
            vrednosti.Add(Convert.ToString(worktime));

            config = lines[7].Trim().Remove(0, 15);
            vrednosti.Add(config);

            return vrednosti;

        }

        public static void WriteToXML(string fullMessage)
        {

            List<string> vr = new List<string>();

            string path = @"..\..\..\..\data.xml";
            string dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string absolutePath = Path.Combine(dir, path);
            //Console.WriteLine("APSOLUTNA PUTANJA TEST 1: " + absolutePath);

            vr = IsolateValues(fullMessage);

            List<string> lista = new List<string>();

            lista = IsolateValues(fullMessage);

            XmlDocument doc = new XmlDocument();

            doc.Load(absolutePath);

            XmlNode item = doc.CreateElement("item");

            XmlNode deviceID = doc.CreateElement("deviceID");
            deviceID.InnerText = vr[0];
            item.AppendChild(deviceID);

            XmlNode deviceType = doc.CreateElement("deviceType");
            deviceType.InnerText = vr[1];
            item.AppendChild(deviceType);

            XmlNode deviceCode = doc.CreateElement("deviceCode");
            deviceCode.InnerText = vr[2];
            item.AppendChild(deviceCode);

            XmlNode time = doc.CreateElement("time");
            time.InnerText = vr[3];
            item.AppendChild(time);

            XmlNode value = doc.CreateElement("value");
            value.InnerText = vr[4];
            item.AppendChild(value);

            XmlNode workTime = doc.CreateElement("workTime");
            workTime.InnerText = vr[5];
            item.AppendChild(workTime);

            XmlNode configuration = doc.CreateElement("configuration");
            configuration.InnerText = vr[6];
            item.AppendChild(configuration);

            doc.DocumentElement.AppendChild(item);

            doc.Save(absolutePath);

            Console.WriteLine("Uspesno dodavanje podataka u XML datoteku.");
        }

        static void ReaderFunction(object client)
        {

            TcpClient tcpClient = (TcpClient)client;

            NetworkStream stream = tcpClient.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = 0;

            // ako hoćemo da primi samo jednu poruku, izbacićemo while i to je to, ne moramo da brinemo o tajmeru i kada će stići stop. stop je tu samo zbog testiranja.
            do
            {
                try
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                }
                catch (IOException)
                {
                    Console.WriteLine(tcpClient.Client.RemoteEndPoint + " forcibly closed by the remote host");
                }
            }
            while (bytesRead < 0);

            string dataReceived = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine(tcpClient.Client.RemoteEndPoint + " šalje[" + bytesRead + "] bajta: " + dataReceived);

            WriteToXML(dataReceived);

            stream.Close();

            tcpClient.Client.Close();

        }

        static void ConnectionHandler(object arg)
        {
            while (true)
            {
                TcpListener listener = (TcpListener)arg;
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Konekcija od " + client.Client.RemoteEndPoint + " prihvaćena u " + DateTime.Now);

                Thread readerThread = new Thread(ReaderFunction);
                readerThread.Start(client);
            }
        }

        static void WorkerFunction(Object state)
        {
            // Wait for a client to connect

            Console.WriteLine("Primač konekcija: " + DateTime.Now);

            while (true)
            {
                TcpListener listener = (TcpListener)state;
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Konekcija od " + client.Client.RemoteEndPoint + " prihvaćena u " + DateTime.Now + "\n");

                Thread readerThread = new Thread(ReaderFunction);
                readerThread.Start(client);
            }
        }

        static void Main(string[] args)
        {
            int tempId = 0;
            string tempType = "";
            int tempCode = 0;
            int tempTime = 0;
            string tempValue = "";
            double tempWorkTime = 0;
            string tempConfig = "";

            string tempRequest = "";

            IPAddress localAddr = IPAddress.Parse(Globals.IP);
            TcpListener server = new TcpListener(localAddr, Globals.PORT);
            server.Start();

            Console.WriteLine("Lokalni kontroler je pokrenut u " + DateTime.Now + " " + Globals.IP + ":" + Globals.PORT);

            Thread connectionHandlerThread = new Thread(ConnectionHandler);
            connectionHandlerThread.Start(server);

            Thread workerThread = new Thread(WorkerFunction);
            workerThread.Start(server);

            // na svakih 300000ms = 5 minuta salje celu xml datoteku ams-u
            System.Threading.Timer timer = new System.Threading.Timer(new TimerCallback(SendXmlFile), null, 300000, 300000);  // prvih 5000 je posle pokretanja za koliko ce biti pozvana funkcija/tajmer, drugi na koliko sledeći put

            workerThread.Join();
        }
    }
}
