using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DecoData
{
    public Vector3 position;
    public int decoType;

    public DecoData(Vector3 position, int decoType)
    {
        this.position = position;
        this.decoType = decoType;
    }
}
