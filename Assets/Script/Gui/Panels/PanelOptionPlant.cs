using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PanelOptionPlant : MonoBehaviour
{
    public bool planting, watering, fertilizing, harvesting, protectionSpray;

    [SerializeField] Plant plant;

    [SerializeField] Button buttonPlanting, buttonWatering, buttonFertilizing, buttonHarvesting, buttonProtectionSpray, buttonBack;
    void Awake()
    {
        EventsManagerSpaar.SubscribeToEvent(EventTypeSpaar.UnsubscribeToEvent, UnsubscribeEvents);
        EventsManagerSpaar.SubscribeToEvent(EventTypeSpaar.PlantDetected, OnPlantDetected);
        EventsManagerSpaar.SubscribeToEvent(EventTypeSpaar.UpdatePanelOptionsPlant, CheckStatePlant);
    }

    private void CheckStatePlant(object[] parameterContainer)
    {
        CheckStatePlant();
    }


    private void OnPlantDetected(object[] parameterContainer)
    {
        var p = (Plant)parameterContainer[0];
        plant = p;
        CheckStatePlant();
    }


    private void UnsubscribeEvents(object[] parameterContainer)
    {
        EventsManagerSpaar.UnsubscribeToEvent(EventTypeSpaar.UnsubscribeToEvent, UnsubscribeEvents);
        EventsManagerSpaar.UnsubscribeToEvent(EventTypeSpaar.PlantDetected, OnPlantDetected);
        EventsManagerSpaar.UnsubscribeToEvent(EventTypeSpaar.UpdatePanelOptionsPlant, CheckStatePlant);
    }


    public void CheckStatePlant()
    {
        switch (plant.currentState)
        {
            case PlantState.Empty:
                {
                    //   DesactivateAllButtons();
                    buttonPlanting.gameObject.SetActive(true);
                    break;
                }
            case PlantState.Seed:
                {
                    //  DesactivateAllButtons();
                    if (!plant.isWatered)
                        buttonWatering.gameObject.SetActive(true);

                    if (!plant.isFertilized)
                        buttonFertilizing.gameObject.SetActive(true);
                    break;
                }
            case PlantState.Small:
            case PlantState.Medium:
            case PlantState.Large:
                {
                    //   DesactivateAllButtons();
                    buttonWatering.gameObject.SetActive(true);
                    buttonFertilizing.gameObject.SetActive(true);
                    buttonProtectionSpray.gameObject.SetActive(true);
                    break;
                }
            case PlantState.Harvestable:
                {
                    //  DesactivateAllButtons();
                    buttonHarvesting.gameObject.SetActive(true);
                    break;
                }
        }




    }
    void Update()
    {

    }

    public void DesactivateAllButtons()
    {
        buttonPlanting.gameObject.SetActive(false);
        buttonWatering.gameObject.SetActive(false);
        buttonFertilizing.gameObject.SetActive(false);
        buttonHarvesting.gameObject.SetActive(false);
        buttonProtectionSpray.gameObject.SetActive(false);
    }

    public void Planting()
    {
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.CambiarPanel, "Panel Planting");
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.ChangeStatePlayer, "Planting"); // volver al estado Idle del jugador
    }
    public void Watering()
    {
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.CambiarPanel, "State_Water");
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.ChangeStatePlayer, "Water"); // volver al estado Idle del jugador
    }
    public void Fertilizing()
    {
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.CambiarPanel, "Panel Fertilizing");
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.ChangeStatePlayer, "Fertilize"); // volver al estado Idle del jugador

    }
    public void Harvesting()
    {
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.CambiarPanel, "Panel Harvest");
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.ChangeStatePlayer, "Harvest"); // volver al estado Idle del jugador

    }
    public void ProtectionSpray()
    {
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.CambiarPanel, "Panel Protection Spray");
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.ChangeStatePlayer, "ProtectionSpray"); // volver al estado Idle del jugador

    }
    public void Back()
    {
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.CambiarPanel, "Panel Game"); // volver al panel del juego    
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.ChangeStatePlayer, "Idle"); // volver al estado Idle del jugador

    }
}
