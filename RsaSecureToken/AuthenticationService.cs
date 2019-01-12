using System;
using System.Collections.Generic;
using RsaSecureToken.Interface;

namespace RsaSecureToken
{
    public class AuthenticationService
    {
        private IProfile _profile;
        private IRsaToken _token;
        private ILog _log;

        public AuthenticationService()
        {
            _profile = new Profile();
            _token = new RsaToken();
        }
        
        public AuthenticationService(IProfile profile, IRsaToken token, ILog log = null)
        {
            _profile = profile;
            _token = token;
            _log = log ?? new ConsoleLog();
        }

        public bool IsValid(string account, string password)
        {
            // 根據 account 取得自訂密碼
            var passwordFromDao = _profile.GetPassword(account);

            // 根據 account 取得 RSA token 目前的亂數
            var randomCode = _token.GetRandom(account);

            // 驗證傳入的 password 是否等於自訂密碼 + RSA token亂數
            var validPassword = passwordFromDao + randomCode;
            var isValid = password == validPassword;

            if (isValid)
            {
                return true;
            }
            else
            {
                // todo, 如何驗證當有非法登入的情況發生時，有正確地記錄log？
                var content = $"account:{account} try to login failed";
                this._log.Save(content);
                return false;
            }
        }
    }

    public class ConsoleLog : ILog
    {
        public void Save(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class Profile : IProfile
    {
        public string GetPassword(string account)
        {
            return Context.GetPassword(account);
        }
    }

    public static class Context
    {
        public static Dictionary<string, string> profiles;

        static Context()
        {
            profiles = new Dictionary<string, string>();
            profiles.Add("joey", "91");
            profiles.Add("mei", "99");
        }

        public static string GetPassword(string key)
        {
            return profiles[key];
        }
    }

    public class RsaToken : IRsaToken
    {
        public string GetRandom(string account)
        {
            var seed = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            var result = seed.Next(0, 999999).ToString("000000");
            Console.WriteLine("randomCode:{0}", result);

            return result;
        }
    }
}