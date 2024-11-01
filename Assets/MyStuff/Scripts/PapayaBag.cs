using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapayaBag : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro papayaStock;

    private void Update()
    {
        papayaStock.text = PlantManager.instance.GetPapayaCrop().ToString();
    }
}
