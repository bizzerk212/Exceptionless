using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exceptionless.Web.Extensions {
    public static class UriExtentions {
        public static OAuth2.Client.Endpoint ToEndpoint(this Uri uri) {
            return new OAuth2.Client.Endpoint {
                BaseUri = uri.GetLeftPart(UriPartial.Authority),
                Resource = uri.PathAndQuery
            };
        }
    }
}
