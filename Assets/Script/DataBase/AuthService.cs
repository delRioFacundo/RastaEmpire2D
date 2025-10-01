using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using System.Net.Http;
using System.Threading.Tasks;
using auth;
using UnityEngine; // Still needed for Debug.Log, but can be removed if not logging to Unity console

public static class AuthService
{
    public static async Task<string> AuthenticateAndGetTokenAsync(string username, string password)
    {
        // In a real application, you would not hardcode credentials like this.
        // This is for demonstration purposes based on the original testBackend.cs.
        var creds = $"my-l2-credentials"; // Or use actual username/password

        var channel = GrpcChannel.ForAddress("https://rasta-app-2nou22vbya-ew.a.run.app:443", new GrpcChannelOptions
        {
            HttpHandler = new GrpcWebHandler(new HttpClientHandler())
        });

        var client = new Auth.AuthClient(channel);

        var request = new auth.AuthRequest
        {
            Credentials = Google.Protobuf.ByteString.CopyFromUtf8(creds)
        };

        try
        {
            var reply = await client.AuthenticateAsync(request);
            Debug.Log("Token: " + reply.Token); // Logs to Unity console if in editor
            return reply.Token;
        }
        catch (RpcException e)
        {
            Debug.LogError($"gRPC Error: {e.Status.Detail}"); // Logs to Unity console if in editor
            throw; // Re-throw the exception for handling upstream
        }
    }
}
