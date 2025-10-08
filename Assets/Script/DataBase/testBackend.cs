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
        try
        {
            string token = await AuthService.AuthenticateAndGetTokenAsync("facu", "1234");
            tokenText.text = token;
        }
        catch (System.Exception ex)
        {
            tokenText.text = "Error: " + ex.Message;
        }
    }
}






