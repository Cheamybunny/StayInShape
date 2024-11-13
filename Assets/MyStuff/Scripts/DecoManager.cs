using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoManager : MonoBehaviour
{
    public static DecoManager instance;
    [SerializeField] private GameObject flowerPrefab;
    [SerializeField] private GameObject flowerPrefab2;
    [SerializeField] private GameObject sunflowerprefab;
    [SerializeField] private GameObject chickenprefab;
    [SerializeField] private GameObject radioprefab;
    [SerializeField] private GameObject gardenUI;
    [SerializeField] private PlayerDataSO player;
    [SerializeField] private SaveManagerSO saveManager;

    private GardenUIBehaviour2 gardenUIBehaviour2;
    void Awake()
    {
        if (instance == null)
        {
            Debug.Log("new manager");
            instance = this;
            gardenUIBehaviour2 = gardenUI.GetComponent<GardenUIBehaviour2>();
            saveManager.Load();
            SetEquipped(player.GetLastHeldItem());
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }

    public static DecoManager getDecoManager()
    {
        return instance;
    }

    public GameObject GetFlowerPrefab()
    {
        return flowerPrefab;
    }

    public GameObject GetFlower2Prefab()
    {
        return flowerPrefab2;
    }

    public GameObject GetSunflowerPrefab()
    {
        return sunflowerprefab;
    }

    public GameObject GetChickenPrefab()
    {
        return chickenprefab;
    }

    public GameObject GetRadioprefab()
    {
        return radioprefab;
    }
    public List<DecoData> GetDecorations()
    {
        return player.GetDecorations();
    }

    public void InsertDecoration(DecoData decoData)
    {
        player.InsertDecoration(decoData);
        saveManager.Save();
        Debug.Log("123 Deco saved");
    }

    public void SetEquipped(int decoType)
    {
        if(decoType == 10)
        {
            gardenUIBehaviour2.UpdateItem(flowerPrefab.transform);
        }
        else if(decoType == 11)
        {
            gardenUIBehaviour2.UpdateItem(flowerPrefab2.transform);
        }
        else if(decoType == 12)
        {
            gardenUIBehaviour2.UpdateItem(sunflowerprefab.transform);
        }
        else if(decoType == 13)
        {
            gardenUIBehaviour2.UpdateItem(chickenprefab.transform);
        }
        else if (decoType == 14)
        {
            gardenUIBehaviour2.UpdateItem(radioprefab.transform);
        }
    }
}
