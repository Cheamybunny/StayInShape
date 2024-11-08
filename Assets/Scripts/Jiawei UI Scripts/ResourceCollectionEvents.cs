using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

public class ResourceCollectionEvents : MonoBehaviour
{
    private UIDocument document;

    private Button button1;
    private Button button2;
    private Button button3;
    private Button button4;
    private Label snapTimer;
    private Label matchTimer;
    private Label chickenTimer;

    [SerializeField]
    private PlayerDataSO playerDataSO;
    [SerializeField]
    private Sprite[] buttonSprites; // 0 green, 1 red

    private List<Button> menuButtons = new List<Button>();

    private AudioSource audioSource;

    public static class GameData
    {
        public static string selectedGame;
        public static int difficulty = -1;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        document = GetComponent<UIDocument>();

        button1 = document.rootVisualElement.Q("HomeButton") as Button;
        button1.RegisterCallback<ClickEvent>(OnHomeClick);

        button2 = document.rootVisualElement.Q("SnapPlay") as Button;
        button2.RegisterCallback<ClickEvent>(OnSnapPlayClick);
        snapTimer = document.rootVisualElement.Q("SnapTimer") as Label;
        HandleSnapButton();

        button3 = document.rootVisualElement.Q("MatchingPlay") as Button;
        button3.RegisterCallback<ClickEvent>(OnMatchingPlayClick);
        matchTimer = document.rootVisualElement.Q("MatchTimer") as Label;
        HandleMatchButton();

        button4 = document.rootVisualElement.Q("ChickenPlay") as Button;
        button4.RegisterCallback<ClickEvent>(OnChickenPlayClick);
        chickenTimer = document.rootVisualElement.Q("ChickenTimer") as Label;
        HandleChickenButton();

        menuButtons = document.rootVisualElement.Query<Button>().ToList();

        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnDisable()
    {
        button1.UnregisterCallback<ClickEvent>(OnHomeClick);
        button2.UnregisterCallback<ClickEvent>(OnSnapPlayClick);
        button3.UnregisterCallback<ClickEvent>(OnMatchingPlayClick);
        button4.UnregisterCallback<ClickEvent>(OnChickenPlayClick);
        
        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void Update()
    {
        HandleSnapButton();
        HandleMatchButton();
        HandleChickenButton();
    }

    private void HandleSnapButton()
    {
        if (playerDataSO.CanPlaySnap())
        {
            snapTimer.style.display = DisplayStyle.None;
            button2.style.backgroundImage = new StyleBackground(buttonSprites[0]);
        }
        else
        {
            snapTimer.style.display = DisplayStyle.Flex;
            snapTimer.text = playerDataSO.GetSnapTimer().ToString();
            button2.style.backgroundImage = new StyleBackground(buttonSprites[1]);
        }
    }

    private void HandleMatchButton()
    {
        if (playerDataSO.CanPlayMatchingCard())
        {
            matchTimer.style.display = DisplayStyle.None;
            button3.style.backgroundImage = new StyleBackground(buttonSprites[0]);
        }
        else
        {
            matchTimer.style.display = DisplayStyle.Flex;
            matchTimer.text = playerDataSO.GetMatchingCardTimer().ToString();
            button3.style.backgroundImage = new StyleBackground(buttonSprites[1]);
        }
    }

    private void HandleChickenButton()
    {
        if (playerDataSO.CanPlayChickenInvaders())
        {
            chickenTimer.style.display = DisplayStyle.None;
            button4.style.backgroundImage = new StyleBackground(buttonSprites[0]);
        }
        else
        {
            chickenTimer.style.display = DisplayStyle.Flex;
            chickenTimer.text = playerDataSO.GetChickenInvaderTimer().ToString();
            button4.style.backgroundImage = new StyleBackground(buttonSprites[1]);
        }
    }

    private void OnHomeClick(ClickEvent evt)
    {
        Debug.Log("You pressed the Home Button");

        SceneManager.LoadScene("GardenSceneJia");
    }

    private void OnSnapPlayClick(ClickEvent evt)
    {
        Debug.Log("You pressed the Snap Play Button");
        
        if (!playerDataSO.CanPlaySnap()) return;
        GameData.selectedGame = "SnapSceneJia";
        SceneManager.LoadScene("DifficultySelectionScene");
    }

    private void OnMatchingPlayClick(ClickEvent evt)
    {
        Debug.Log("You pressed the Matching Play Button");

        if (!playerDataSO.CanPlayMatchingCard()) return;
        GameData.selectedGame = "MatchingCardScene";
        SceneManager.LoadScene("DifficultySelectionScene");
    }

    private void OnChickenPlayClick(ClickEvent evt)
    {
        Debug.Log("You pressed the Chicken Play Button");

        if (!playerDataSO.CanPlayChickenInvaders()) return;
        GameData.selectedGame = "ChickenInvaderScene";
        SceneManager.LoadScene("DifficultySelectionScene");
    }

    private void OnAllButtonsClick(ClickEvent evt)
    {
        audioSource.Play();
    }
}
