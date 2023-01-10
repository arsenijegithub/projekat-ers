// LocalController - ponasa se i kao server i kao klijent

using System.Net.Sockets;
using System;
using System.Xml;
using System.Text;
using System.Timers;

namespace LocalController
{
    class Program
    {
        // u main-u je implementirano primanje podataka od lokalnog uredjaja
        // a ovde slanje xml-a ams-u
        public static void SendXmlFile()
        {
            // neka po default-u bude podeseno na slanje kontroleru
            TcpClient client = new TcpClient("127.0.0.1", 8086);

            // client stream za citanje i pisanje
            NetworkStream stream = client.GetStream();

            Console.WriteLine("Uspostavljanje konekcije sa AMS-om...");

            // poruka za server
            string message = "Podaci o novom lokalnom uredjaju su poslati.";



            string xmlString = System.IO.File.ReadAllText("D:\\fakultet\\5 - semestar\\Elementi razvoja softvera\\projekat-step-by-step\\projekat\\LocalDevice\\data.xml");
            //            string xmlString = System.IO.File.ReadAllText(@"..\..\..\..\" + "data.xml");
            byte[] data = System.Text.Encoding.ASCII.GetBytes(xmlString); // ovde prosledjujem ono sta ce da posalje serveru

            //while (true)
            stream.Write(data, 0, data.Length);

            Console.WriteLine("[LK] Poslato: {0}", data);

            data = new byte[4096];
            int bytes = stream.Read(data, 0, data.Length);
            string response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("[LK] Primljeno: {0}", response);

            stream.Close();
            client.Close();
        }

        public static void ReciveFromDevice(int id, string type, int code, int tempTime, string val, double worktime, string config, string all)
        {
            TcpListener server = new TcpListener(8085);
            server.Start();

            Console.WriteLine("Lokalni kontroler ceka novu konekciju...");

            try
            {
                while (true)
                {
                    // prihvatanje konekcije od klijenta
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();

                    // citanje upita od klijenta
                    byte[] data = new byte[4096];
                    int bytes = stream.Read(data, 0, data.Length);

                    string request = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                    /*
                    if (request == "3")
                    {
                        stream.Close();
                        client.Close();

                        server.Stop();

                        break;
                    }
                    */

                    // sve sto sam dobio od klijenta
                    Console.WriteLine("[LK] Primljeno: {0}", request);

                    // slanje odgovora nazad klijentu
                    string response = "Lokalni kontroler je uspesno primio podatke o lokalnom uredjaju...";
                    data = System.Text.Encoding.ASCII.GetBytes(response);
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("[LK] Poslato: {0}", response);

                    // e sad rasparcamo string tako da uzmemo samo vrednosti o novom lokalnom uredjaju i kasnije to postavimo u xml fajl

                    try
                    {
                        all = Convert.ToString(request);

                        string[] lines = all.Split('\n');

                        id = Convert.ToInt32(lines[1].Trim().Remove(0, 4));
                        type = lines[2].Trim().Remove(0, 6);
                        code = Convert.ToInt32(lines[3].Trim().Remove(0, 17));
                        tempTime = Convert.ToInt32(lines[4].Trim().Remove(0, 11));
                        val = lines[5].Trim().Remove(0, 7);
                        worktime = Convert.ToInt32(lines[6].Trim().Remove(0, 10));
                        config = lines[7].Trim().Remove(0, 15);

                        // dodavanje tih podata u xml fajl
                        XmlDocument doc = new XmlDocument();


                        //                        string path = System.IO.File.ReadAllText(@"..\..\..\..\" + "data.xml");
                        string path = "D:\\fakultet\\5 - semestar\\Elementi razvoja softvera\\projekat-step-by-step\\projekat\\LocalDevice\\data.xml";
                        doc.Load(path);
                        XmlNode item = doc.CreateElement("item");

                        XmlNode deviceId = doc.CreateElement("deviceId");

                        deviceId.InnerText = id.ToString();
                        item.AppendChild(deviceId);
                        doc.DocumentElement.AppendChild(item);

                        XmlNode deviceType = doc.CreateElement("deviceType");

                        deviceType.InnerText = type.ToString();
                        item.AppendChild(deviceType);
                        doc.DocumentElement.AppendChild(item);

                        XmlNode deviceCode = doc.CreateElement("deviceCode");

                        deviceCode.InnerText = code.ToString();
                        item.AppendChild(deviceCode);
                        doc.DocumentElement.AppendChild(item);

                        XmlNode time = doc.CreateElement("time");

                        time.InnerText = tempTime.ToString();
                        item.AppendChild(time);
                        doc.DocumentElement.AppendChild(item);

                        XmlNode value = doc.CreateElement("value");

                        value.InnerText = val.ToString();
                        item.AppendChild(value);
                        doc.DocumentElement.AppendChild(item);

                        XmlNode workTime = doc.CreateElement("workTime");

                        workTime.InnerText = worktime.ToString();
                        item.AppendChild(workTime);
                        doc.DocumentElement.AppendChild(item);

                        XmlNode configuration = doc.CreateElement("configuration");

                        configuration.InnerText = config.ToString();
                        item.AppendChild(configuration);
                        doc.DocumentElement.AppendChild(item);

                        doc.Save(path);

                        Console.WriteLine("Uspesno dodavanje podataka u XML datoteku.");


                        stream.Close();
                        client.Close();

                        server.Stop();

                    }
                    catch
                    {
                        Console.WriteLine("Neuspesno dodavanje podataka u XML datoteku.");
                    }
                }

            }
            catch
            {
                Console.WriteLine("Greska kod kontrolera - servera.");
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

            /*
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 10 * 1000;                                //300 * 1000 = 300000 milisekundi = 5 minuta
            timer.Elapsed += (sender, e) => SendXmlFile();
            timer.Start();
            */
        }
    }
}
