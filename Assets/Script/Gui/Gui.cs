using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gui : MonoBehaviour
{

    public List<GameObject> paneles;
    public string panelActual, panelAnterior, firstPanel;
    




    void Awake()
    {
        EventsManagerSpaar.SubscribeToEvent(EventTypeSpaar.CambiarPanel, CambiarPanel);

    }

    void Start()
    {
        Paneles(firstPanel);

        
    }

    void Update()
    {

    }


    void CambiarPanel(object[] parameters)
    {
        var p1 = (string)parameters[0];
        Paneles(p1);
    }


    public void Paneles(string nombre)
    {
        if (panelActual == nombre) return;

        panelAnterior = panelActual;

        panelActual = nombre;


        foreach (var item in paneles)
        {
            if (item.name != nombre) item.SetActive(false);
            else
            {
                item.SetActive(true);
            }
        }


        //EventsManager.TriggerEvent(EventType.Panel, new object[] { nombre });
    }


    public void CambiarAPanelAnterior()
    {
        Paneles(panelAnterior);
    }

}























