using System;
using UnityEngine;

public class DataPlayer : MonoBehaviour
{
    string walletId;
    public string WalletId { get => walletId; }
    string playerName;

    public string PlayerName { get => playerName; }


    void Awake()
    {
        EventsManagerSpaar.SubscribeToEvent(EventTypeSpaar.WalletId, OnWalletId);
    }

    private void OnWalletId(object[] parameterContainer)
    {
        if (parameterContainer != null && parameterContainer.Length > 0)
        {
            walletId = (string)parameterContainer[0];
            EventsManagerSpaar.TriggerEvent(EventTypeSpaar.GetDataBase, walletId);
            Debug.Log("Wallet ID received: " + walletId);
        }
        else
        {
            Debug.LogWarning("No parameters received for WalletId event.");
        }
    }

}
