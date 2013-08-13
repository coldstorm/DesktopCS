using System;
using System.Windows.Media;
using DesktopCS.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesktopCS.Tests
{
    [TestClass]
    public class IdentParserTests
    {
        [TestMethod]
        public void InvalidIdentTest()
        {
            // arrange
            const string testIdent = "RANDOM_IDENT";
            ArgumentException expectedException = null;

            // act
            try
            {
                IdentHelper.Parse(testIdent);
            }
            catch (ArgumentException ex)
            {
                expectedException = ex;
            }
            

            // assert
            Assert.IsNotNull(expectedException);
        }

        [TestMethod]
        public void EmptyIdentTest()
        {
            // arrange
            const string testIdent = "";
            ArgumentException expectedException = null;

            // act
            try
            {
                IdentHelper.Parse(testIdent);
            }
            catch (ArgumentException ex)
            {
                expectedException = ex;
            }

            // assert
            Assert.IsNotNull(expectedException);
        }

        [TestMethod]
        public void NullIdentTest()
        {
            // arrange
            const string testIdent = null;
            ArgumentException expectedException = null;

            // act
            try
            {
                IdentHelper.Parse(testIdent);
            }
            catch (ArgumentException ex)
            {
                expectedException = ex;
            }

            // assert
            Assert.IsNotNull(expectedException);
        }

        [TestMethod]
        public void NoHexIdentTest()
        {
            // arrange
            const string testIdent = "QQ";
            ArgumentException expectedException = null;

            // act
            try
            {
                IdentHelper.Parse(testIdent);
            }
            catch (ArgumentException ex)
            {
                expectedException = ex;
            }

            // assert
            Assert.IsNotNull(expectedException);
        }

        [TestMethod]
        public void InvalidHexIdentTest()
        {
            // arrange
            const string testIdent = "GGGQQ";
            ArgumentException expectedException = null;

            // act
            try
            {
                IdentHelper.Parse(testIdent);
            }
            catch (ArgumentException ex)
            {
                expectedException = ex;
            }

            // assert
            Assert.IsNotNull(expectedException);
        }


        [TestMethod]
        public void TooShortHexIdentTest()
        {
            // arrange
            const string testIdent = "12345QQ";
            ArgumentException expectedException = null;

            // act
            try
            {
                IdentHelper.Parse(testIdent);
            }
            catch (ArgumentException ex)
            {
                expectedException = ex;
            }

            // assert
            Assert.IsNotNull(expectedException);
        }

        [TestMethod]
        public void ShortHexIdentTest()
        {
            // arrange
            const string testIdent = "FFFQQ";

            // act
            var userMetadata = IdentHelper.Parse(testIdent);

            // assert
            Assert.AreEqual(userMetadata.CountryCode, "QQ");
            Assert.AreEqual(userMetadata.Color.Color, Brushes.White.Color);
        }

        [TestMethod]
        public void LongHexIdentTest()
        {
            // arrange
            const string testIdent = "FFFFFFQQ";

            // act
            var userMetadata = IdentHelper.Parse(testIdent);

            // assert
            Assert.AreEqual(userMetadata.CountryCode, "QQ");
            Assert.AreEqual(userMetadata.Color.Color, Brushes.White.Color);
        }

    }
}
