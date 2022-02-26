using System;
using System.Collections.Generic;

namespace Core.Access.Utility.ConfigParser
{
    public interface IConfigParser
    {
        string ClientAESKey { get; }

        TimeSpan TransactionTimeoutInSeconds { get; }

        long? JwtExpiryInSeconds { get; }

        IEnumerable<string> JwtAudiences { get; }
    }
}
