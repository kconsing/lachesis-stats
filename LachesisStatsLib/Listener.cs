using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace LachesisStatsLib
{
    class Listener
    {
        private TcpListener listener = null;
        private IPAddress local_address = IPAddress.Parse("10.1.10.130");
        private Int32 port = 22222;
        private bool is_running = false;



        public Listener(string ip, int port)
        {
            this.local_address = IPAddress.Parse(ip);
            this.port = port;
            this.listener = new TcpListener(this.local_address, this.port);
        }

        public void StartListener()
        {
            listener.Start();
            is_running = true;

            while (is_running)
            {

                TcpClient cli = listener.AcceptTcpClient();

                Thread t = new Thread(() =>
                {
                    StreamReader reader = new StreamReader(cli.GetStream(), Encoding.ASCII);
                    StreamWriter writer = new StreamWriter(cli.GetStream(), Encoding.ASCII);
                    string message = reader.ReadLine();
                });

                t.IsBackground = true;
                t.Start();
            }
        }

        public void StopListener()
        {
            listener.Stop();
        }

        class LachesisResult
        {
            private int statcount { get; set; }
            private string timestamp { get; set; }
            private string computername { get; set; }
            private int controlid { get; set; }

            public LachesisResult()
            {
                statcount = 0;
                timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                computername = "";
                controlid = 0;
            }

            public int SetStatCount(int count)
            {
                return count;
            }

            public string SetTimeStamp(string time)
            {
                return time;
            }

            public string SetComputerName(string cname)
            {
                return cname;
            }

            public int SetControlId(int cid)
            {
                return cid;
            }
        }
    }
}
