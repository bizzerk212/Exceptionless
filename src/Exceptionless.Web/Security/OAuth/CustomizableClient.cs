using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OAuth2.Client;
using OAuth2.Configuration;
using OAuth2.Infrastructure;
using OAuth2.Models;

namespace Exceptionless.Web.Security.OAuth {
    public class CustomizableClient : OAuth2Client {
        private Endpoint codeServiceEndpoint;
        private Endpoint tokenServiceEndpoint;
        private Endpoint userInfoServiceEndpoint;
        private string name;

        public CustomizableClient(IRequestFactory factory,
            IClientConfiguration configuration,
            string name,
            Endpoint codeServiceEndpoint, 
            Endpoint tokenServiceEndpoint, 
            Endpoint userInfoServiceEndpoint)
            : base(factory, configuration) {

            this.name = name;
            this.codeServiceEndpoint = codeServiceEndpoint;
            this.tokenServiceEndpoint = tokenServiceEndpoint;
            this.userInfoServiceEndpoint = userInfoServiceEndpoint;
        }

        public override string Name {
            get {
                return name;
            }
        }


        protected override Endpoint AccessCodeServiceEndpoint { 
            get {
                return codeServiceEndpoint;
            } 
        }

        protected override Endpoint AccessTokenServiceEndpoint {
            get {
                return tokenServiceEndpoint;
            }
        }

        protected override Endpoint UserInfoServiceEndpoint {
            get {
                return userInfoServiceEndpoint;
            }
        }

        protected override UserInfo ParseUserInfo(string content) {
            var response = JObject.Parse(content);
            var avatarUri = response["picture"].SafeGet(x => x.Value<string>());
            const string avatarUriTemplate = "{0}?sz={1}";
            return new UserInfo {
                Id = response["id"].Value<string>(),
                Email = response["email"].SafeGet(x => x.Value<string>()),
                FirstName = response["given_name"].Value<string>(),
                LastName = response["family_name"].Value<string>(),
                AvatarUri =
                    {
                        Small = !string.IsNullOrWhiteSpace(avatarUri) ? string.Format(avatarUriTemplate, avatarUri, 36) : string.Empty,
                        Normal = avatarUri,
                        Large = !string.IsNullOrWhiteSpace(avatarUri) ? string.Format(avatarUriTemplate, avatarUri, 300): string.Empty
                    }
            };
        }
    }
}
