using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetPotatoBag : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro sweetPotatoStock;

    private void Update()
    {
        sweetPotatoStock.text = PlantManager.instance.GetSweetPotatoCrop().ToString();
    }
}
