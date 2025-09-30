using System;
using UnityEngine;

public class GetDataBase : MonoBehaviour
{
    void Awake()
    {
        EventsManagerSpaar.SubscribeToEvent(EventTypeSpaar.GetDataBase, OnGetDataBase);
    }

    private void OnGetDataBase(object[] parameterContainer)
    {
        var idDataBase = (string)parameterContainer[0];
        Debug.Log("Get Data Base for Wallet ID: " + idDataBase);
    }


    void Update()
    {
        
    }
}
