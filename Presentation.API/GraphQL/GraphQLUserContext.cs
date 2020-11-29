using System;
using System.Collections.Generic;
using Presentation.API.Auth;

namespace Presentation.API.GraphQL
{
    public class GraphQLUserContext : Dictionary<string, object>
    {
        /// <summary>
        /// Reads the user id or null if not available
        /// </summary>
        /// <returns></returns>
        public string GetUserId()
        {
            object userIdRaw = this.GetValueOrDefault(ClaimTypes.UserId, null);
            if (userIdRaw == null) return null;

            return Convert.ToString(userIdRaw);
        }
    }
}
