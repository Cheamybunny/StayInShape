using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ScreenshotDisplayEvents : MonoBehaviour
{
    private UIDocument document;

    private Button button1;
    private Button button2;

    private VisualElement screenshotContainer;
    
    private List<Button> menuButtons = new List<Button>();

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        document = GetComponent<UIDocument>();

        button1 = document.rootVisualElement.Q("SaveButton") as Button;
        button1.RegisterCallback<ClickEvent>(OnSaveClick);

        button2 = document.rootVisualElement.Q("CancelButton") as Button;
        button2.RegisterCallback<ClickEvent>(OnCancelClick);

        screenshotContainer = document.rootVisualElement.Q("ScreenshotContainer") as VisualElement;
        if (GardenUIEvents.capturedScreenshot != null)
        {
            Debug.Log("Displaying SS");
            // Convert Texture2D to a usable format for UI Toolkit
            var sprite = Sprite.Create(GardenUIEvents.capturedScreenshot, new Rect(0, 0, GardenUIEvents.capturedScreenshot.width, GardenUIEvents.capturedScreenshot.height), new Vector2(0.5f, 0.5f));
            var texture = sprite.texture;

            screenshotContainer.style.backgroundImage = new StyleBackground(texture);
        }

        menuButtons = document.rootVisualElement.Query<Button>().ToList();

        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnDisable()
    {
        button1.UnregisterCallback<ClickEvent>(OnSaveClick);
        button2.UnregisterCallback<ClickEvent>(OnCancelClick);

        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnSaveClick(ClickEvent evt)
    {
        Debug.Log("You pressed Save Button");

        SaveScreenshot();
    }

    private void SaveScreenshot()
    {
        // Define the path to save the screenshot in the public Pictures/Screenshots directory
        string directoryPath = Path.Combine("/storage/emulated/0/Pictures/Screenshots");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string filePath = Path.Combine(directoryPath, $"Screenshot_{System.DateTime.Now:yyyyMMdd_HHmmss}.png");
        byte[] bytes = GardenUIEvents.capturedScreenshot.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);

    #if UNITY_ANDROID
        // Refresh the Android gallery so the image appears in the standard gallery app
        AndroidJavaClass mediaScanner = new AndroidJavaClass("android.media.MediaScannerConnection");
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        mediaScanner.CallStatic("scanFile", currentActivity, new string[] { filePath }, null, null);
    #endif

        Debug.Log($"Screenshot saved to: {filePath}");
        UnityEngine.SceneManagement.SceneManager.LoadScene("OriginalScene");
    }

    private void OnCancelClick(ClickEvent evt)
    {
        Debug.Log("You pressed Cancel Button");

        SceneManager.LoadScene("GardenSceneJia");
    }
    
    private void OnAllButtonsClick(ClickEvent evt)
    {
        audioSource.Play();
    }
}
