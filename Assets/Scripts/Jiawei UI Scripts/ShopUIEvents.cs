using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using System.Xml.Serialization;

public class ShopUIEvents : MonoBehaviour
{
    private UIDocument _document;

    private Button _button1;
    private Button _button2;
    private Button _button3;
    private Button _button4;
    private Button _button5;
    private Button _button6;
    private Button _button7;
    private Button closeErrorButton;

    //decorations buttons
    private Button flower1Button;
    private Button flower2Button;
    private Button sunflowerButton;
    private Button chickenButton;
    private Button radioButton;

    private Label errorMessage;
    private Label chillistock;
    private Label sweetpotatostock;
    private Label eggplantstock;
    private Label calamansistock;
    private Label loofastock;
    private Label papayastock;

    private List<Button> _menuButtons = new List<Button>();

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _document = GetComponent<UIDocument>();


        _button1 = _document.rootVisualElement.Q("BackButton") as Button;
        _button1.RegisterCallback<ClickEvent>(OnBackButtonClick);

        _button2 = _document.rootVisualElement.Q("ChilliButton") as Button;
        _button2.RegisterCallback<ClickEvent>(OnBuyChilli);

        _button3 = _document.rootVisualElement.Q("EggplantButton") as Button;
        _button3.RegisterCallback<ClickEvent>(OnBuyEggplant);

        _button4 = _document.rootVisualElement.Q("LoofaButton") as Button;
        _button4.RegisterCallback<ClickEvent>(OnBuyLoofa);

        _button5 = _document.rootVisualElement.Q("SweetPotatoButton") as Button;
        _button5.RegisterCallback<ClickEvent>(OnBuySweetPotato);

        _button6 = _document.rootVisualElement.Q("CalamansiButton") as Button;
        _button6.RegisterCallback<ClickEvent>(OnBuyCalamansi);

        _button7 = _document.rootVisualElement.Q("PapayaButton") as Button;
        _button7.RegisterCallback<ClickEvent>(OnBuyPapaya);

        flower1Button = _document.rootVisualElement.Q("Flower1Button") as Button;
        flower1Button.RegisterCallback<ClickEvent>(OnBuyFlower1);

        flower2Button = _document.rootVisualElement.Q("Flower2Button") as Button;
        flower2Button.RegisterCallback<ClickEvent>(OnBuyFlower2);

        sunflowerButton = _document.rootVisualElement.Q("SunFlowerButton") as Button;
        sunflowerButton.RegisterCallback<ClickEvent>(OnBuySunflower);

        chickenButton = _document.rootVisualElement.Q("ChickenButton") as Button;
        chickenButton.RegisterCallback<ClickEvent>(OnBuyChicken);

        radioButton = _document.rootVisualElement.Q("RadioButton") as Button;
        radioButton.RegisterCallback<ClickEvent>(OnBuyRadio);

        closeErrorButton = _document.rootVisualElement.Q("closeError") as Button;
        closeErrorButton.RegisterCallback<ClickEvent>(OnCloseErrorClick);
        closeErrorButton.style.display = DisplayStyle.None;

        errorMessage = _document.rootVisualElement.Q("errorpopup") as Label;
        errorMessage.style.display = DisplayStyle.None;

        chillistock = _document.rootVisualElement.Q("chillistock") as Label;
        sweetpotatostock = _document.rootVisualElement.Q("sweetpotatostock") as Label;
        eggplantstock = _document.rootVisualElement.Q("eggplantstock") as Label;
        calamansistock = _document.rootVisualElement.Q("calamansistock") as Label;
        loofastock = _document.rootVisualElement.Q("loofastock") as Label;
        papayastock = _document.rootVisualElement.Q("papayastock") as Label;

        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();

        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnDisable()
    {
        _button1.UnregisterCallback<ClickEvent>(OnBackButtonClick);
        flower1Button.UnregisterCallback<ClickEvent>(OnBuyFlower1);
        flower2Button.UnregisterCallback<ClickEvent>(OnBuyFlower2);
        sunflowerButton.UnregisterCallback<ClickEvent>(OnBuySunflower);
        chickenButton.UnregisterCallback<ClickEvent>(OnBuyChicken);
        radioButton.UnregisterCallback<ClickEvent>(OnBuyRadio);
        closeErrorButton.UnregisterCallback<ClickEvent>(OnCloseErrorClick);

        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnBuyFlower1(ClickEvent evt)
    {
        bool canBuyFlower1 = ShopManager.instance.CanBuyFlower1();
        if (canBuyFlower1)
        {
            SceneManager.LoadScene("GardenSceneJia");
        }
    }
    private void OnBuyFlower2(ClickEvent evt)
    {
        bool canBuyFlower2 = ShopManager.instance.CanBuyFlower2();
        if (canBuyFlower2)
        {
            SceneManager.LoadScene("GardenSceneJia");
        }
    }

    private void OnBuySunflower(ClickEvent evt)
    {
        bool canBuySunflower = ShopManager.instance.CanBuySunflower();
        if (canBuySunflower)
        {
            SceneManager.LoadScene("GardenSceneJia");
        }
    }

    private void OnBuyChicken(ClickEvent evt)
    {
        bool canBuyChicken = ShopManager.instance.CanBuyChicken();
        if (canBuyChicken)
        {
            SceneManager.LoadScene("GardenSceneJia");
        }
    }

    private void OnBuyRadio(ClickEvent evt)
    {
        bool canBuyRadio = ShopManager.instance.CanBuyRadio();
        if (canBuyRadio)
        {
            SceneManager.LoadScene("GardenSceneJia");
        }
    }

    private void OnCloseErrorClick(ClickEvent evt)
    {
        closeErrorButton.style.display = DisplayStyle.None;
        errorMessage.style.display = DisplayStyle.None;

    }
    public void ThrowError(string errorMessageText)
    {
        closeErrorButton.style.display = DisplayStyle.Flex;
        errorMessage.text = errorMessageText;
        errorMessage.style.display = DisplayStyle.Flex;
    }
    private void OnBackButtonClick(ClickEvent evt)
    {
        Debug.Log("You pressed Back Button");

        SceneManager.LoadScene("GardenSceneJia");
    }

    private void OnBuyChilli(ClickEvent evt)
    {
        Debug.Log("Buying Chilli");
        ShopManager.instance.AddChilli();

    }

    private void OnBuyEggplant(ClickEvent evt)
    {
        Debug.Log("Buying Eggplant");
        ShopManager.instance.AddEggplant();
    }

    private void OnBuyLoofa(ClickEvent evt)
    {
        Debug.Log("Buying Loofa");
        ShopManager.instance.AddLoofa();
    }

    private void OnBuySweetPotato(ClickEvent evt)
    {
        Debug.Log("Buying sweet potato");
        ShopManager.instance.AddSweetPotato();
    }

    private void OnBuyCalamansi(ClickEvent evt)
    {
        Debug.Log("Buying calamansi");
        ShopManager.instance.AddKalamansi();
    }

    private void OnBuyPapaya(ClickEvent evt)
    {
        Debug.Log("Buying papaya");
        ShopManager.instance.AddPapaya();
    }

    private void OnAllButtonsClick(ClickEvent evt)
    {
        _audioSource.Play();
    }

    public void SetChilliStockValue(int value)
    {
        chillistock.text = value.ToString();
    }
    public void SetEggplantStockValue(int value)
    {
        eggplantstock.text = value.ToString();
    }
    public void SetLoofaStockValue(int value)
    {
        loofastock.text = value.ToString();
    }
    public void SetSweetPotatoStockValue(int value)
    {
        sweetpotatostock.text = value.ToString();
    }
    public void SetPapayaValue(int value)
    {
        papayastock.text = value.ToString();
    }
    public void SetKalamansiStockValue(int value)
    {
        calamansistock.text = value.ToString();
    }
}
