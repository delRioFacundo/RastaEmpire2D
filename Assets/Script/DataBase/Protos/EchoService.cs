using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using EchoClient = Echo.Echo.EchoClient;

namespace Echo
{
    public static partial class EchoService
    {
        private const string DefaultAddress = "rasta-app-2nou22vbya-ew.a.run.app:443";

        private static GrpcChannel _channel;
        private static EchoClient _client;

        private static void EnsureClient()
        {
            if (_client == null)
            {
                _channel = GrpcChannel.ForAddress($"https://{DefaultAddress}", new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });
                _client = new EchoClient(_channel);
            }
        }

        public static async Task<string> SayHelloAsync(string message, string authToken)
        {
            EnsureClient();
            var headers = new Metadata { { "Authorization", authToken } };
            var request = new EchoRequest { Message = message };
            try
            {
                var reply = await _client.SayHelloAsync(request, headers);
                UnityEngine.Debug.Log($"Echo Response: {reply.Message}");
                return reply.Message;
            }
            catch (RpcException e)
            {
                UnityEngine.Debug.LogError($"gRPC Error (SayHello): {e.Status.Detail}");
                throw;
            }
        }
    }
}
