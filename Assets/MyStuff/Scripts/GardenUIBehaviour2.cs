using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GardenUIBehaviour2 : MonoBehaviour
{
    [SerializeField] private PlayerDataSO player;
    [SerializeField] private SaveManagerSO saveManager;
    [SerializeField] private PlantManager plantManager;
    [SerializeField] private DecoManager decoManager;
    [SerializeField] private AudioSource plantSound;

    public const int MAGNIFY = -1;
    public const int WATER = 1;
    public const int FERTILIZER = 2;
    public const int TROWEL = 3;
    public const int CHILLI = 4;
    public const int EGGPLANT = 5;
    public const int LOOFA = 6;
    public const int SWEETPOTATO = 7;
    public const int PAPAYA = 8;
    public const int CALAMANSI = 9;
    public const int FLOWER = 10;
    public int rayDistance = 5;
    private Component equippedItem;
    public GardenUIEvents gardenUIEvents;
    // Start is called before the first frame update
    void Start()
    {
        gardenUIEvents = GetComponent<GardenUIEvents>();
    }

    private void Awake()
    {
        saveManager.Load();
        if(player.GetLastHeldItem() == 10)
        {
            Debug.Log("123 Player is holding decoration!");
            UpdateItem(decoManager.GetFlowerPrefab().transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        gardenUIEvents.setFertiliserText(player.GetFertilizer());
        gardenUIEvents.setWaterText(player.GetWater());
        gardenUIEvents.setCurrentLevel((int) Mathf.Floor(player.GetExp() / 1000));
    }
    public void InsertPlant(Vector3 tapPosition, PlotLogic plotLogic)
    {
        if (player.GetChilliCrop() >= 1)
        {
            plantSound.Play();
            plotLogic.InsertPlant(plantManager.getPlantPrefab(), tapPosition); // Use hit.point for exact position
            player.SetChilliCrop(-1);
            saveManager.Save();
        }
        else
        {
            ThrowError("Oops! Unfortunately, you do not \nhave any Chilli Stock.\nGo to the shop to buy more!");
        }
    }

    public void InsertLoofa(Vector3 tapPosition, PlotLogic plotLogic)
    {
        if (player.GetLoofaCrop() >= 1)
        {
            plantSound.Play();
            plotLogic.InsertPlant(plantManager.getLoofaPrefab(), tapPosition); // Use hit.point for exact position
            player.SetLoofaCrop(-1);
            saveManager.Save();
        }
        else
        {
            ThrowError("Oops! Unfortunately, you do not \nhave any Loofa Stock.\nGo to the shop to buy more!");
        }
    }

    public void InsertEggplant(Vector3 tapPosition, PlotLogic plotLogic)
    {
        if (player.GetEggplantCrop() >= 1)
        {
            plantSound.Play();
            plotLogic.InsertPlant(plantManager.getEggplantPrefab(), tapPosition); // Use hit.point for exact position
            player.SetEggplantCrop(-1);
            saveManager.Save();
        }
        else
        {
            ThrowError("Oops! Unfortunately, you do not \nhave any Eggplant Stock.\nGo to the shop to buy more!");
        }
    }

    public void InsertSweetPotato(Vector3 tapPosition, PlotLogic plotLogic)
    {
        if (player.GetSweetPotatoCrop() >= 1)
        {
            plantSound.Play();
            plotLogic.InsertPlant(plantManager.getSweetPotatoPrefab(), tapPosition); // Use hit.point for exact position
            player.SetSweetPotatoCrop(-1);
            saveManager.Save();
        }
        else
        {
            ThrowError("Oops! Unfortunately, you do not \nhave any Sweet Potato Stock.\nGo to the shop to buy more!");
        }
    }

    public void InsertPapaya(Vector3 tapPosition, PlotLogic plotLogic)
    {
        if (player.GetPapayaCrop() >= 1)
        {
            plantSound.Play();
            plotLogic.InsertPlant(plantManager.getPapayaPrefab(), tapPosition); // Use hit.point for exact position
            player.SetPapayaCrop(-1);
            saveManager.Save();
        }
        else
        {
            ThrowError("Oops! Unfortunately, you do not \nhave any Papaya Stock.\nGo to the shop to buy more!");
        }
    }

    public void InsertKalamansi(Vector3 tapPosition, PlotLogic plotLogic)
    {
        if (player.GetKalamansiCrop() >= 1)
        {
            plantSound.Play();
            plotLogic.InsertPlant(plantManager.getKalamansiPrefab(), tapPosition); // Use hit.point for exact position
            player.SetKalamansiCrop(-1);
            saveManager.Save();
        }
        else
        {
            ThrowError("Oops! Unfortunately, you do not \nhave any Calamansi Stock.\nGo to the shop to buy more!");
        }
    }
    public void UpdateItem(Transform item)
    {
        Debug.Log("123 " + item.IsUnityNull());
        if (item.IsUnityNull())
        {
            Debug.Log("123 UNEQUIPPED!");
            gardenUIEvents.UpdatePickedItem(0);
            equippedItem = null;
        }
        else if (item.TryGetComponent<WaterLogic>(out WaterLogic water))
        {
            gardenUIEvents.UpdatePickedItem(WATER);
            equippedItem = water;
        }
        else if (item.TryGetComponent<FertiliserLogic>(out FertiliserLogic fertiliser))
        {
            gardenUIEvents.UpdatePickedItem(FERTILIZER);
            equippedItem = fertiliser;
        }
        else if (item.TryGetComponent<TrowelLogic>(out TrowelLogic trowelLogic))
        {
            gardenUIEvents.UpdatePickedItem(TROWEL);
            equippedItem = trowelLogic;
        }
        else if (item.TryGetComponent<ChilliBag>(out ChilliBag chilliBag))
        {
            gardenUIEvents.UpdatePickedItem(CHILLI);
            equippedItem = chilliBag;
        }
        else if (item.TryGetComponent<EggplantBag>(out EggplantBag eggplantBag))
        {
            gardenUIEvents.UpdatePickedItem(EGGPLANT);
            equippedItem = eggplantBag;
        }
        else if (item.TryGetComponent<LoofaBag>(out LoofaBag loofaBag))
        {
            gardenUIEvents.UpdatePickedItem(LOOFA);
            equippedItem = loofaBag;
        }
        else if (item.TryGetComponent<SweetPotatoBag>(out SweetPotatoBag sweetPotatoBag))
        {
            gardenUIEvents.UpdatePickedItem(SWEETPOTATO);
            equippedItem = sweetPotatoBag;
        }
        else if (item.TryGetComponent<PapayaBag>(out PapayaBag papayaBag))
        {
            gardenUIEvents.UpdatePickedItem(PAPAYA);
            equippedItem = papayaBag;
        }
        else if (item.TryGetComponent<CalamansiBag>(out CalamansiBag calamansiBag))
        {
            gardenUIEvents.UpdatePickedItem(CALAMANSI);
            equippedItem = calamansiBag;
        }
        else if (item.TryGetComponent<FlowerLogic>(out FlowerLogic flowerLogic))
        {
            Debug.Log("123 Got component Reached here");
            gardenUIEvents.UpdatePickedItem(FLOWER);
            equippedItem = flowerLogic;
        }
        else if (item.TryGetComponent<MagnifierLogic>(out MagnifierLogic magnifierLogic))
        {
            gardenUIEvents.UpdatePickedItem(MAGNIFY);
            equippedItem = magnifierLogic;
        }
    }

    public void ThrowError(string message)
    {
        gardenUIEvents.ThrowError(message);
    }
    public Component getEquipped()
    {
        return equippedItem;
    }
}
