using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using NUnit;
using System.Net.Sockets;
using System.Net;

namespace DataCollection
{
    public class Vector3
    {
        public float x;
        public float y;
        public float z;
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    class Lib
    {
        public static bool ReadEmbeddedHeader(string resourceName, out DataHeader dataHeader)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                File.WriteAllText("temp.xml", result);
            }

            dataHeader = new DataHeader();
            bool couldRead = DataHeader.TryReadHeader("temp.xml", out dataHeader);
            File.Delete("temp.xml");
            return couldRead;
        }


        public static void StartServer(DataHeader header, int port, out Parser parser)
        {
            NetworkDataSource dataSource = new NetworkDataSource(port, header);
            parser = new Parser(dataSource);
            parser.start();
        }

        public static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(new Random().Next(v.Length));
        }

        public static Socket CreateConnectionSocket(int port)
        {
            Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            soc.Connect(IPAddress.Loopback, port);
            return soc;
        }
    }
}
