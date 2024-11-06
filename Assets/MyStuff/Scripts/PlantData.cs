using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlantData
{
    public String plantedTime;
    public Vector3 position;
    public float growthAmount;
    public float growthRate;
    public float witherTime;
    public int plantType;

    public PlantData(String plantedTime, Vector3 position, float growthAmount, float growthRate, float witherTime, int plantType)
    {
        this.plantedTime = plantedTime;
        this.position = position;
        this.growthAmount = growthAmount;
        this.growthRate = growthRate;
        this.witherTime = witherTime;
        this.plantType = plantType;
    }
}
