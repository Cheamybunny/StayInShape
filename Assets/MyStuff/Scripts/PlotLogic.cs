using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlotLogic : MonoBehaviour
{

    const string DATETIME_FORMAT = "MM/dd/yyyy HH:mm:ss";
    List<PlantData> plants;
    private void Awake()
    {

    }
    private void Start()
    {
        LoadPlants();
        gameObject.SetActive(true);
    }

    public void InsertPlant(GameObject plantPrefab, Vector3 position)
    {
        //get relative position of plant with soil
        Debug.Log("Insert Here");
        GameObject spawnedPlant = Instantiate(plantPrefab);
        spawnedPlant.transform.SetParent(transform);
        spawnedPlant.transform.localPosition = transform.InverseTransformPoint(position);
        spawnedPlant.transform.rotation = transform.rotation;
        spawnedPlant.transform.localScale = new Vector3(0.3f, 2f, 0.3f);
    }

    public void LoadPlants()
    {
        plants = PlantManager.instance.GetPlants();
        foreach (var plant in plants)
        {
            GameObject spawnedPlant;
            if (plant.plantType == 1)
            {
                spawnedPlant = Instantiate(PlantManager.instance.getPlantPrefab());

            }
            else if (plant.plantType == 2)
            {
                spawnedPlant = Instantiate(PlantManager.instance.getLoofaPrefab());
            }
            else if(plant.plantType == 3)
            {
                spawnedPlant = Instantiate(PlantManager.instance.getEggplantPrefab());
            }
            else if(plant.plantType == 4)
            {
                spawnedPlant = Instantiate(PlantManager.instance.getSweetPotatoPrefab());
            }
            else if(plant.plantType == 5)
            {
                spawnedPlant = Instantiate(PlantManager.instance.getPapayaPrefab());
            }
            else
            {
                spawnedPlant = Instantiate(PlantManager.instance.getKalamansiPrefab());
            }
            spawnedPlant.transform.SetParent(transform);
            String reformattingTime = DateTime.Now.ToString(DATETIME_FORMAT);
            TimeSpan elapsedTime = DateTime.ParseExact(reformattingTime, DATETIME_FORMAT, null) - DateTime.ParseExact(plant.plantedTime, DATETIME_FORMAT, null);
            float elapsedSeconds = (float)elapsedTime.TotalSeconds;
            spawnedPlant.transform.localPosition = plant.position;
            spawnedPlant.transform.rotation = transform.rotation;
            spawnedPlant.transform.localScale = new Vector3(0.3f, 2f, 0.3f);
            if(plant.plantType == 1)
            {
                spawnedPlant.TryGetComponent<PlantLogic>(out PlantLogic plantLogic);
                plantLogic.setGrowthAmount(plant.growthAmount + elapsedSeconds);
                plantLogic.setGrowthRate(plant.growthRate);
                plantLogic.setWither(plant.witherTime + elapsedSeconds);
            }
            else if(plant.plantType == 2)
            {
                spawnedPlant.TryGetComponent<LoofaLogic>(out LoofaLogic loofaLogic);
                loofaLogic.setGrowthAmount(plant.growthAmount + elapsedSeconds);
                loofaLogic.setGrowthRate(plant.growthRate);
                loofaLogic.setWither(plant.witherTime + elapsedSeconds);
            }
            else if (plant.plantType == 3)
            {
                spawnedPlant.TryGetComponent<EggplantLogic>(out EggplantLogic eggplant);
                eggplant.setGrowthAmount(plant.growthAmount + elapsedSeconds);
                eggplant.setGrowthRate(plant.growthRate);
                eggplant.setWither(plant.witherTime + elapsedSeconds);
            }
            else if (plant.plantType == 4)
            {
                spawnedPlant.TryGetComponent<SweetpotatoLogic>(out SweetpotatoLogic sweetpotato);
                sweetpotato.setGrowthAmount(plant.growthRate + elapsedSeconds);
                sweetpotato.setGrowthRate(plant.growthRate);
                sweetpotato.setWither(plant.witherTime + elapsedSeconds);
            }
            else if (plant.plantType == 5)
            {
                spawnedPlant.TryGetComponent<PapayaLogic>(out PapayaLogic papaya);
                papaya.setGrowthAmount(plant.growthRate + elapsedSeconds);
                papaya.setGrowthRate(plant.growthRate);
                papaya.setWither(plant.witherTime + elapsedSeconds);
            }
            else
            {
                spawnedPlant.TryGetComponent<CalamansiLogic>(out CalamansiLogic calamansi);
                calamansi.setGrowthAmount(plant.growthRate + elapsedSeconds);
                calamansi.setGrowthRate(plant.growthRate);
                calamansi.setWither(plant.witherTime + elapsedSeconds);
            }
        }
        Debug.Log("Plants spawned, list cleared");
        PlantManager.instance.ClearList();
    }

    private void OnDestroy()
    {

    }
}
