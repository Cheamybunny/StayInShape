using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ScreenshotDisplayEvents : MonoBehaviour
{
    private UIDocument document;
    [SerializeField] ScreenshotPreviewer screenshots;
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
        menuButtons = document.rootVisualElement.Query<Button>().ToList();

        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }

        // screenshots will not be initialised yet. So we must call from screenshot's end.
    }

    public void setScreenshot(Sprite ss)
    {
        if (ss != null)
        {
            var texture = ss.texture;
            screenshotContainer.style.backgroundImage = new StyleBackground(texture);
        }
        else
        {
            Debug.Log("Cannot set screenshot!");
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
