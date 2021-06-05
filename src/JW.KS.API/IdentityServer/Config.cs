using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace JW.KS.API.IdentityServer
{
    public class Config
    {
        public static IEnumerable<ApiScope> Scopes =>
             new List<ApiScope> { new ApiScope("api.knowledgespace", "Full access to KnowledgeSpace api") };

        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("api.knowledgespace", "KnowledgeSpace API")
            };
    }
}