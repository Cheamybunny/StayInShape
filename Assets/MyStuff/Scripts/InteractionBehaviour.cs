using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class InteractionBehaviour : MonoBehaviour
{
    [SerializeField] GardenUIBehaviour2 gardenUIBehaviour2;
    [SerializeField] PlantManager plantManager;
    [SerializeField] PlayerDataSO player;
    [SerializeField] SaveManagerSO saveManager;
    [SerializeField] AudioSource buttonClick;
    [SerializeField] GardenUIEvents gardenUIEvents;
    DefaultInputActions actions;

    private void Awake()
    {
        actions = new DefaultInputActions();
        actions.Enable();
        saveManager.Load();
    }
    private void OnDestroy()
    {
        Debug.Log("InteractionBehaviour destroyed");
        actions.Disable();
    }

    private void InsertPlant(GameObject plant, PlotLogic plot, Vector3 position)
    {
        if (plant.TryGetComponent<ChilliBag>(out ChilliBag chilliBag))
        {
            gardenUIBehaviour2.InsertPlant(position, plot);
        }
        else if(plant.TryGetComponent<EggplantBag>(out EggplantBag eggplantBag))
        {
            gardenUIBehaviour2.InsertEggplant(position, plot);
        }
        else if (plant.TryGetComponent<LoofaBag>(out LoofaBag loofaBag))
        {
            gardenUIBehaviour2.InsertLoofa(position, plot);
        }
        else if(plant.TryGetComponent<SweetPotatoBag>(out SweetPotatoBag sweetPotatoBag))
        {
            gardenUIBehaviour2.InsertSweetPotato(position, plot);
        }
        else if (plant.TryGetComponent<PapayaBag>(out PapayaBag papayaBag))
        {
            gardenUIBehaviour2.InsertPapaya(position, plot);
        }
        else if(plant.TryGetComponent<CalamansiBag>(out CalamansiBag calamansiBag))
        {
            gardenUIBehaviour2.InsertKalamansi(position, plot);
        }
        else
        {
            Debug.Log("Crop not in yet");
        }
    }

    private void Update()
    {
        //determines how "near" player needs to be to interact with the assets
        int rayDistance = 5;
        if (actions.UI.Click.WasPressedThisFrame())
        {
            //When users tap the screen, shoots a ray
            Vector2 clickPosition = actions.UI.Point.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(clickPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                Debug.Log(hit.transform.gameObject.name);
                if (hit.transform.TryGetComponent<PlotLogic>(out PlotLogic plotLogic))
                {
                    Component heldItem = gardenUIBehaviour2.getEquipped();
                    if (heldItem != null && !heldItem.TryGetComponent<WaterLogic>(out WaterLogic water) &&
                        !heldItem.TryGetComponent<FertiliserLogic>(out FertiliserLogic fertiliser) &&
                        !heldItem.TryGetComponent<TrowelLogic>(out TrowelLogic trowel))
                    {
                        InsertPlant(heldItem.gameObject, plotLogic, hit.point);
                    }
                }
                //logic if player taps the garden
                else if(hit.transform.TryGetComponent<GardenLogic>(out GardenLogic garden))
                {
                    Debug.Log("123 YOU HITTING THE GARDEN CUH");
                    Component heldItem = gardenUIBehaviour2.getEquipped();
                    if (heldItem != null && heldItem.TryGetComponent<FlowerLogic>(out FlowerLogic flower))
                    {
                        Debug.Log("123 I KNOW YOU HOLDING FLOWER CUH");
                        player.SetLastHeldItem(-1);
                        saveManager.Save();
                        garden.InsertDecoration(DecoManager.instance.GetFlowerPrefab(), hit.point);
                        gardenUIBehaviour2.UpdateItem(null);
                    }
                    else if (heldItem != null && heldItem.TryGetComponent<Flower2Logic>(out Flower2Logic flower2Logic))
                    {
                        player.SetLastHeldItem(-1);
                        saveManager.Save();
                        garden.InsertDecoration(DecoManager.instance.GetFlower2Prefab(), hit.point);
                        gardenUIBehaviour2.UpdateItem(null);
                    }
                    else if (heldItem != null && heldItem.TryGetComponent<SunflowerLogic>(out SunflowerLogic sunflower))
                    {
                        player.SetLastHeldItem(-1);
                        saveManager.Save();
                        garden.InsertDecoration(DecoManager.instance.GetSunflowerPrefab(), hit.point);
                        gardenUIBehaviour2.UpdateItem(null);
                    }
                    else if (heldItem != null && heldItem.TryGetComponent<ChickenDecorLogic>(out ChickenDecorLogic chickenDecor))
                    {
                        player.SetLastHeldItem(-1);
                        saveManager.Save();
                        garden.InsertDecoration(DecoManager.instance.GetChickenPrefab(), hit.point);
                        gardenUIBehaviour2.UpdateItem(null);
                    }
                    else if (heldItem != null && heldItem.TryGetComponent<RadioLogic>(out RadioLogic radio))
                    {
                        player.SetLastHeldItem(-1);
                        saveManager.Save();
                        garden.InsertDecoration(DecoManager.instance.GetRadioprefab(), hit.point);
                        gardenUIBehaviour2.UpdateItem(null);
                    }
                }
                //logic if player taps bag of seeds
                else if (hit.transform.TryGetComponent<ChilliBag>(out ChilliBag chilliBag) ||
                    hit.transform.TryGetComponent<EggplantBag>(out EggplantBag eggplantBag) ||
                    hit.transform.TryGetComponent<LoofaBag>(out LoofaBag loofaBag) ||
                    hit.transform.TryGetComponent<SweetPotatoBag>(out SweetPotatoBag sweetPotatoBag) ||
                    hit.transform.TryGetComponent<CalamansiBag>(out CalamansiBag calamansiBag) ||
                    hit.transform.TryGetComponent<PapayaBag>(out PapayaBag papayaBag))
                {
                    if(player.GetLastHeldItem() < 10)
                    {
                        buttonClick.Play();
                        gardenUIBehaviour2.UpdateItem(hit.transform);
                    }
                    else
                    {
                        gardenUIEvents.ThrowError(ErrorManager.instance.HoldingDecorationError());
                    }
                }
                //logic if player taps a plant
                else if (hit.transform.TryGetComponent<PlantLogic>(out PlantLogic plant))
                {
                    if (plant.HarvestPlant())
                    {
                        Debug.Log("200xp gained!");
                        player.SetExp(200);
                        player.SetChilliCrop(1);
                        saveManager.Save();
                    }
                    else
                    {
                        if (gardenUIBehaviour2.getEquipped() != null)
                        {
                            if (gardenUIBehaviour2.getEquipped().GetType() == typeof(FertiliserLogic))
                            {
                                if (player.GetFertilizer() >= 1)
                                {
                                    if (plant.Insert(gardenUIBehaviour2.getEquipped()))
                                    {
                                        player.SetFertilizer(player.GetFertilizer() - 1);
                                        saveManager.Save();
                                    }
                                    else
                                    {
                                        Debug.Log("ERROR HERE");
                                        gardenUIBehaviour2.ThrowError(ErrorManager.instance.CropWitheredError());
                                    }
                                }
                                else
                                {
                                    gardenUIBehaviour2.ThrowError(ErrorManager.instance.NoResourcesError("Fertilizer"));
                                }
                            }
                            else if (gardenUIBehaviour2.getEquipped().GetType() == typeof(WaterLogic))
                            {
                                if (player.GetWater() >= 1)
                                {
                                    if (plant.Insert(gardenUIBehaviour2.getEquipped()))
                                    {
                                        player.SetWater(player.GetWater() - 1);
                                        saveManager.Save();
                                    }
                                    else
                                    {
                                        gardenUIBehaviour2.ThrowError(ErrorManager.instance.CropWitheredError());
                                    }
                                }
                                else
                                {
                                    gardenUIBehaviour2.ThrowError(ErrorManager.instance.NoResourcesError("Water"));
                                }
                            }
                            else if(gardenUIBehaviour2.getEquipped().GetType() == typeof(MagnifierLogic))
                            {
                                gardenUIBehaviour2.ThrowError(plant.getStatus());
                            }
                            else if (gardenUIBehaviour2.getEquipped().GetType() == typeof(TrowelLogic))
                            {
                                plant.DestroyPlant();
                            }
                        }
                        else
                        {
                            Debug.Log("nothing equipped");
                        }
                    }
                }
                else if(hit.transform.TryGetComponent<LoofaLogic>(out LoofaLogic loofa))
                {
                    if (loofa.HarvestPlant())
                    {
                        Debug.Log("1000xp gained!");
                        player.SetExp(1000);
                        player.SetLoofaCrop(1);
                        saveManager.Save();
                    }
                    else
                    {
                        if (gardenUIBehaviour2.getEquipped() != null)
                        {
                            if (gardenUIBehaviour2.getEquipped().GetType() == typeof(FertiliserLogic))
                            {
                                if (player.GetFertilizer() >= 1)
                                {
                                    if (loofa.Insert(gardenUIBehaviour2.getEquipped()))
                                    {
                                        player.SetFertilizer(player.GetFertilizer() - 1);
                                        saveManager.Save();
                                    }
                                    else
                                    {
                                        gardenUIBehaviour2.ThrowError(ErrorManager.instance.CropWitheredError());
                                    }
                                }
                                else
                                {
                                    gardenUIBehaviour2.ThrowError(ErrorManager.instance.NoResourcesError("Fertiliser"));
                                }
                            }
                            else if (gardenUIBehaviour2.getEquipped().GetType() == typeof(WaterLogic))
                            {
                                if (player.GetWater() >= 1)
                                {
                                    if (loofa.Insert(gardenUIBehaviour2.getEquipped()))
                                    {
                                        player.SetWater(player.GetWater() - 1);
                                        saveManager.Save();
                                    }
                                    else
                                    {
                                        gardenUIBehaviour2.ThrowError(ErrorManager.instance.CropWitheredError());
                                    }
                                }
                                else
                                {
                                    gardenUIBehaviour2.ThrowError(ErrorManager.instance.NoResourcesError("Water"));
                                }
                            }
                            else if(gardenUIBehaviour2.getEquipped().GetType() == typeof(MagnifierLogic))
                            {
                                gardenUIBehaviour2.ThrowError(loofa.getStatus());
                            }
                            else if (gardenUIBehaviour2.getEquipped().GetType() == typeof(TrowelLogic))
                            {
                                loofa.DestroyPlant();
                            }
                        }
                        else
                        {
                            Debug.Log("nothing equipped");
                        }
                    }
                }
                else if (hit.transform.TryGetComponent<EggplantLogic>(out EggplantLogic eggplant))
                {
                    if (eggplant.HarvestPlant())
                    {
                        Debug.Log("500xp gained!");
                        player.SetExp(500);
                        player.SetEggplantCrop(1);
                        saveManager.Save();
                    }
                    else
                    {
                        if (gardenUIBehaviour2.getEquipped() != null)
                        {
                            if (gardenUIBehaviour2.getEquipped().GetType() == typeof(FertiliserLogic))
                            {
                                if (player.GetFertilizer() >= 1)
                                {
                                    if (eggplant.Insert(gardenUIBehaviour2.getEquipped()))
                                    {
                                        player.SetFertilizer(player.GetFertilizer() - 1);
                                        saveManager.Save();
                                    }
                                    else
                                    {
                                        gardenUIBehaviour2.ThrowError(ErrorManager.instance.CropWitheredError());
                                    }
                                }
                                else
                                {
                                    gardenUIBehaviour2.ThrowError(ErrorManager.instance.NoResourcesError("Fertiliser"));
                                }
                            }
                            else if (gardenUIBehaviour2.getEquipped().GetType() == typeof(WaterLogic))
                            {
                                if (player.GetWater() >= 1)
                                {
                                    if (eggplant.Insert(gardenUIBehaviour2.getEquipped()))
                                    {
                                        player.SetWater(player.GetWater() - 1);
                                        saveManager.Save();
                                    }
                                    else
                                    {
                                        gardenUIBehaviour2.ThrowError(ErrorManager.instance.CropWitheredError());
                                    }
                                }
                                else
                                {
                                    gardenUIBehaviour2.ThrowError(ErrorManager.instance.NoResourcesError("Water"));
                                }
                            }
                            else if(gardenUIBehaviour2.getEquipped().GetType() == typeof(MagnifierLogic))
                            {
                                gardenUIBehaviour2.ThrowError(eggplant.getStatus());
                            }
                            else if (gardenUIBehaviour2.getEquipped().GetType() == typeof(TrowelLogic))
                            {
                                eggplant.DestroyPlant();
                            }
                        }
                        else
                        {
                            Debug.Log("nothing equipped");
                        }
                    }
                }
                else if (hit.transform.TryGetComponent<SweetpotatoLogic>(out SweetpotatoLogic sweetpotato))
                {
                    if (sweetpotato.HarvestPlant())
                    {
                        Debug.Log("1200xp gained!");
                        player.SetExp(1200);
                        player.SetSweetPotatoCrop(1);
                        saveManager.Save();
                    }
                    else
                    {
                        if (gardenUIBehaviour2.getEquipped() != null)
                        {
                            if (gardenUIBehaviour2.getEquipped().GetType() == typeof(FertiliserLogic))
                            {
                                if (player.GetFertilizer() >= 1)
                                {
                                    if (sweetpotato.Insert(gardenUIBehaviour2.getEquipped()))
                                    {
                                        player.SetFertilizer(player.GetFertilizer() - 1);
                                        saveManager.Save();
                                    }
                                    else
                                    {
                                        gardenUIBehaviour2.ThrowError(ErrorManager.instance.CropWitheredError());
                                    }
                                }
                                else
                                {
                                    gardenUIBehaviour2.ThrowError(ErrorManager.instance.NoResourcesError("Fertiliser"));
                                }
                            }
                            else if (gardenUIBehaviour2.getEquipped().GetType() == typeof(WaterLogic))
                            {
                                if (player.GetWater() >= 1)
                                {
                                    if (sweetpotato.Insert(gardenUIBehaviour2.getEquipped()))
                                    {
                                        player.SetWater(player.GetWater() - 1);
                                        saveManager.Save();
                                    }
                                    else
                                    {
                                        gardenUIBehaviour2.ThrowError(ErrorManager.instance.CropWitheredError());
                                    }
                                }
                                else
                                {
                                    gardenUIBehaviour2.ThrowError(ErrorManager.instance.NoResourcesError("Water"));
                                }
                            }
                            else if(gardenUIBehaviour2.getEquipped().GetType() == typeof(MagnifierLogic))
                            {
                                gardenUIBehaviour2.ThrowError(sweetpotato.getStatus());
                            }
                            else if (gardenUIBehaviour2.getEquipped().GetType() == typeof(TrowelLogic))
                            {
                                sweetpotato.DestroyPlant();
                            }
                        }
                        else
                        {
                            Debug.Log("nothing equipped");
                        }
                    }
                }
                else if (hit.transform.TryGetComponent<PapayaLogic>(out PapayaLogic papaya))
                {
                    if (papaya.HarvestPlant())
                    {
                        Debug.Log("2000xp gained!");
                        player.SetExp(2000);
                        player.SetPapayaCrop(1);
                        saveManager.Save();
                    }
                    else
                    {
                        if (gardenUIBehaviour2.getEquipped() != null)
                        {
                            if (gardenUIBehaviour2.getEquipped().GetType() == typeof(FertiliserLogic))
                            {
                                if (player.GetFertilizer() >= 1)
                                {
                                    if (papaya.Insert(gardenUIBehaviour2.getEquipped()))
                                    {
                                        player.SetFertilizer(player.GetFertilizer() - 1);
                                        saveManager.Save();
                                    }
                                    else
                                    {
                                        gardenUIBehaviour2.ThrowError(ErrorManager.instance.CropWitheredError());
                                    }
                                }
                                else
                                {
                                    gardenUIBehaviour2.ThrowError(ErrorManager.instance.NoResourcesError("Fertiliser"));
                                }
                            }
                            else if (gardenUIBehaviour2.getEquipped().GetType() == typeof(WaterLogic))
                            {
                                if (player.GetWater() >= 1)
                                {
                                    if (papaya.Insert(gardenUIBehaviour2.getEquipped()))
                                    {
                                        player.SetWater(player.GetWater() - 1);
                                        saveManager.Save();
                                    }
                                    else
                                    {
                                        gardenUIBehaviour2.ThrowError(ErrorManager.instance.CropWitheredError());
                                    }
                                }
                                else
                                {
                                    gardenUIBehaviour2.ThrowError(ErrorManager.instance.NoResourcesError("Water"));
                                }
                            }
                            else if (gardenUIBehaviour2.getEquipped().GetType() == typeof(TrowelLogic))
                            {
                                papaya.DestroyPlant();
                            }
                            else if (gardenUIBehaviour2.getEquipped().GetType() == typeof(MagnifierLogic))
                            {
                                gardenUIBehaviour2.ThrowError(papaya.getStatus());
                            }
                        }
                        else
                        {
                            Debug.Log("nothing equipped");
                        }
                    }
                }
                else if (hit.transform.TryGetComponent<CalamansiLogic>(out CalamansiLogic calamansi))
                {
                    if (calamansi.HarvestPlant())
                    {
                        Debug.Log("1500xp gained!");
                        player.SetExp(1500);
                        player.SetKalamansiCrop(1);
                        saveManager.Save();
                    }
                    else
                    {
                        if (gardenUIBehaviour2.getEquipped() != null)
                        {
                            if (gardenUIBehaviour2.getEquipped().GetType() == typeof(FertiliserLogic))
                            {
                                if (player.GetFertilizer() >= 1)
                                {
                                    if (calamansi.Insert(gardenUIBehaviour2.getEquipped()))
                                    {
                                        player.SetFertilizer(player.GetFertilizer() - 1);
                                        saveManager.Save();
                                    }
                                    else
                                    {
                                        gardenUIBehaviour2.ThrowError(ErrorManager.instance.CropWitheredError());
                                    }
                                }
                                else
                                {
                                    gardenUIBehaviour2.ThrowError(ErrorManager.instance.NoResourcesError("Fertiliser"));
                                }
                            }
                            else if (gardenUIBehaviour2.getEquipped().GetType() == typeof(WaterLogic))
                            {
                                if (player.GetWater() >= 1)
                                {
                                    if (calamansi.Insert(gardenUIBehaviour2.getEquipped()))
                                    {
                                        player.SetWater(player.GetWater() - 1);
                                        saveManager.Save();
                                    }
                                    else
                                    {
                                        gardenUIBehaviour2.ThrowError(ErrorManager.instance.CropWitheredError());
                                    }
                                }
                                else
                                {
                                    gardenUIBehaviour2.ThrowError(ErrorManager.instance.NoResourcesError("Water"));
                                }
                            }
                            else if (gardenUIBehaviour2.getEquipped().GetType() == typeof(MagnifierLogic))
                            {
                                gardenUIBehaviour2.ThrowError(calamansi.getStatus());
                            }
                            else if (gardenUIBehaviour2.getEquipped().GetType() == typeof(TrowelLogic))
                            {
                                calamansi.DestroyPlant();
                            }
                        }
                        else
                        {
                            Debug.Log("nothing equipped");
                        }
                    }
                }
                else
                {
                    //if not plant, then check if player trying to equip fertilizer or water OR trowel
                    if ((hit.transform.TryGetComponent<WaterLogic>(out WaterLogic water) || 
                        (hit.transform.TryGetComponent<FertiliserLogic>(out FertiliserLogic fertiliser)) ||
                        hit.transform.TryGetComponent<TrowelLogic>(out TrowelLogic trowel) ||
                        hit.transform.TryGetComponent<MagnifierLogic>(out MagnifierLogic magnifier)))
                    {
                        if(player.GetLastHeldItem() < 10)
                        {
                            buttonClick.Play();
                            gardenUIBehaviour2.UpdateItem(hit.transform);
                        }
                        else
                        {
                            gardenUIEvents.ThrowError(ErrorManager.instance.HoldingDecorationError());
                        }
                    }
                }
            }
        }
    }
}
