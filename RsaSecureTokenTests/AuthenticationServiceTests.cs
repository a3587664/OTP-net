using NSubstitute;
using NUnit.Framework;
using RsaSecureToken.Interface;
using Assert = NUnit.Framework.Assert;

namespace RsaSecureToken.Tests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private IProfile _profile;
        private IRsaToken _rsaToken;
        private ILog _log;
        private AuthenticationService _authenticationService;

        [SetUp]
        public void SetUp()
        {
            _profile = Substitute.For<IProfile>();
            _rsaToken = Substitute.For<IRsaToken>();
            _log = Substitute.For<ILog>();
            _authenticationService = new AuthenticationService(_profile, _rsaToken, _log);
        }

        [Test]
        public void IsValidTest()
        {
            var authenticate = new AuthenticationService(new FakeProfile(), new FakeToken(), new ConsoleLog());

            var actual = authenticate.IsValid("joey", "91000000");

            Assert.IsTrue(actual);
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

        [Test]
        public void When_InValid_Should_Log()
        {
            GivenProfile("joey", "91");
            GivenToken("123456");
            _authenticationService.IsValid("joey", "wrong pwd");
            _log.Received(1).Save(Arg.Is<string>(m => m.Contains("joey") && m.Contains("login failed")));
            //_log.Received(1).Save("account:joey try to login failed"); //過度指定
        }

        private void ShouldBeValid(string account, string passCode)
        {
            var actual = _authenticationService.IsValid(account, passCode);

            Assert.IsTrue(actual);

            _log.Received(0).Save(Arg.Is<string>(m => m.Contains(account) && m.Contains("login failed")));
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
