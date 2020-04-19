using System;
using Amazon.Runtime;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using System.Threading.Tasks;
using Amazon;

namespace aws_cognito
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            string username = "username";
            string password = "password";
            string clientId = "clientId";
            string poolId = "poolId";
            Console.WriteLine(await AuthenticateWithSrpAsync(username, password, poolId, clientId));
            Console.ReadLine();
        }

        public static async Task<string> AuthenticateWithSrpAsync(string username, string password, string poolId, string clientId)
        {
            AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(), RegionEndpoint.APSoutheast1);

            CognitoUserPool userPool = new CognitoUserPool(poolId, clientId, provider);
            CognitoUser user = new CognitoUser(username, clientId, userPool, provider);

            AuthFlowResponse context = await user.StartWithSrpAuthAsync(new InitiateSrpAuthRequest
            {
                Password = password
            }).ConfigureAwait(false);

            string token = context?.AuthenticationResult?.AccessToken;
            return token;
        }
    }
}