using UnityEngine;
using System.Threading.Tasks;
using UnityEngine;
using Grpc.Core;
using System.Threading.Tasks;
using Unity.VisualScripting;
using auth;
using Grpc.Net.Client;
using System.Net.Http;
using Grpc.Net.Client.Web;
using System;
using TMPro;
public class testBackend : MonoBehaviour
{
    public TMP_Text tokenText; 
    async void Start()
    {
        var username = "facu";
        var password = "1234";

        // Si tu backend espera "facu:1234"
        var creds = "my-l2-credentials";//$"{username}:{password}";

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
            Debug.Log("Token: " + reply.Token);
            tokenText.text = reply.Token;
        }
        catch (RpcException e)
        {
            Debug.LogError($"gRPC Error: {e.Status.Detail}");
            tokenText.text = "Error: " + e.Status.Detail;
        }


        /*   var reply = await client.AuthenticateAsync(request);
          Debug.Log("Token: " + reply.Token); */



    }
}






