using System;
using System.IO;
using Cryptography;

namespace Authentication.File
{
    public class AuthenticationFile : IAuthentication
    {
        private readonly string m_HashAlgorithm = "SHA512";

        public bool IsAuthenticated(object authInfo)
        {
            var authData = authInfo as AuthData;

            if (authData == null)
            {
                throw new ArgumentException("Use AuthData object as argument");
            }

            try
            {
                using (StreamReader reader = new StreamReader(authData.AuthFilePath))
                {
                    string hash = reader.ReadToEnd();

                    return SimpleHash.VerifyHash(authData.Password, m_HashAlgorithm, hash);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Open file error.", e);
            }
        }
    }
}
