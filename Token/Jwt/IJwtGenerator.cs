using System.Collections.Generic;

namespace Token.Jwt
{
    public interface IJwtGenerator
    {
        public string Generate(string windowsAccount, IEnumerable<string> validRoles);
    }
}
