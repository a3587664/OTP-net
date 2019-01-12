using NSubstitute;
using NUnit.Framework;
using RsaSecureToken.Interface;
using Assert = NUnit.Framework.Assert;

namespace RsaSecureToken.Tests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private IProfile _profile = Substitute.For<IProfile>();
        private IRsaToken _rsaToken = Substitute.For<IRsaToken>();

        [Test]
        public void IsValidTest()
        {
            var authenticate = new AuthenticationService(new FakeProfile(), new FakeToken());

            var actual = authenticate.IsValid("joey", "91000000");

            Assert.IsTrue(actual);                       
        }

        [Test]
        public void Hard_Code_Token_Should_Failed()
        {
            var authenticate = new AuthenticationService();

            var actual = authenticate.IsValid("joey", "91000000");

            Assert.IsFalse(actual);                       
        }

        [Test]
        public void IsValidTest_Sub()
        {
            var authenticate = new AuthenticationService(_profile, _rsaToken);

            _profile.GetPassword("joey").Returns("91");
            _rsaToken.GetRandom("").ReturnsForAnyArgs("000000");

            var actual = authenticate.IsValid("joey", "91000000");

            Assert.IsTrue(actual);                       
        }
    }

    public class FakeProfile : IProfile
    {
        public string GetPassword(string account)
        {
            if (account == "joey")
            {
                return "91";
            }

            return "";
        }
    }

    public class FakeToken : IRsaToken
    {
        public string GetRandom(string account)
        {
            return "000000";
        }
    }
}
