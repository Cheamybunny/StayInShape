using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public static TestManager instance;
    [SerializeField] private GameObject destinycubePrefab;
    [SerializeField] private TestEvents testEvents;
    DefaultInputActions actions;

    private Renderer targetRenderer;
    private int currLevel = 1;
    private GameObject[] spawnedCubes;
    private GameObject[] spawnedTargets;
    private float result1;
    private float result2;
    private float result3;
    public float distanceBetweenCubes = 2f; // Distance between each cubes for lvl2
    public float timeToMemorise;
    private int[] coloursShown;
    private int cubesDestroyed = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            actions = new DefaultInputActions();
            actions.Enable();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        actions.Disable();
    }

    private void Start()
    {

    }

    public int GetCurrLevel()
    {
        return currLevel;
    }

    public void Level1()
    {
        float spawnDistance = 5f;
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;

        // Calculate the spawn position (camera position + forward direction * spawnDistance)
        Vector3 spawnPosition = cameraPosition + cameraForward * spawnDistance;

        // Instantiate the cube at the calculated position
        GameObject spawnedCube = Instantiate(destinycubePrefab, spawnPosition, Quaternion.identity);
        targetRenderer = spawnedCube.GetComponentInChildren<Renderer>();
        StartCoroutine(WaitForPlayerTap());
    }
    public void Level2(float timeMemorise, int[] colours)
    {

        // Initialize the array to hold the references to the cubes
        spawnedCubes = new GameObject[5];

        for (int i = 0; i < 5; i++)
        {
            // Calculate the position for each cube along the camera's forward direction
            Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 5 + Camera.main.transform.right * (distanceBetweenCubes * (i + 1));

            // Instantiate the cube at the calculated position
            spawnedCubes[i] = Instantiate(destinycubePrefab, spawnPosition, Quaternion.identity);

            // Make the cube face the camera
            spawnedCubes[i].transform.LookAt(Camera.main.transform);
        }
        timeToMemorise = timeMemorise;
        coloursShown = colours;
        StartCoroutine(WaitForPlayerRecall());
    }

    public void Level3()
    {
        float distFromcamera = 5f;
        float maxLeft = -3f;
        float maxright = 3f;
        float maxtop = 3f;
        float maxbottom = -3f;

        spawnedTargets = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            //generate random position
            Vector3 spawnPosition = Camera.main.transform.position +
                (Camera.main.transform.forward * distFromcamera) +
                (Camera.main.transform.right * Random.Range(maxLeft, maxright)) +
                (Camera.main.transform.up * Random.Range(maxbottom, maxtop));
            // Instantiate the cube at the calculated position
            spawnedTargets[i] = Instantiate(destinycubePrefab, spawnPosition, Quaternion.identity);

            // Make the cube face the camera
            spawnedTargets[i].transform.LookAt(Camera.main.transform);
            spawnedTargets[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        StartCoroutine(WaitForPlayerShoot());
    }

    private IEnumerator WaitForPlayerShoot()
    {
        float startTime = Time.time;
        while(cubesDestroyed != 3)
        {
            // Check if the player clicks on the object
            if (actions.UI.Click.WasPressedThisFrame())
            {
                Vector2 clickPosition = actions.UI.Point.ReadValue<Vector2>();
                Ray ray = Camera.main.ScreenPointToRay(clickPosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // Check if the player clicked the target object
                    if (hit.collider.gameObject.TryGetComponent<DestinyCube>(out DestinyCube destiny))
                    {
                        destiny.Shot();
                        cubesDestroyed++;
                    }
                }
            }
            // Yield for a frame before checking again
            yield return null;
        }
        float timeFinish = Time.time - startTime;
        result3 = timeFinish;
        Debug.Log("Player destroyed all cubes in " + timeFinish + " seconds.");
        testEvents.DisplayResults(result1, result2, result3);
    }

    private IEnumerator WaitForPlayerTap()
    {
        // Pick a random time between 3 and 8 seconds
        float timeToWait = Random.Range(3f, 8f);

        // Wait for the random duration
        yield return new WaitForSeconds(timeToWait);

        // Change the color of the object to green
        targetRenderer.material.color = Color.green;

        // Start measuring the time from when the object turns green
        float startTime = Time.time;

        // Wait until the player clicks on the object
        bool playerClicked = false;

        while (!playerClicked)
        {
            // Check if the player clicks on the object
            if (actions.UI.Click.WasPressedThisFrame())
            {
                Vector2 clickPosition = actions.UI.Point.ReadValue<Vector2>();
                Ray ray = Camera.main.ScreenPointToRay(clickPosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // Check if the player clicked the target object
                    if (hit.collider.gameObject.TryGetComponent<DestinyCube>(out DestinyCube destiny))
                    {
                        playerClicked = true;

                        // Calculate how long it took for the player to tap
                        float timeClicked = Time.time - startTime;
                        Destroy(destiny.gameObject);
                        Debug.Log("Player clicked the object after " + timeClicked + " seconds.");
                        result1 = timeClicked;
                        currLevel = 2;
                        testEvents.NextLevel(currLevel);
                    }
                }
            }

            // Yield for a frame before checking again
            yield return null;
        }
    }

    private bool arrayChecker()
    {
        for (int i = 0; i < 5; i++)
        {
            spawnedCubes[i].TryGetComponent<DestinyCube>(out DestinyCube destiny);
            if(destiny.GetColor() == coloursShown[i])
            {
                continue;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator WaitForPlayerRecall()
    {

        float startTime = Time.time;

        while (!arrayChecker())
        {
            // Check if the player clicks on the object
            if (actions.UI.Click.WasPressedThisFrame())
            {
                Vector2 clickPosition = actions.UI.Point.ReadValue<Vector2>();
                Ray ray = Camera.main.ScreenPointToRay(clickPosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // Check if the player clicked the target object
                    if (hit.collider.gameObject.TryGetComponent<DestinyCube>(out DestinyCube destiny))
                    {
                        destiny.ChangeColor();
                    }
                }
            }

            // Yield for a frame before checking again
            yield return null;
        }
        for(int i = 0;i < 5; i++)
        {
            Destroy(spawnedCubes[i].gameObject);
        }
        float timefinished = Time.time - startTime;
        float totalTimetaken = timefinished + timeToMemorise;
        result2 = totalTimetaken;
        Debug.Log("Player managed to memorise in " + totalTimetaken + " seconds.");
        currLevel = 3;
        testEvents.NextLevel(currLevel);
    }

}
