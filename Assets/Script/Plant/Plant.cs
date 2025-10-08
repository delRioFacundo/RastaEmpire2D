using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlantState
{
    Empty,       // no tiene nada plantado
    Seed,        // recién plantada
    Small,       // brote
    Medium,      // intermedia
    Large,       // grande sin cogollos
    Harvestable  // lista para cosechar
}


public class Plant : MonoBehaviour
{
    [Header("Estado")]
    public PlantState currentState = PlantState.Empty;

    [Header("Condiciones")]
    public bool isWatered = false;
    public bool isFertilized = false;

    public bool isProtectionSprayed = false;

    [Header("Timers de crecimiento")]
    public float growthTime = 30f; // tiempo para avanzar de estado
    float growthTimer;

    [SerializeField] List<GameObject> stateVisuals; // referencias a los objetos visuales para cada estado

    // 👇 referencia a la corutina actual de crecimiento
    Coroutine growthRoutine;

    void Start()
    {
        growthTimer = growthTime;
    }

    // 🔹 Ya no necesitás la lógica de Update, la comentamos o dejamos vacío
    void Update()
    {
        // Se elimina la lógica de crecimiento frame a frame
    }

    public void DesactivateAllVisuals()
    {
        foreach (var go in stateVisuals)
        {
            go.SetActive(false);
        }
    }

    public void PlantSeed()
    {
        if (currentState == PlantState.Empty)
        {
            DesactivateAllVisuals();
            stateVisuals[1].SetActive(true); // activar visual de semilla
            currentState = PlantState.Seed;
            ResetConditions();
            EventsManagerSpaar.TriggerEvent(EventTypeSpaar.UpdatePanelOptionsPlant);
            Debug.Log("Plantaste una semilla");
        }
    }

    void AdvanceGrowth()
    {
        switch (currentState)
        {
            case PlantState.Seed:
                currentState = PlantState.Small;
                DesactivateAllVisuals();
                stateVisuals[2].SetActive(true);
                break;
            case PlantState.Small:
                currentState = PlantState.Medium;
                DesactivateAllVisuals();
                stateVisuals[3].SetActive(true);
                break;
            case PlantState.Medium:
                currentState = PlantState.Large;
                DesactivateAllVisuals();
                stateVisuals[4].SetActive(true);
                break;
            case PlantState.Large:
                currentState = PlantState.Harvestable;
                DesactivateAllVisuals();
                stateVisuals[5].SetActive(true);
                break;
        }

        ResetConditions();
        Debug.Log("Planta creció a: " + currentState);
    }

    void ResetConditions()
    {
        isWatered = false;
        isFertilized = false;
        growthTimer = growthTime;
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.UpdatePanelOptionsPlant);
    }

    // === API pública para el Player ===
    public void Water()
    {
        if (!isWatered)
        {
            isWatered = true;
            EventsManagerSpaar.TriggerEvent(EventTypeSpaar.UpdatePanelOptionsPlant);
            TryStartGrowth();
        }
    }

    public void Fertilize()
    {
        if (!isFertilized)
        {
            isFertilized = true;
            EventsManagerSpaar.TriggerEvent(EventTypeSpaar.UpdatePanelOptionsPlant);
            TryStartGrowth();
        }
    }

    public void ProtectionSpray()
    {
        if (!isProtectionSprayed)
        {
            isProtectionSprayed = true;
            EventsManagerSpaar.TriggerEvent(EventTypeSpaar.UpdatePanelOptionsPlant);
            // Aquí podrías agregar lógica adicional si es necesario
            Debug.Log("¡Protegiste la planta con spray!");
        }
    }

    public void Harvest()
    {
        if (currentState == PlantState.Harvestable)
        {
            Debug.Log("¡Cosechaste la planta!");
            // Resetear a semilla o a vacío, según tu diseño
            currentState = PlantState.Seed;
            isWatered = false;
            isFertilized = false;
            growthTimer = growthTime;
            EventsManagerSpaar.TriggerEvent(EventTypeSpaar.UpdatePanelOptionsPlant);
        }
    }

    // 🔹 Nueva función: intenta iniciar la corutina de crecimiento
    void TryStartGrowth()
    {
        if (isWatered && isFertilized && growthRoutine == null && currentState != PlantState.Empty && currentState != PlantState.Harvestable)
        {
            growthRoutine = StartCoroutine(GrowthCountdown());
        }
    }

    // 🔹 Corutina de crecimiento (reemplaza el Update)
    IEnumerator GrowthCountdown()
    {
        growthTimer = growthTime;

        while (growthTimer > 0f)
        {
            growthTimer -= Time.deltaTime;
            yield return null; // espera al siguiente frame
        }

        AdvanceGrowth();
        growthRoutine = null;
    }
}
