using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenCalibration : MonoBehaviour
{
    [SerializeField] GameObject garden;
    GardenUIEvents gardenUIEvent;

    // Start is called before the first frame update
    void Start()
    {
        GardenUIEvents[] objs = FindObjectsOfType<GardenUIEvents>();

        foreach (GardenUIEvents gardenUI in objs) // There should only be one
        {
            if (objs.Length > 1)
            {
                throw new System.Exception("There are should not be more than one GardenUIEvent in the scene!");
            }
            Debug.Log("Found GameObject with GardenUIEvents: " + gardenUI.gameObject.name);
            gardenUIEvent = gardenUI;
        }

        if (gardenUIEvent == null)
        {
            throw new System.Exception("No GardenUIEvent found in the scene!");
        }

        gardenUIEvent.SetUp(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnGarden()
    {
        garden.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
