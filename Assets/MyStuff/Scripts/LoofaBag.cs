using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoofaBag : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro loofaStock;

    private void Update()
    {
        loofaStock.text = PlantManager.instance.GetLoofaStock().ToString();
    }
}
