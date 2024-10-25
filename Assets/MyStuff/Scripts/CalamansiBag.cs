using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalamansiBag : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro calamansiStock;

    private void Update()
    {
        calamansiStock.text = PlantManager.instance.GetCalamansiCrop().ToString();
    }
}
