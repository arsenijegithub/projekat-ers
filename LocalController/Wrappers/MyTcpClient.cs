using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LocalController.Wrappers
{
    public class MyTcpClient
    {
        private readonly string _input;
        private readonly ErrorLogger _errorLogger;

        public SenderTask(string input, ErrorLogger errorLogger)
        {
            _input = input;
            _errorLogger = errorLogger;
        }

        public bool Do()
        {
            try
            {
                Console.WriteLine("- Sending {0}", _input);

                return true;
            }
            catch (Exception e)
            {
                _errorLogger.Log(String.Format("- An error sending {0}: {1}", _input, e.Message));

                return false;
            }
        }
    }
}
