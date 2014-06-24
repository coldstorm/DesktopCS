using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DesktopCS.Converters;
using DesktopCS.Helpers;
using NetIRC;

namespace DesktopCS.Tests.Unit.Converters
{
    [TestClass]
    public class RankConverterTests
    {
        private RankConverter rankConverter;

        [TestInitialize]
        public void InitRankConverter()
        {
            this.rankConverter = new RankConverter();
        }

        [TestMethod]
        public void NullRankConversion()
        {
            NullReferenceException expectedException = null;

            try
            {
                var output = this.rankConverter.Convert(null, typeof(char), null, null);
            }

            catch (NullReferenceException ex)
            {
                expectedException = ex;
            }

            Assert.IsNotNull(expectedException);
        }

        [TestMethod]
        public void SingleRankConversion()
        {
            UserRank rank = UserRank.Voice;

            var output = this.rankConverter.Convert(rank, typeof(char), null, null);

            Assert.AreEqual(NetIRCHelper.RankChars[UserRank.Voice], output);
        }

        [TestMethod]
        public void MultipleRankConversion()
        {
            UserRank rank = UserRank.Voice | UserRank.HalfOp;

            var output = this.rankConverter.Convert(rank, typeof(char), null, null);

            Assert.AreEqual(NetIRCHelper.RankChars[UserRank.HalfOp], output);
        }
    }
}
