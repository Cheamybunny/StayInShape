using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GardenUIEvents : MonoBehaviour
{
    [SerializeField] SceneChanger sceneChanger;
    private UIDocument _document;

    private Button _button1;
    private Button _button2;
    private Button _button3;
    private Button _button4;
    private Button _button5;
    private Button closeErrorButton;

    public static Texture2D capturedScreenshot;

    private Label errorMessage2;
    private Label levelvalue;
    private Label watervalue;
    private Label fertiliservalue;

    private VisualElement resourceTracker;
    private VisualElement pickedItem;
    public Sprite originalSprite;
    public Sprite newSprite;
    public Sprite magnifierSprite;
    public Sprite waterSprite;
    public Sprite fertilizerSprite;
    public Sprite trowelSprite;
    public Sprite chilliSprite;
    public Sprite eggplantSprite;
    public Sprite loofaSprite;
    public Sprite papayaSprite;
    public Sprite kalamansiSprite;
    public Sprite sweetPotatoSprite;
    public Sprite flowerSprite;
    public Sprite flower2Sprite;
    public Sprite sunflowerSprite;
    public Sprite chickenSprite;
    public Sprite radioSprite;
    private bool isOriginal = true;
    private int pickedItemType;

    private VisualElement popUp;
    private bool isActive = true;

    private List<Button> _menuButtons = new List<Button>();

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _document = GetComponent<UIDocument>();

        _button1 = _document.rootVisualElement.Q("StepsButton") as Button;
        _button1.RegisterCallback<ClickEvent>(OnStepsButtonClick);

        _button2 = _document.rootVisualElement.Q("TakePhotoButton") as Button;
        _button2.RegisterCallback<ClickEvent>(OnTakePhotoClick);
        
        _button3 = _document.rootVisualElement.Q("CareBookButton") as Button;
        _button3.RegisterCallback<ClickEvent>(OnCareBookClick);

        _button4 = _document.rootVisualElement.Q("ShopButton") as Button;
        _button4.RegisterCallback<ClickEvent>(OnShopClick);

        _button5 = _document.rootVisualElement.Q("GamesButton") as Button;
        _button5.RegisterCallback<ClickEvent>(OnGamesClick);

        closeErrorButton = _document.rootVisualElement.Q("CloseErrorMessage") as Button;
        closeErrorButton.RegisterCallback<ClickEvent>(OnCloseErrorClick);
        closeErrorButton.style.display = DisplayStyle.None;

        errorMessage2 = _document.rootVisualElement.Q("errorMessage") as Label;
        errorMessage2.style.display = DisplayStyle.None;
        levelvalue = _document.rootVisualElement.Q("levelvalue") as Label;
        watervalue = _document.rootVisualElement.Q("watervalue") as Label;
        fertiliservalue = _document.rootVisualElement.Q("fertiliservalue") as Label;


        pickedItem = _document.rootVisualElement.Q("PickedItem") as VisualElement;

        resourceTracker = _document.rootVisualElement.Q("ResourceTracker") as VisualElement;
        resourceTracker.RegisterCallback<ClickEvent>(OnResourceTrackerClick);

        popUp = _document.rootVisualElement.Q("PopUp") as VisualElement;
        popUp.RegisterCallback<ClickEvent>(OnPopUpClick);
        StartCoroutine(HidePopUpAfterDelay(3f));

        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();

        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnDisable()
    {
        _button1.UnregisterCallback<ClickEvent>(OnStepsButtonClick);
        _button2.UnregisterCallback<ClickEvent>(OnTakePhotoClick);
        _button3.UnregisterCallback<ClickEvent>(OnCareBookClick);
        _button4.UnregisterCallback<ClickEvent>(OnShopClick);
        closeErrorButton.UnregisterCallback<ClickEvent>(OnCloseErrorClick);
        resourceTracker.UnregisterCallback<ClickEvent>(OnResourceTrackerClick);
        popUp.UnregisterCallback<ClickEvent>(OnPopUpClick);

        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnCloseErrorClick(ClickEvent evt)
    {
        closeErrorButton.style.display = DisplayStyle.None;
        errorMessage2.style.display = DisplayStyle.None;

    }

    public void ThrowError(string errorMessageText)
    {
        closeErrorButton.style.display = DisplayStyle.Flex;
        errorMessage2.text = errorMessageText;
        errorMessage2.style.display = DisplayStyle.Flex;
    }

    private void OnStepsButtonClick(ClickEvent evt)
    {
        Debug.Log("You pressed Steps Button");

        sceneChanger.GoToScene("MyStepsScene");
    }

    public void UpdatePickedItem(int item)
    {
        pickedItemType = item;
        if (item == 0)
        {
            pickedItem.style.backgroundImage = new StyleBackground(originalSprite);
        }
        else if(item == -1)
        {
            pickedItem.style.backgroundImage = new StyleBackground(magnifierSprite);
        }
        else if (item == 1)
        {
            Debug.Log("ITS WATER CUHHH");
            pickedItem.style.backgroundImage = new StyleBackground(waterSprite);
        }
        else if (item == 2)
        {
            Debug.Log("FAERTILISERRR");
            pickedItem.style.backgroundImage = new StyleBackground(fertilizerSprite);
        }
        else if (item == 3)
        {
            Debug.Log("TROOOOWELLLL");
            pickedItem.style.backgroundImage = new StyleBackground(trowelSprite);
        }
        else if (item == 4)
        {
            Debug.Log("CHILLIIIIIIIII");
            pickedItem.style.backgroundImage = new StyleBackground(chilliSprite);
        }
        else if (item == 5)
        {
            Debug.Log("EGGPLANT");
            pickedItem.style.backgroundImage = new StyleBackground(eggplantSprite);
        }
        else if (item == 6)
        {
            Debug.Log("LOOOOOFAAAA");
            pickedItem.style.backgroundImage = new StyleBackground(loofaSprite);
        }
        else if (item == 7)
        {
            Debug.Log("SWEET POTATOOO TA TA TA");
            pickedItem.style.backgroundImage = new StyleBackground(sweetPotatoSprite);
        }
        else if (item == 8)
        {
            Debug.Log("PAPAPAPAPAYAYAYAYA");
            pickedItem.style.backgroundImage = new StyleBackground(papayaSprite);
        }
        else if (item == 9)
        {
            Debug.Log("CALAMANSHEEEEEESH");
            pickedItem.style.backgroundImage = new StyleBackground(kalamansiSprite);
        }
        else if (item == 10)
        {
            pickedItem.style.backgroundImage = new StyleBackground(flowerSprite);
        }
        else if (item == 11)
        {
            pickedItem.style.backgroundImage = new StyleBackground(flower2Sprite);
        }
        else if (item == 12)
        {
            pickedItem.style.backgroundImage = new StyleBackground(sunflowerSprite);
        }
        else if (item == 13)
        {
            pickedItem.style.backgroundImage = new StyleBackground(chickenSprite);
        }
        else if (item == 14)
        {
            pickedItem.style.backgroundImage = new StyleBackground(radioSprite);
        }
    }

    private void OnTakePhotoClick(ClickEvent evt)
    {
        Debug.Log("You pressed Take Photo Button");

        StartCoroutine(TakeScreenshot());
    }

    private IEnumerator TakeScreenshot()
    {
        string timeStamp = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        string fileName = "Screenshot" + timeStamp + ".png";
        string pathToSave;
        if (Application.platform != RuntimePlatform.Android)
        {
            pathToSave = Application.persistentDataPath + "/" + fileName;
            ScreenshotPreviewer.recentScreenshotPath = pathToSave;
        } else
        {
            pathToSave = fileName;
            ScreenshotPreviewer.recentScreenshotPath = Application.persistentDataPath + "/" + pathToSave;
        }
        ScreenCapture.CaptureScreenshot(pathToSave);
        Debug.Log("Screenshot saved to " + pathToSave);
        yield return new WaitForSeconds(1);
        // Load the scene where the screenshot will be displayed
        SceneManager.LoadScene("ScreenshotDisplayScene");
    }

    private void OnCareBookClick(ClickEvent evt)
    {
        Debug.Log("You pressed Care Book Button");

        sceneChanger.GoToScene("CareBookScene");
    }

    private void OnShopClick(ClickEvent evt)
    {
        Debug.Log("You pressed Shop Button");

        sceneChanger.GoToScene("ShopScene");
    }

    private void OnGamesClick(ClickEvent evt)
    {
        Debug.Log("You pressed Games Button");

        sceneChanger.GoToScene("ResourceCollectionSceneJia");
    }

    
    private void OnResourceTrackerClick(ClickEvent evt)
    {
        Debug.Log("You pressed Resource Tracker");
        /**
        if (isOriginal)
        {
            ChangeSprite(newSprite, 86f, 46f);
        } else {
            ChangeSprite(originalSprite, 30f, 18f);
        }

        isOriginal = !isOriginal;
        **/
        // sceneChanger.GoToScene("ResourceCollectionSceneJia");
    }
    

     private void ChangeSprite(Sprite sprite, float widthPercent, float heightPercent)
    {
        if (sprite != null)
        {
            resourceTracker.style.backgroundImage = new StyleBackground(sprite);

            resourceTracker.style.width = new Length(widthPercent, LengthUnit.Percent);;
            resourceTracker.style.height = new Length(heightPercent, LengthUnit.Percent);;
        }
    }

    private void OnPopUpClick(ClickEvent evt)
    {
        if (isActive)
        {
            popUp.style.display = DisplayStyle.None;
            isActive = false;
        }
    }

    private IEnumerator HidePopUpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (isActive)
        {
            popUp.style.display = DisplayStyle.None;
            isActive = false;
        }
    }
    
    public void setWaterText(int value)
    {
        watervalue.text = value.ToString();
    }

    public void setFertiliserText(int value)
    {
        fertiliservalue.text = value.ToString();
    }

    public void setCurrentLevel(int value)
    {
        levelvalue.text = value.ToString();
    }

    public int GetPickedItemType()
    {
        return pickedItemType;
    }
    private void OnAllButtonsClick(ClickEvent evt)
    {
        _audioSource.Play();
    }
}
