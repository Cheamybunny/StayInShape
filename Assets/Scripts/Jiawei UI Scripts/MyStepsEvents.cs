using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MyStepsEvents : MonoBehaviour
{
    private UIDocument document;
    private StepCounter counter;
    private Button button1;
    private ScrollView scrollView1;
    private Label countdown;
    private Label stepCount;

    private List<Button> menuButtons = new List<Button>();

    private AudioSource audioSource;

    [SerializeField]
    PlayerDataSO playerDataSO;

    [SerializeField]
    private List<Sprite> collectedSprites;
    private List<Sprite> uncollectedSprites;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        document = GetComponent<UIDocument>();
        counter = GetComponent<StepCounter>();

        button1 = document.rootVisualElement.Q("BackButton") as Button;
        button1.RegisterCallback<ClickEvent>(OnBackClick);

        scrollView1 = document.rootVisualElement.Q("Goals") as ScrollView;

        countdown = document.rootVisualElement.Q("CountdownLabel") as Label;
        stepCount = document.rootVisualElement.Q("StepCount") as Label;

        menuButtons = document.rootVisualElement.Query<Button>().ToList();

        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
            
        }

        int steps = counter.GetStepCount();
        stepCount.text = $"Total steps: {steps}";
    }
    
    private void OnDisable()
    {
        button1.UnregisterCallback<ClickEvent>(OnBackClick);

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
    }

    public void SetStepUICount(int newSteps)
    {
        stepCount.text = $"Total steps: {newSteps}";
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

    private void OnAllButtonsClick(ClickEvent evt)
    {
        audioSource.Play();
    }
}
