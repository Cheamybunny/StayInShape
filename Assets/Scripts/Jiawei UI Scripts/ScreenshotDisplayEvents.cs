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
    private Button button3;
    private Button button4;

    private VisualElement screenshotContainer;
    
    private List<Button> menuButtons = new List<Button>();

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        document = GetComponent<UIDocument>();

        button1 = document.rootVisualElement.Q("CancelButton") as Button;
        button1.RegisterCallback<ClickEvent>(OnCancelClick);

        button2 = document.rootVisualElement.Q("DeleteButton") as Button;
        button2.RegisterCallback<ClickEvent>(OnDeleteClick);

        button3 = document.rootVisualElement.Q("LeftButton") as Button;
        button3.RegisterCallback<ClickEvent>(OnLeftClick);

        button4 = document.rootVisualElement.Q("RightButton") as Button;
        button4.RegisterCallback<ClickEvent>(OnRightClick);

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
        button1.UnregisterCallback<ClickEvent>(OnCancelClick);
        button2.UnregisterCallback<ClickEvent>(OnDeleteClick);
        button3.UnregisterCallback<ClickEvent>(OnLeftClick);
        button4.UnregisterCallback<ClickEvent>(OnRightClick);

        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnCancelClick(ClickEvent evt)
    {
        Debug.Log("You pressed Cancel Button");
        SceneManager.LoadScene("GardenSceneJia");
    }

    private void OnDeleteClick(ClickEvent evt)
    {
        Debug.Log("You pressed Delete Button");

        screenshots.ClearPictures();
        SceneManager.LoadScene("GardenSceneJia");
    }

    private void OnLeftClick(ClickEvent evt)
    {
        Debug.Log("You pressed Left Button");

        setScreenshot(screenshots.PreviousPicture());
    }

    private void OnRightClick(ClickEvent evt)
    {
        Debug.Log("You pressed Right Button");

        setScreenshot(screenshots.NextPicture());
    }
    
    private void OnAllButtonsClick(ClickEvent evt)
    {
        audioSource.Play();
    }
}
