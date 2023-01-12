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
        public static void SendXmlFile()
        {
            // neka po default-u bude podeseno na slanje kontroleru
            TcpClient client = new TcpClient("127.0.0.1", 8086);

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

        public static void ReciveFromDevice(int id, string type, int code, int tempTime, string val, double worktime, string config, string all)
        {
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                TcpListener server = new TcpListener(localAddr, 8001);
                server.Start();

                Console.WriteLine("Lokalni kontroler ceka novu konekciju...");

                TcpClient client = server.AcceptTcpClient();
                NetworkStream stream = client.GetStream();

                // citanje upita od klijenta
                byte[] data = new byte[4096];
                int bytes = stream.Read(data, 0, data.Length);

                string request = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                // sve sto sam dobio od klijenta
                Console.WriteLine("[LU->LK] Primljeno: {0}", request);

                WriteToXML(request); ;

                // slanje odgovora nazad klijentu
                string response = "Lokalni kontroler je uspesno primio podatke o lokalnom uredjaju...";
                data = System.Text.Encoding.ASCII.GetBytes(response);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("[LK->LU] Poslato: {0}", response);

                stream.Close();
                client.Close();

                server.Stop();

            }
            catch (Exception)
            {
                Console.WriteLine("Greska kod kontrolera - servera");
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

            ReciveFromDevice(tempId, tempType, tempCode, tempTime, tempValue, tempWorkTime, tempConfig, tempRequest);

            SendXmlFile();

            int timeLeft = 10;                       // max broj iteracija
            while(timeLeft > 0)
            {
                SendXmlFile();
                Thread.Sleep(5000); // pauzira ga na 5 sekcundi  = 5000 - test, 5 minuta = 300000 ms
            }
            Console.WriteLine("Max broj slanja je poslat\n");

            /*
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 10 * 1000;                                //300 * 1000 = 300000 milisekundi = 5 minuta
            timer.Elapsed += (sender, e) => SendXmlFile();
            timer.Start();
            */
        }
    }
}
