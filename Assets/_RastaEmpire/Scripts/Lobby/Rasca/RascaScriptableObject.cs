using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "RascaScriptableObject", menuName = "Rasta Empire v2/RascaScriptableObject", order = 0)]
public class RascaScriptableObject : ScriptableObject
{
    public int type;
    public float minPurityCBDPlant;
    public float maxPurityCBDPlant;
    public float porcentajeObtenerDesde;
    public float porcentajeObtenerHasta;

    public string pathImg;
    public int uses;
}
