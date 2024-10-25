using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggplantBag : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro eggPlantStock;

    private void Update()
    {
        eggPlantStock.text = PlantManager.instance.GetEggplantStock().ToString();
    }
}
