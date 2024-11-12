using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MyStepsEvents : MonoBehaviour
{
    private UIDocument document;

    private Button button1;
    private ScrollView scrollView1;
    private Label countdown;
    private Label stepCount;
    private VisualElement popup;
    private Label popupLabel;
    private bool isActive = false;

    private List<Button> menuButtons = new List<Button>();

    private AudioSource audioSource;

    [SerializeField]
    PlayerDataSO playerDataSO;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        document = GetComponent<UIDocument>();

        button1 = document.rootVisualElement.Q("BackButton") as Button;
        button1.RegisterCallback<ClickEvent>(OnBackClick);

        scrollView1 = document.rootVisualElement.Q("Goals") as ScrollView;

        countdown = document.rootVisualElement.Q("CountdownLabel") as Label;
        stepCount = document.rootVisualElement.Q("StepCount") as Label;

        popup = document.rootVisualElement.Q("Popup") as VisualElement;
        popupLabel = document.rootVisualElement.Q("PopLabel") as Label;
        popup.style.display = DisplayStyle.None;
        popup.RegisterCallback<ClickEvent>(OnPopUpClick);

        menuButtons = document.rootVisualElement.Query<Button>().ToList();

        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }
    
    private void OnDisable()
    {
        button1.UnregisterCallback<ClickEvent>(OnBackClick);
        popup.UnregisterCallback<ClickEvent>(OnPopUpClick);

        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void Update()
    {
        int daysRemaining = GetDaysUntilNextMonth();
        string dayText = daysRemaining == 1 ? "day" : "days";
        countdown.text = $"{daysRemaining} {dayText} until reset";

        int steps = playerDataSO.GetSteps();
        stepCount.text = $"Total steps: {steps}";
    }

    private int GetDaysUntilNextMonth()
    {
        DateTime now = DateTime.Now;
        DateTime nextMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1);
        return (nextMonth - now).Days;
    }

    private void OnBackClick(ClickEvent evt)
    {
        Debug.Log("You pressed the Back Button");

        SceneManager.LoadScene("GardenSceneJia");
    }

    private void OnPopUpClick(ClickEvent evt)
    {
        if (isActive)
        {
            popup.style.display = DisplayStyle.None;
            isActive = false;
        }
    }

    private void PopUpSpawn(String message)
    {
        popupLabel.text = message;
        popup.style.display = DisplayStyle.Flex;
        isActive = true;
    }

    private void OnAllButtonsClick(ClickEvent evt)
    {
        audioSource.Play();
    }
}
