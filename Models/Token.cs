using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Token
    {
        public Token(string accessToken, string expiresIn, string tokenType, string scope)
        {
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
            TokenType = tokenType;
            Scope = scope;
        }

        public string AccessToken { get; set; }
        public string ExpiresIn { get; set; }
        public string TokenType { get; set; }
        public string Scope { get; set; }
    }


}
