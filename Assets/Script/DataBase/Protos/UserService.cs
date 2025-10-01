using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace User
{
    public static partial class UserService
    {
        private const string DefaultAddress = "rasta-app-2nou22vbya-ew.a.run.app:443";

        private static GrpcChannel _channel;
        private static UserServiceClient _client;

        private static void EnsureClient()
        {
            if (_client == null)
            {
                _channel = GrpcChannel.ForAddress($"https://{DefaultAddress}", new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });
                _client = new UserServiceClient(_channel);
            }
        }

        public static async Task<User> GetUserAsync(string userId, string authToken)
        {
            EnsureClient();
            var headers = new Metadata { { "Authorization", authToken } };
            var request = new GetUserRequest { UserId = userId };
            try
            {
                var reply = await _client.GetUserAsync(request, headers);
                UnityEngine.Debug.Log($"User Data Retrieved: {reply.User.UserId}");
                return reply.User;
            }
            catch (RpcException e)
            {
                UnityEngine.Debug.LogError($"gRPC Error (GetUser): {e.Status.Detail}");
                throw;
            }
        }

        public static async Task<bool> UserExistsAsync(byte[] credentials)
        {
            EnsureClient();
            var request = new UserExistsRequest { Credentials = Google.Protobuf.ByteString.CopyFrom(credentials) };
            try
            {
                var reply = await _client.UserExistsAsync(request);
                UnityEngine.Debug.Log($"User Exists: {reply.Exists}, User ID: {reply.UserId}");
                return reply.Exists;
            }
            catch (RpcException e)
            {
                UnityEngine.Debug.LogError($"gRPC Error (UserExists): {e.Status.Detail}");
                throw;
            }
        }

        public static async Task<User> CreateUserAsync(byte[] credentials)
        {
            EnsureClient();
            var request = new CreateUserRequest { Credentials = Google.Protobuf.ByteString.CopyFrom(credentials) };
            try
            {
                var reply = await _client.CreateUserAsync(request);
                UnityEngine.Debug.Log($"User Created: {reply.User.UserId}");
                return reply.User;
            }
            catch (RpcException e)
            {
                UnityEngine.Debug.LogError($"gRPC Error (CreateUser): {e.Status.Detail}");
                throw;
            }
        }
    }
}
