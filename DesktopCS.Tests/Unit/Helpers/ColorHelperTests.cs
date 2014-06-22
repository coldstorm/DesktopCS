using System;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DesktopCS.Helpers;

namespace DesktopCS.Tests.Helpers
{
    [TestClass]
    public class ColorHelperTests
    {
        [TestMethod]
        public void InvalidHexToColorTest()
        {
            string hex = "invalid hex";
            Exception expectedException = null;
            Color output = default(Color);

            try
            {
                output = ColorHelper.FromHexWithoutHash(hex);
            }

            catch (Exception ex)
            {
                expectedException = ex;
            }

            Assert.IsNull(expectedException);
            Assert.AreEqual(output, default(Color));
        }

        [TestMethod]
        public void ShortHexToColorTest()
        {
            string hex = "fff";

            Color output = ColorHelper.FromHexWithoutHash(hex);

            Assert.AreEqual(output, Colors.White);
        }

        [TestMethod]
        public void LongHexToColorTest()
        {
            string hex = "ffffff";

            Color output = ColorHelper.FromHexWithoutHash(hex);

            Assert.AreEqual(output, Colors.White);
        }

        [TestMethod]
        public void ValidColorToHexTest()
        {
            string output = ColorHelper.ToHexWithoutHash(Colors.White);

            Assert.AreEqual(output, "FFFFFF");
        }
    }
}
