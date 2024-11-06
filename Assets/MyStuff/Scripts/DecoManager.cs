using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoManager : MonoBehaviour
{
    public static DecoManager instance;
    [SerializeField] private GameObject flowerPrefab;
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

    public List<DecoData> GetDecorations()
    {
        return player.GetDecorations();
    }

    public void InsertDecoration(DecoData decoData)
    {
        player.InsertDecoration(decoData);
        saveManager.Save();
    }

    public void SetEquipped(int decoType)
    {
        if(decoType == 10)
        {
            gardenUIBehaviour2.UpdateItem(flowerPrefab.transform);
        }
    }
}
