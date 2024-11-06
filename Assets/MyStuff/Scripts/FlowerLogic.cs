using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FlowerLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public DecoData ToDecoData()
    {
        return new DecoData(transform.localPosition, 1);
    }

    private void OnDestroy()
    {
        DecoManager.instance.InsertDecoration(this.ToDecoData());
    }
}
