using UnityEngine;

public class ChilliBag : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro chilliStock;

    private void Update()
    {
        chilliStock.text = PlantManager.instance.GetChilliStock().ToString();
    }

    public PlantLogic retrieveChilli()
    {
        return new PlantLogic();
    }


}
