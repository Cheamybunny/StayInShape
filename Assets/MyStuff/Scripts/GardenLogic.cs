using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARFoundation;

public class GardenLogic : MonoBehaviour
{
    private ARTrackedImageManager imageManager;

    private List<DecoData> decoDataList;

    private void Awake()
    {
        // Find the ARTrackedImageManager in the scene
        imageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    private void Start()
    {
        Debug.Log("Gameobject position " + gameObject.transform.position);
        Debug.Log("Camera position " + Camera.main.transform.position);
        Debug.Log("Garden scale " + gameObject.transform.localScale);
        transform.localPosition +=  new Vector3(0, -0.5f, 0);
        imageManager.enabled = false;
        LoadDecos();
        
    }

    private void LoadDecos()
    {
        decoDataList = DecoManager.instance.GetDecorations();
        foreach(var decoData in decoDataList)
        {
            GameObject spawnedDeco;
            if (decoData.decoType == 1)
            {
                spawnedDeco = Instantiate(DecoManager.instance.GetFlowerPrefab());
            }
            else if (decoData.decoType == 2)
            {
                spawnedDeco = Instantiate(DecoManager.instance.GetFlower2Prefab());
            }
            else if (decoData.decoType == 3)
            {
                spawnedDeco = Instantiate(DecoManager.instance.GetSunflowerPrefab());
            }
            else if (decoData.decoType == 4)
            {
                spawnedDeco = Instantiate(DecoManager.instance.GetChickenPrefab());
            }
            else if (decoData.decoType == 5)
            {
                spawnedDeco = Instantiate(DecoManager.instance.GetRadioprefab());
            }
            else
            {
                spawnedDeco = Instantiate(DecoManager.instance.GetFlowerPrefab());
            }
            spawnedDeco.transform.SetParent(transform);
            spawnedDeco.transform.localPosition = decoData.position;
            spawnedDeco.transform.rotation = transform.rotation;
            spawnedDeco.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        DecoManager.instance.ClearDecos();
    }

    private void Update()
    {

    }

    public void InsertDecoration(GameObject decoPrefab, Vector3 position)
    {
        //get relative position of plant with soil
        Debug.Log("Insert Here");
        GameObject deco = Instantiate(decoPrefab);
        deco.transform.SetParent(transform);
        deco.transform.localPosition = transform.InverseTransformPoint(position) + new Vector3(0f, 0.05f, 0f);
        deco.transform.rotation = transform.rotation;
        deco.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    void OnDestroy()
    {
        imageManager.enabled = true;
    }
}
