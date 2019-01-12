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
        private AuthenticationService _authenticationService;

        public AuthenticationServiceTests()
        {
            _authenticationService = new AuthenticationService(_profile, _rsaToken);
        }

        [Test]
        public void IsValidTest()
        {
            var authenticate = new AuthenticationService(new FakeProfile(), new FakeToken());

            var actual = authenticate.IsValid("joey", "91000000");

            Assert.IsTrue(actual);                       
        }

        [Test]
        public void InValid_Should_Failed()
        {
            var authenticate = new AuthenticationService();

            var actual = authenticate.IsValid("joey", "91000000");

            Assert.IsFalse(actual);                       
        }

        [Test]
        public void IsValidTest_Sub()
        {
            GivenProfile("joey", "91");
            GivenToken("000000");

            ShouldBeValid("joey", "91000000");
        }

        [Test]
        public void InValidTest_Sub()
        {
            GivenProfile("joey", "91");
            GivenToken("123456");

            ShouldBeInValid("joey", "91000000");
        }

        private void ShouldBeValid(string account, string passCode)
        {
            var actual = _authenticationService.IsValid(account, passCode);

            Assert.IsTrue(actual);
        }

        private void ShouldBeInValid(string account, string passCode)
        {
            var actual = _authenticationService.IsValid(account, passCode);

            Assert.IsFalse(actual);
        }

        private void GivenToken(string token)
        {
            _rsaToken.GetRandom("").ReturnsForAnyArgs(token);
        }

        private void GivenProfile(string account, string password)
        {
            _profile.GetPassword(account).Returns(password);
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
