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
        
    }

    private void LoadDecos()
    {
        decoDataList = DecoManager.instance.GetDecorations();
        foreach(var decoData in decoDataList)
        {
            GameObject spawnedDeco;
            if(decoData.decoType == 1)
            {
                spawnedDeco = Instantiate(DecoManager.instance.GetFlowerPrefab());
            }
            else
            {
                spawnedDeco = Instantiate(DecoManager.instance.GetFlowerPrefab()); //yardstick for when more decos are added
            }
            spawnedDeco.transform.SetParent(transform);
            spawnedDeco.transform.localPosition = decoData.position;
            spawnedDeco.transform.rotation = transform.rotation;
            spawnedDeco.transform.localScale = new Vector3(2f, 2f, 2f);
        }
    }

    /**
    private void OnEnable()
    {
        if (imageManager != null)
        {
            imageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
    }

    private void OnDisable()
    {
        if (imageManager != null)
        {
            imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            // Set the position and scale of this object to match the tracked image
            transform.position = trackedImage.transform.position;
            transform.rotation = trackedImage.transform.rotation;
            transform.localScale = scaleFactor;

            // Optionally, set this object as a child of the tracked image
            transform.SetParent(trackedImage.transform);
            transform.localPosition += new Vector3(0, 2f, -5f);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            transform.position = trackedImage.transform.position;
            transform.rotation = trackedImage.transform.rotation;
        }
    }
    **/
    private void Update()
    {

    }

    public void InsertDecoration(GameObject decoPrefab, Vector3 position)
    {
        //get relative position of plant with soil
        Debug.Log("Insert Here");
        GameObject deco = Instantiate(decoPrefab);
        deco.transform.SetParent(transform);
        deco.transform.localPosition = transform.InverseTransformPoint(position);
        deco.transform.rotation = transform.rotation;
        deco.transform.localScale = new Vector3(2f, 2f, 2f);
    }

    void OnDestroy()
    {
        imageManager.enabled = true;
    }
}
