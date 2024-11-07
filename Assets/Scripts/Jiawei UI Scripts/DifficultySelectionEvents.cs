using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

public class DifficultySelectionEvents : MonoBehaviour
{
    private UIDocument document;

    private Button button1;
    private Button button2;

    private VisualElement easy;
    private VisualElement medium;
    private VisualElement difficult;

    private bool easySelected = false;
    private bool mediumSelected = false;
    private bool difficultSelected = false;

    [SerializeField]
    private Sprite[] easySprites;
    [SerializeField]
    private Sprite[] mediumSprites;
    [SerializeField]
    private Sprite[] difficultSprites;

    private List<Button> menuButtons = new List<Button>();

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        document = GetComponent<UIDocument>();

        button1 = document.rootVisualElement.Q("BackButton") as Button;
        button1.RegisterCallback<ClickEvent>(OnBackClick);

        button2 = document.rootVisualElement.Q("StartButton") as Button;
        button2.RegisterCallback<ClickEvent>(OnStartClick);

        easy = document.rootVisualElement.Q("Easy") as VisualElement;
        easy.RegisterCallback<ClickEvent>(OnEasyClick);

        medium = document.rootVisualElement.Q("Medium") as VisualElement;
        medium.RegisterCallback<ClickEvent>(OnMediumClick);

        difficult = document.rootVisualElement.Q("Difficult") as VisualElement;
        difficult.RegisterCallback<ClickEvent>(OnDifficultClick);

        menuButtons = document.rootVisualElement.Query<Button>().ToList();

        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnDisable()
    {
        button1.UnregisterCallback<ClickEvent>(OnBackClick);
        button2.UnregisterCallback<ClickEvent>(OnStartClick);
        easy.UnregisterCallback<ClickEvent>(OnEasyClick);
        medium.UnregisterCallback<ClickEvent>(OnMediumClick);
        difficult.UnregisterCallback<ClickEvent>(OnDifficultClick);
        
        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void Update()
    {
        if (ResourceCollectionEvents.GameData.difficulty == -1) 
        {
            button2.style.display = DisplayStyle.None;
        }
        else
        {
            button2.style.display = DisplayStyle.Flex;
        }
    }

    private void OnBackClick(ClickEvent evt)
    {
        Debug.Log("You pressed the Back Button");

        ResourceCollectionEvents.GameData.difficulty = -1;
        SceneManager.LoadScene("ResourceCollectionSceneJia");
    }

    private void OnStartClick(ClickEvent evt)
    {
        Debug.Log("You pressed the Start Button");

        string gameSelection = ResourceCollectionEvents.GameData.selectedGame;
        SceneManager.LoadScene(gameSelection);
    }

    private void OnEasyClick(ClickEvent evt)
    {
        if (mediumSelected)
        {
            medium.style.backgroundImage = new StyleBackground(mediumSprites[0]);
            mediumSelected = false;
        }
        if (difficultSelected)
        {
            difficult.style.backgroundImage = new StyleBackground(difficultSprites[0]);
            difficultSelected = false;
        }

        if (!easySelected)
        {
            Debug.Log("Easy has been selected");

            ResourceCollectionEvents.GameData.difficulty = 0;
            easy.style.backgroundImage = new StyleBackground(easySprites[1]);
            easySelected = true;
        }
        else
        {
            Debug.Log("Easy has been deselected");

            ResourceCollectionEvents.GameData.difficulty = -1;
            easy.style.backgroundImage = new StyleBackground(easySprites[0]);
            easySelected = false;
        }
    }

    private void OnMediumClick(ClickEvent evt)
    {
        if (easySelected)
        {
            easy.style.backgroundImage = new StyleBackground(easySprites[0]);
            easySelected = false;
        }
        if (difficultSelected)
        {
            difficult.style.backgroundImage = new StyleBackground(difficultSprites[0]);
            difficultSelected = false;
        }

        if (!mediumSelected)
        {
            Debug.Log("Medium has been selected");

            ResourceCollectionEvents.GameData.difficulty = 1;
            medium.style.backgroundImage = new StyleBackground(mediumSprites[1]);
            mediumSelected = true;
        }
        else
        {
            Debug.Log("Medium has been deselected");

            ResourceCollectionEvents.GameData.difficulty = -1;
            medium.style.backgroundImage = new StyleBackground(mediumSprites[0]);
            mediumSelected = false;
        }
    }

    private void OnDifficultClick(ClickEvent evt)
    {
        if (easySelected)
        {
            easy.style.backgroundImage = new StyleBackground(easySprites[0]);
            easySelected = false;
        }
        if (mediumSelected)
        {
            medium.style.backgroundImage = new StyleBackground(mediumSprites[0]);
            mediumSelected = false;
        }

        if (!difficultSelected)
        {
            Debug.Log("Difficult has been selected");

            ResourceCollectionEvents.GameData.difficulty = 2;
            difficult.style.backgroundImage = new StyleBackground(difficultSprites[1]);
            difficultSelected = true;
        }
        else
        {
            Debug.Log("Medium has been deselected");

            ResourceCollectionEvents.GameData.difficulty = -1;
            difficult.style.backgroundImage = new StyleBackground(difficultSprites[0]);
            difficultSelected = false;
        }
    }

    private void OnAllButtonsClick(ClickEvent evt)
    {
        audioSource.Play();
    }
}
