using NUnit.Framework;
using SerialSniffer;
namespace SerialSnifferTests
{
    [TestFixture]
    public class ByteEnumerableExtensionsTests
    {
        [Test]
        public void ToHex_WorksWell_WithAnEmptyEnumeration()
        {
            byte[] test = new byte[] { };
            string r = test.ToHex(string.Empty);
            Assert.AreEqual(string.Empty, r);
        }

        [Test]
        public void ToHex_WorksWell_WithASingleNonPrintableCharacter()
        {
            byte[] test = new byte[] { 0x10 };
            string r = test.ToHex(string.Empty);
            Assert.AreEqual("10                                              | .", r);
        }

        [Test]
        public void ToHex_WorksWell_WithASinglePrintableCharacter()
        {
            byte[] test = new byte[] { 0x65 };
            string r = test.ToHex(string.Empty);
            Assert.AreEqual("65                                              | e", r);
        }

        [Test]
        public void ToHex_WorksWell_WithNonDefaultBytesPerRow()
        {
            byte[] test = new byte[] { 0x65 };
            string r = test.ToHex(string.Empty, 8);
            Assert.AreEqual("65                      | e", r);
        }

        [Test]
        public void ToHex_WorksWell_ANumberOfBytesEquallingTheBytesPerRowCount()
        {
            byte[] test = new byte[] { 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c };
            string r = test.ToHex(string.Empty, 8);
            Assert.AreEqual("65 66 67 68 69 6a 6b 6c | efghijkl", r);
        }

        [Test]
        public void ToHex_WorksWell_ANumberOfBytesOneMoreTheBytesPerRowCount()
        {
            byte[] test = new byte[] { 0x65, 0x66, 0x67, 0x68, 0x69, 0x6A, 0x6B, 0x6C, 0x6D };
            string r = test.ToHex(string.Empty, 8);
            Assert.AreEqual("65 66 67 68 69 6a 6b 6c | efghijkl\n6d                      | m", r);
        }

        [Test]
        public void ToHex_WorksWell_ANumberOfBytesTwoMoreTheBytesPerRowCount()
        {
            byte[] test = new byte[] { 0x65, 0x66, 0x67, 0x68, 0x69, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E };
            string r = test.ToHex(string.Empty, 8);
            Assert.AreEqual("65 66 67 68 69 6a 6b 6c | efghijkl\n6d 6e                   | mn", r);
        }

        [Test]
        public void ToHex_WorksWell_ANumberOfBytesTwoMoreTheBytesPerRowCountAndPreamble()
        {
            byte[] test = new byte[] { 0x65, 0x66, 0x67, 0x68, 0x69, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E };
            string r = test.ToHex("Preamble ", 8);
            Assert.AreEqual("Preamble 65 66 67 68 69 6a 6b 6c | efghijkl\n         6d 6e                   | mn", r);
        }
    }
}
