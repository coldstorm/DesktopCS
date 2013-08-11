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
            ArgumentException expectedExcetpion = null;

            // act
            try
            {
                IdentParser.Parse(testIdent);
            }
            catch (ArgumentException ex)
            {
                expectedExcetpion = ex;
            }
            

            // assert
            Assert.IsNotNull(expectedExcetpion);
        }

        [TestMethod]
        public void EmptyIdentTest()
        {
            // arrange
            const string testIdent = "";
            ArgumentException expectedExcetpion = null;

            // act
            try
            {
                IdentParser.Parse(testIdent);
            }
            catch (ArgumentException ex)
            {
                expectedExcetpion = ex;
            }

            // assert
            Assert.IsNotNull(expectedExcetpion);
        }

        [TestMethod]
        public void NullIdentTest()
        {
            // arrange
            const string testIdent = null;
            ArgumentException expectedExcetpion = null;

            // act
            try
            {
                IdentParser.Parse(testIdent);
            }
            catch (ArgumentException ex)
            {
                expectedExcetpion = ex;
            }

            // assert
            Assert.IsNotNull(expectedExcetpion);
        }

        [TestMethod]
        public void NoHexIdentTest()
        {
            // arrange
            const string testIdent = "QQ";
            ArgumentException expectedExcetpion = null;

            // act
            try
            {
                IdentParser.Parse(testIdent);
            }
            catch (ArgumentException ex)
            {
                expectedExcetpion = ex;
            }

            // assert
            Assert.IsNotNull(expectedExcetpion);
        }

        [TestMethod]
        public void InvalidHexIdentTest()
        {
            // arrange
            const string testIdent = "GGGQQ";
            ArgumentException expectedExcetpion = null;

            // act
            try
            {
                IdentParser.Parse(testIdent);
            }
            catch (ArgumentException ex)
            {
                expectedExcetpion = ex;
            }

            // assert
            Assert.IsNotNull(expectedExcetpion);
        }


        [TestMethod]
        public void TooShortHexIdentTest()
        {
            // arrange
            const string testIdent = "12345QQ";
            ArgumentException expectedExcetpion = null;

            // act
            try
            {
                IdentParser.Parse(testIdent);
            }
            catch (ArgumentException ex)
            {
                expectedExcetpion = ex;
            }

            // assert
            Assert.IsNotNull(expectedExcetpion);
        }

        [TestMethod]
        public void ShortHexIdentTest()
        {
            // arrange
            const string testIdent = "FFFQQ";

            // act
            var userMetadata = IdentParser.Parse(testIdent);

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
            var userMetadata = IdentParser.Parse(testIdent);

            // assert
            Assert.AreEqual(userMetadata.CountryCode, "QQ");
            Assert.AreEqual(userMetadata.Color.Color, Brushes.White.Color);
        }

    }
}
