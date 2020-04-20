using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;
using System.Reflection;

namespace DataCollection
{
    [TestFixture]
    public class ReadGoodHeader
    {
        bool couldRead = false;
        DataHeader header;
        InternalDataHeader intHeader;
        [OneTimeSetUp]
        public void TestFixtureSetup()
        {
            couldRead = Lib.ReadEmbeddedHeader("SVAR_UnitTests.TestXmls.GoodHeaderFile.xml", out header);
            if(couldRead)
                intHeader = new InternalDataHeader(header);
        }

        [Test]
        public void CouldOpenFile()
        {
            Assert.IsTrue(couldRead);
            TestContext.Write("Correctly Loaded HeaderFile");
        }

        [Test]
        public void ReadDeltaTimeCorrectly()
        {
            Assert.IsTrue(couldRead, "File not parsed");
            TestContext.Write($"Read Delta time as {header.DeltaTime}");
            Assert.AreEqual(header.DeltaTime, .2f);
        }


        [Test]
        public void ReadSourceCorrectly()
        {
            Assert.IsTrue(couldRead, "File not parsed");
            TestContext.Write($"Read source type as {header.SourceType}");
            Assert.AreEqual(header.SourceType, DataSourceType.NETWORK);
        }

        [Test]
        public void ReadDataPoints()
        {
            Assert.IsTrue(couldRead, "File not parsed");
            Assert.AreEqual(intHeader.DataPoints.Length, 4);

            
            Assert.AreEqual("testSensor1", intHeader.DataPoints[0].name);
            Assert.AreEqual(1, intHeader.DataPoints[0].X);
            Assert.AreEqual(2, intHeader.DataPoints[0].Y);
            Assert.AreEqual(3, intHeader.DataPoints[0].Z);
            Assert.AreEqual(0, intHeader.DataPoints[0].index);
            Assert.AreEqual(Unit.Inch, intHeader.DataPoints[0].Units);
            Assert.AreEqual(0, intHeader.DataPoints[0].Min);
            Assert.AreEqual(true, intHeader.DataPoints[0].isMinFixed);
            Assert.AreEqual(10, intHeader.DataPoints[0].Max);
            Assert.AreEqual(false, intHeader.DataPoints[0].isMaxFixed);

            Assert.AreEqual("testSensor2", intHeader.DataPoints[1].name);
            Assert.AreEqual(1, intHeader.DataPoints[1].X);
            Assert.AreEqual(2, intHeader.DataPoints[1].Y);
            Assert.AreEqual(3, intHeader.DataPoints[1].Z);
            Assert.AreEqual(1, intHeader.DataPoints[1].index);
            Assert.AreEqual(Unit.Foot, intHeader.DataPoints[1].Units);
            Assert.AreEqual(float.MaxValue, intHeader.DataPoints[1].Min);
            Assert.AreEqual(false, intHeader.DataPoints[1].isMinFixed);
            Assert.AreEqual(float.MinValue, intHeader.DataPoints[1].Max);
            Assert.AreEqual(false, intHeader.DataPoints[1].isMaxFixed);

            Assert.AreEqual("testSensor3", intHeader.DataPoints[2].name);
            Assert.AreEqual(2, intHeader.DataPoints[2].X);
            Assert.AreEqual(2, intHeader.DataPoints[2].Y);
            Assert.AreEqual(2, intHeader.DataPoints[2].Z);
            Assert.AreEqual(2, intHeader.DataPoints[2].index);
            Assert.AreEqual(Unit.Kip, intHeader.DataPoints[2].Units);
            Assert.AreEqual(-10, intHeader.DataPoints[2].Min);
            Assert.AreEqual(false, intHeader.DataPoints[2].isMinFixed);
            Assert.AreEqual(10, intHeader.DataPoints[2].Max);
            Assert.AreEqual(true, intHeader.DataPoints[2].isMaxFixed);

            Assert.AreEqual("testSensor5", intHeader.DataPoints[3].name);
            Assert.AreEqual(1, intHeader.DataPoints[3].X);
            Assert.AreEqual(2, intHeader.DataPoints[3].Y);
            Assert.AreEqual(3, intHeader.DataPoints[3].Z);
            Assert.AreEqual(10, intHeader.DataPoints[3].index);
            Assert.AreEqual(Unit.Foot, intHeader.DataPoints[3].Units);
            Assert.AreEqual(float.MaxValue, intHeader.DataPoints[3].Min);
            Assert.AreEqual(false, intHeader.DataPoints[3].isMinFixed);
            Assert.AreEqual(float.MinValue, intHeader.DataPoints[3].Max);
            Assert.AreEqual(false, intHeader.DataPoints[3].isMaxFixed);
        }
    }
}
