using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;
using System.Reflection;
using System.Net;
using System.Threading;
using System.Net.Sockets;


namespace DataCollection
{
    [TestFixture]
    class TCPConnectionSinglePoint
    {

        [Test, Sequential]
        public void BasicSend([Random(0.0f, 10.0f, 10)] float value)
        {
            Parser parser;
            DataHeader header = new DataHeader();
            header.Delimeter = ",";
            header.DeltaTime = .0f;
            header.Name = "testHeader";
            header.SourceLocation = "127.0.0.1";
            header.SourceType = DataSourceType.NETWORK;
            header.DataPoints = new DataPointDefinition[1]
            {
                new DataPointDefinition()
                {
                    Index = 0,
                    Name = "test data point",
                    Min = new FloatRange(0,true),
                    Max = new FloatRange(10, true),
                    Units = Lib.RandomEnumValue<Unit>(),
                    X = 0,
                    Y = 0,
                    Z = 0
                }
            };

            Lib.StartServer(header, 3000, out parser);
            var connection = Lib.CreateConnectionSocket(3000);
            connection.Send(ASCIIEncoding.ASCII.GetBytes($"{value}\n"));
            Thread.Sleep(25);
            parser.UpdateBeforeDraw();
            Assert.IsNotNull(parser.currentInfo);
            Assert.IsNotNull(parser.currentInfo.values);
            Assert.AreEqual(1, parser.currentInfo.values.Length);
            Assert.AreEqual(0, parser.currentInfo.values[0].minValue);
            Assert.AreEqual(10, parser.currentInfo.values[0].maxValue);
            Assert.That(value, Is.EqualTo(parser.currentInfo.values[0].value).Within(.0001));
            parser.Dispose();
            connection.Dispose();
        }

        [Test, Sequential]
        public void DiffrentDelimeter()
        {
            Parser parser;
            DataHeader header = new DataHeader();
            header.Delimeter = "&";
            header.DeltaTime = .0f;
            header.Name = "testHeader";
            header.SourceLocation = "127.0.0.1";
            header.SourceType = DataSourceType.NETWORK;
            header.DataPoints = new DataPointDefinition[2]
            {
                new DataPointDefinition()
                {
                    Index = 0,
                    Name = "test data point",
                    Min = new FloatRange(0,true),
                    Max = new FloatRange(10, true),
                    Units = Lib.RandomEnumValue<Unit>(),
                    X = 0,
                    Y = 0,
                    Z = 0
                },
                new DataPointDefinition()
                {
                    Index = 1,
                    Name = "test data point",
                    Min = new FloatRange(0,true),
                    Max = new FloatRange(10, true),
                    Units = Lib.RandomEnumValue<Unit>(),
                    X = 0,
                    Y = 0,
                    Z = 0
                }
            };

            Lib.StartServer(header, 3000, out parser);
            var connection = Lib.CreateConnectionSocket(3000);
            connection.Send(ASCIIEncoding.ASCII.GetBytes($"{5}&{7}\n"));
            Thread.Sleep(25);
            parser.UpdateBeforeDraw();
            Assert.IsNotNull(parser.currentInfo);
            Assert.IsNotNull(parser.currentInfo.values);
            Assert.AreEqual(2, parser.currentInfo.values.Length);
            Assert.AreEqual(0, parser.currentInfo.values[0].minValue);
            Assert.AreEqual(10, parser.currentInfo.values[0].maxValue);
            Assert.That(5, Is.EqualTo(parser.currentInfo.values[0].value).Within(.0001));

            Assert.AreEqual(0, parser.currentInfo.values[1].minValue);
            Assert.AreEqual(10, parser.currentInfo.values[1].maxValue);
            Assert.That(7, Is.EqualTo(parser.currentInfo.values[1].value).Within(.0001));
            parser.Dispose();
            connection.Dispose();
        }

        [Test, Sequential]
        public void ChangeMinAndMax([Random(0.0f, 10.0f, 10)] float value)
        {
            Parser parser;
            DataHeader header = new DataHeader();
            header.Delimeter = ",";
            header.DeltaTime = .0f;
            header.Name = "testHeader";
            header.SourceLocation = "127.0.0.1";
            header.SourceType = DataSourceType.NETWORK;
            header.DataPoints = new DataPointDefinition[1]
            {
                new DataPointDefinition()
                {
                    Index = 0,
                    Name = "test data point",
                    Units = Lib.RandomEnumValue<Unit>(),
                    X = 0,
                    Y = 0,
                    Z = 0
                }
            };

            Lib.StartServer(header, 3000, out parser);
            var connection = Lib.CreateConnectionSocket(3000);
            connection.Send(ASCIIEncoding.ASCII.GetBytes($"{value}\n"));
            Thread.Sleep(25);
            parser.UpdateBeforeDraw();
            Assert.IsNotNull(parser.currentInfo);
            Assert.IsNotNull(parser.currentInfo.values);
            Assert.AreEqual(1, parser.currentInfo.values.Length);
            Assert.That(value, Is.EqualTo(parser.currentInfo.values[0].minValue).Within(.0001));
            Assert.That(value, Is.EqualTo(parser.currentInfo.values[0].maxValue).Within(.0001));
            Assert.That(value, Is.EqualTo(parser.currentInfo.values[0].value).Within(.0001));
            parser.Dispose();
            connection.Dispose();
        }

        [Test, Sequential]
        public void MultiSend()
        {
            Parser parser;
            DataHeader header = new DataHeader();
            header.Delimeter = ",";
            header.DeltaTime = .0f;
            header.Name = "testHeader";
            header.SourceLocation = "127.0.0.1";
            header.SourceType = DataSourceType.NETWORK;
            header.DataPoints = new DataPointDefinition[50];
            float[] values = new float[50];
            Random rand = new Random();
            string valueString = "";
            for (int i = 0; i < 50; i++)
            {
                header.DataPoints[i] = new DataPointDefinition();
                header.DataPoints[i].Index = (uint?)i;
                header.DataPoints[i].Max = new FloatRange((float)rand.NextDouble(), true);
                header.DataPoints[i].Min = new FloatRange((float)rand.NextDouble(), true);
                header.DataPoints[i].Units = Lib.RandomEnumValue<Unit>();
                values[i] = (float)rand.NextDouble();
                valueString += $"{values[i]},";
            }
            

            Lib.StartServer(header, 3000, out parser);
            var connection = Lib.CreateConnectionSocket(3000);
            connection.Send(ASCIIEncoding.ASCII.GetBytes($"{valueString}\n"));
            Thread.Sleep(50);
            parser.UpdateBeforeDraw();
            Assert.IsNotNull(parser.currentInfo);
            Assert.IsNotNull(parser.currentInfo.values);
            Assert.AreEqual(50, parser.currentInfo.values.Length);
            for (int i = 0; i < 50; i++)
            {
                Assert.That(header.DataPoints[i].Min.Value.value, Is.EqualTo(parser.currentInfo.values[i].minValue).Within(.0001));
                Assert.That(header.DataPoints[i].Max.Value.value, Is.EqualTo(parser.currentInfo.values[i].maxValue).Within(.0001));
                Assert.That(values[i], Is.EqualTo(parser.currentInfo.values[i].value).Within(.0001));
                Assert.AreEqual(header.DataPoints[i].Units, parser.currentInfo.values[i].unit);
            }

            parser.Dispose();
            connection.Dispose();
        }

        [Test, Sequential]
        public void MutipleConnections()
        {
            Parser parser;
            DataHeader header = new DataHeader();
            header.Delimeter = ";";
            header.DeltaTime = .0f;
            header.Name = "testHeader";
            header.SourceLocation = "127.0.0.1";
            header.SourceType = DataSourceType.NETWORK;
            header.DataPoints = new DataPointDefinition[2]
            {
                new DataPointDefinition()
                {
                    Index = 0,
                    Name = "test data point",
                    Min = new FloatRange(0,true),
                    Max = new FloatRange(10, true),
                    Units = Lib.RandomEnumValue<Unit>(),
                    X = 0,
                    Y = 0,
                    Z = 0
                },
                new DataPointDefinition()
                {
                    Index = 1,
                    Name = "test data point",
                    Min = new FloatRange(0,true),
                    Max = new FloatRange(10, true),
                    Units = Lib.RandomEnumValue<Unit>(),
                    X = 0,
                    Y = 0,
                    Z = 0
                }
            };

            Lib.StartServer(header, 3000, out parser);
            var connection1 = Lib.CreateConnectionSocket(3000);
            Thread.Sleep(50);
            var connection2 = Lib.CreateConnectionSocket(3000);
            connection1.Send(ASCIIEncoding.ASCII.GetBytes($"{5}\n"));
            connection1.Send(ASCIIEncoding.ASCII.GetBytes($"{5}\n"));
            connection1.Send(ASCIIEncoding.ASCII.GetBytes($"{5}\n"));
            connection1.Send(ASCIIEncoding.ASCII.GetBytes($"{5}\n"));
            connection2.Send(ASCIIEncoding.ASCII.GetBytes($"{7}\n"));
            connection2.Send(ASCIIEncoding.ASCII.GetBytes($"{7}\n"));
            connection2.Send(ASCIIEncoding.ASCII.GetBytes($"{7}\n"));
            connection2.Send(ASCIIEncoding.ASCII.GetBytes($"{7}\n"));

            Thread.Sleep(250);
            parser.UpdateBeforeDraw();
            Assert.IsNotNull(parser.currentInfo);
            Assert.IsNotNull(parser.currentInfo.values);
            Assert.AreEqual(2, parser.currentInfo.values.Length);
            Assert.AreEqual(0, parser.currentInfo.values[0].minValue);
            Assert.AreEqual(10, parser.currentInfo.values[0].maxValue);
            Assert.That(parser.currentInfo.values[0].value, Is.EqualTo(5).Within(.0001));

            Assert.AreEqual(0, parser.currentInfo.values[1].minValue);
            Assert.AreEqual(10, parser.currentInfo.values[1].maxValue);
            Assert.That(parser.currentInfo.values[1].value, Is.EqualTo(7).Within(.0001));
            parser.Dispose();
            connection1.Dispose();
            connection2.Dispose();
        }
    }
}
