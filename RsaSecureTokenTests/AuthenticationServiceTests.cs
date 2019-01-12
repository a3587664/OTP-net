﻿using NSubstitute;
using NUnit.Framework;
using RsaSecureToken.Interface;
using Assert = NUnit.Framework.Assert;

namespace RsaSecureToken.Tests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        [Test]
        public void IsValidTest()
        {
            var authenticate = new AuthenticationService(new FakeProfile(), new FakeToken());

            var actual = authenticate.IsValid("joey", "91000000");

            Assert.IsTrue(actual);                       
        }

        [Test]
        public void IsValidTest_Sub()
        {
            var profile = Substitute.For<IProfile>();
            var token = Substitute.For<IRsaToken>();

            var authenticate = new AuthenticationService(profile, token);

            profile.GetPassword("joey").Returns("91");
            token.GetRandom("").ReturnsForAnyArgs("000000");

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
