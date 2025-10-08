using ScratchCardAsset;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RascaController : MonoBehaviour
{
    public Sprite[] premiosSpriteIndoors;
    public Sprite[] premiosSpriteSemilla;
    public Image[] premiosImage;
    public Sprite imagenGanadora;
    public Sprite[] imagenesPerdedoras;

    public Image[] probalidades1;
    public Image[] probalidades2;
    public Image[] probalidades3;

    public Sprite[] fondos;
    public Image fondoRasca;

    public EraseProgress[] premiosProgress;
    public EraseProgress[] Probabilidades1Progress;
    public EraseProgress[] Probabilidades2Progress;
    public EraseProgress[] Probabilidades3Progress;

    public RascaScriptableObject[] rascaScriptableObjects;

    public GameObject panelGanador;
    public Image imagenPanelGanador;

    bool ganadorDescubierto;
    bool tipoRasca;
    int valorGanador;
    int columnaGanadora;
    bool[][] columnasRasca = new bool[3][];

    public string idRasca;
    public string tipoRascaString;

    Image[][] probabilidades = new Image[3][];
    EraseProgress[][] progresos = new EraseProgress[4][];

    //FirebaseAPI firebase;

    [SerializeField] private string wallet;




    async void Start()
    {
        panelGanador.SetActive(false);
       /*  await Waiter.Until(() => FirebaseAPI.Instance.ready == true);
        firebase = FirebaseAPI.Instance; */
       // PlayerPrefs.SetString("Wallet", "0x55761cf6f83ad20b8fb871c66bfb27ddc72ec8e9");
        wallet = PlayerPrefs.GetString("Wallet");

        tipoRasca = (tipoRascaString.Equals("1") ? true : false);

        ObtenerGanador();
      //  AddItemToInventoryBaseData(wallet, rascaScriptableObjects[valorGanador], tipoRasca);

        List<Sprite> imagesIndoor = premiosSpriteIndoors.ToList();
        List<Sprite> imagesPlants = premiosSpriteSemilla.ToList();
        //probalidades1[0].sprite = imagenGanadora;
        probabilidades[0] = probalidades1;
        probabilidades[1] = probalidades2;
        probabilidades[2] = probalidades3;


        columnaGanadora = UnityEngine.Random.Range(0, 3);

        for (int i = 0; i < columnasRasca.Length; i++)
        {
            columnasRasca[i] = new bool[3];
            if (columnaGanadora == i)
            {
                for (int j = 0; j < columnasRasca.Length; j++)
                    columnasRasca[i][j] = true;

            }
            else
            {
                columnasRasca[i][0] = randomBool();
                columnasRasca[i][1] = randomBool();

                if (columnasRasca[i][0] && columnasRasca[i][1])
                    columnasRasca[i][2] = false;
                else
                    columnasRasca[i][2] = randomBool();
            }
        }

        if (tipoRasca)
        {
            //premiosImage[columnaGanadora].sprite = premiosSpriteIndoors[valorGanador];
            premiosImage[columnaGanadora].sprite = imagesIndoor[valorGanador];
            imagesIndoor.RemoveAt(valorGanador);
            fondoRasca.sprite = fondos[0];
            imagenPanelGanador.sprite = premiosSpriteIndoors[valorGanador];
        }
        else
        {
            //premiosImage[columnaGanadora].sprite = premiosSpriteSemilla[valorGanador];
            premiosImage[columnaGanadora].sprite = imagesPlants[valorGanador];
            imagesPlants.RemoveAt(valorGanador);
            fondoRasca.sprite = fondos[1];
            imagenPanelGanador.sprite = premiosSpriteSemilla[valorGanador];
        }

        for (int i = 0; i < columnasRasca.Length; i++)
        {
            for (int j = 0; j < columnasRasca[i].Length; j++)
            {
                if (columnasRasca[i][j])
                    probabilidades[i][j].sprite = imagenGanadora;
                else
                    probabilidades[i][j].sprite = imagenesPerdedoras[UnityEngine.Random.Range(0, imagenesPerdedoras.Length)];
            }

            if (premiosImage[i] != premiosImage[columnaGanadora])
            {
                if (tipoRasca)
                {
                    //premiosImage[i].sprite = premiosSpriteIndoors[UnityEngine.Random.Range(0, premiosSpriteIndoors.Length)];
                    int imageRandom = UnityEngine.Random.Range(0, imagesIndoor.Count());
                    premiosImage[i].sprite = imagesIndoor[imageRandom];
                    imagesIndoor.RemoveAt(imageRandom);
                }
                else
                {
                    //premiosImage[i].sprite = premiosSpriteSemilla[UnityEngine.Random.Range(0, premiosSpriteSemilla.Length)];
                    int imageRandom = UnityEngine.Random.Range(0, imagesPlants.Count());
                    premiosImage[i].sprite = imagesPlants[imageRandom];
                    imagesPlants.RemoveAt(imageRandom);
                }
            }
        }


    }


    private void ObtenerGanador()
    {
        int probabilidadGanadora = UnityEngine.Random.Range(1, 101); ;
        for (int i = 0; i < rascaScriptableObjects.Length; i++)
        {
            if (probabilidadGanadora >= rascaScriptableObjects[i].porcentajeObtenerDesde && probabilidadGanadora <= rascaScriptableObjects[i].porcentajeObtenerHasta)
            {
                valorGanador = rascaScriptableObjects[i].type - 1;
            }
        }

        //tipoRasca = randomBool();
    }

    public void Destruir()
    {
        /* EventsManager.TriggerEvent(EventType.Panels, "");
        EventsManager.UnsubscribeToEvent(EventType.OpenRasca, UnsuscribeRasca); */
        Destroy(gameObject);

    }
    // Update is called once per frame
    void Update()
    {
        if (!ganadorDescubierto)
        {

            float progress = 0;
            for (int i = 0; i < Probabilidades1Progress.Length; i++)
            {
                progress += Probabilidades1Progress[i].GetProgress();
                progress += Probabilidades2Progress[i].GetProgress();
                progress += Probabilidades3Progress[i].GetProgress();

            }
            if (progress >= 9.8)
            {
                descubrirGanador();
                ganadorDescubierto = true;
            }
        }
    }

    private void descubrirGanador()
    {

        panelGanador.SetActive(true);

    }

    private bool randomBool()
    {
        if (UnityEngine.Random.value >= 0.5)
        {
            return true;
        }
        return false;
    }/* 
    async void AddItemToInventoryBaseData(string _walletUser, RascaScriptableObject _itemGanado, bool _tipo)
    {
        if (!_tipo)
        {
            //await AddPlanta(_walletUser, _idInventory, (RascaScriptableObject)_itemGanado);
            await AddPlanta(_walletUser, await GetIdPlant(), _itemGanado);
        }
        else
        {
            //await AddIndoor(_walletUser, _idInventory, (RascaScriptableObject)_itemGanado);
            await AddIndoor(_walletUser, await GetIdIndoor(), _itemGanado);
        }


      //  EventsManager.TriggerEvent(EventType.DeleteItemRascaInventory, new object[] { tipoRasca });
    }
    async Task<int> GetIdPlant()
    {
        var path = "plants/";
        ContadorTabla res = await firebase.firestore.QueryAs<ContadorTabla>(path, "0Contador");
        return res.cantidad;
    }
    async Task UpdateContadorPlants(int idPlantMax)
    {
        var path = "plants/";
        await firebase.firestore.UpdateAsync(path, "0Contador", new Dictionary<string, object> { { "cantidad", idPlantMax + 1 } });
    }
    async Task AddPlanta(string walletUser, int idPlant, RascaScriptableObject item)
    {
        var path = "usuarios/" + walletUser + "/inventario/plants/idplants/";
        float CBD = UnityEngine.Random.Range(item.minPurityCBDPlant, item.maxPurityCBDPlant);
        await firebase.firestore.SetAsync(path, idPlant.ToString(), new Dictionary<string, object> { { "idPlant", idPlant }, { "type", valorGanador + 1 } });



        path = "plants/";
        await firebase.firestore.SetAsync(path, idPlant.ToString(), new Dictionary<string, object> {
            {"idPlant", idPlant },
            {"uses", item.uses },
            {"type", valorGanador + 1 },
            {"cbd", CBD} });

        await UpdateContadorPlants(idPlant);

    }
    async Task<int> GetIdIndoor()
    {
        var path = "indoors/";
        ContadorTabla res = await firebase.firestore.QueryAs<ContadorTabla>(path, "0Contador");
        return res.cantidad;
    }


    async Task AddIndoor(string walletUser, int idIndoor, RascaScriptableObject item)
    {
        var path = "usuarios/" + walletUser + "/inventario/indoors/idindoors/";

        await firebase.firestore.SetAsync(path, idIndoor.ToString(), new Dictionary<string, object> { { "idIndoor", idIndoor }, { "type", valorGanador + 1 } });


        path = "indoors/";
        await firebase.firestore.SetAsync(path, idIndoor.ToString(), new Dictionary<string, object> {
            {"idIndoor", idIndoor },
            {"uses", item.uses },
            {"type", valorGanador + 1 } });

        await UpdateContadorIndoor(idIndoor);
    }
    async Task UpdateContadorIndoor(int idIndoorMax)
    {
        var path = "indoors/";
        await firebase.firestore.UpdateAsync(path, "0Contador", new Dictionary<string, object> { { "cantidad", idIndoorMax + 1 } });
    }
 */
    public void UnsuscribeRasca(object[] parameters)
    {
        idRasca = "";
        tipoRasca = false;
        ganadorDescubierto = false;
        columnaGanadora = 0;
    }
}

public class ContadorTabla
{
    public int cantidad;
}
