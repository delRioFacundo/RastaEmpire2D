using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class N8nWebhookSender : MonoBehaviour
{
    public bool activar;
    public string dataToSend = "Mensaje desde Unity";
    public void Update()
    {
        if (activar)
        {
            SendToN8n();
            activar = false;
        }
    }
    // Llamá a esta función cuando quieras enviar datos a n8n
    public void SendToN8n()
    {
        StartCoroutine(PostToN8n(dataToSend));
    }

    IEnumerator PostToN8n(string message)
    {
        string url = "https://n8n-n8n.c9dk6o.easypanel.host/webhook/unity-event";

        // Enviamos la variable como JSON {"value":"lo que sea"}
        string jsonBody = "{\"value\":\"" + message + "\"}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("✅ Respuesta de n8n: " + jsonResponse);

            RespuestaN8n respuesta = JsonUtility.FromJson<RespuestaN8n>(jsonResponse);
            Debug.Log("Mensaje: " + respuesta.message);

        }
        else
        {
            Debug.LogError("❌ Error al enviar: " + request.error);
        }
    }
}

[System.Serializable]
public class RespuestaN8n
{
    public string message;
    public string status;
}
