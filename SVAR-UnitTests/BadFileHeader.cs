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
    class BadFileHeader
    {
        [Test]
        public void BadHeaderFileBadIndex()
        {
            DataHeader header;
            InternalDataHeader intHeader;

            bool couldRead = Lib.ReadEmbeddedHeader("SVAR_UnitTests.TestXmls.BadHeaderFileBadIndex.xml", out header);
            Assert.IsFalse(couldRead);
        }

        [Test]
        public void BadHeaderFileBadUnit()
        {
            DataHeader header;
            InternalDataHeader intHeader;
            bool couldRead = Lib.ReadEmbeddedHeader("SVAR_UnitTests.TestXmls.BadHeaderFileBadUnit.xml", out header);
            Assert.IsFalse(couldRead);
        }
    }
}
