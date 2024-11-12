using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Reward
{
    public int stepsNeeded;
    public int waterReward;
    public int fertilizerReward;

    public Reward(int steps, int water, int fert)
    {
        stepsNeeded = steps;
        waterReward = water;
        fertilizerReward = fert;
    }
}

public class MyStepsEvents : MonoBehaviour
{
    private UIDocument document;
    private StepCounter counter;
    private Button button1;
    private ScrollView scrollView1;
    private Label countdown;
    private Label stepCount;

    private List<Button> menuButtons = new List<Button>();
    private List<Button> goalButtons = new List<Button>();

    private AudioSource audioSource;

    [SerializeField]
    PlayerDataSO playerDataSO; // Should not use to get steps
    [SerializeField]
    SaveManagerSO saveManagerSO;

    [SerializeField]
    private List<Sprite> collectedSprites;
    [SerializeField]
    private List<Sprite> uncollectedSprites;

    private List<Reward> goals = new List<Reward>();

    private void InstantiateGoals() // Bad practice: All these are hardcoded as the images are hardcoded too
    {
        goals.Add(new Reward(steps: 8000, water: 2, fert: 2)); // 1
        goals.Add(new Reward(steps: 18000, water: 3, fert: 0)); // 2
        goals.Add(new Reward(steps: 24000, water: 2, fert: 2)); // 3
        goals.Add(new Reward(steps: 32000, water: 0, fert: 3)); // 4
        goals.Add(new Reward(steps: 40000, water: 4, fert: 4)); // 5
        goals.Add(new Reward(steps: 48000, water: 4, fert: 4)); // 6
        goals.Add(new Reward(steps: 56000, water: 6, fert: 0)); // 7
        goals.Add(new Reward(steps: 64000, water: 6, fert: 6)); // 8
        goals.Add(new Reward(steps: 72000, water: 0, fert: 6)); // 9
        goals.Add(new Reward(steps: 80000, water: 6, fert: 6)); // 10
        goals.Add(new Reward(steps: 88000, water: 8, fert: 8)); // 11
        goals.Add(new Reward(steps: 96000, water: 9, fert: 0)); // 12
        goals.Add(new Reward(steps: 104000, water: 8, fert: 8)); // 13
        goals.Add(new Reward(steps: 112000, water: 0, fert: 9)); // 14
        goals.Add(new Reward(steps: 124000, water: 10, fert: 10)); // 15
    }

    private void PurchaseReward(int index)
    {
        Reward chosenReward = goals[index];
        // Check if player can purchase reward
        if (!playerDataSO.GetHasCollectedRewardStatus(index))
        {
            if (counter.GetStepCount() >= chosenReward.stepsNeeded)
            {
                string popupMessage = $"You have earned {chosenReward.waterReward} bag(s) of water and {chosenReward.fertilizerReward} bag(s) of fertilizer";
                // Popup(popupMessage);
                // Update playerDataSO
                playerDataSO.SetFertilizer(playerDataSO.GetFertilizer() + chosenReward.fertilizerReward);
                playerDataSO.SetWater(playerDataSO.GetWater() + chosenReward.waterReward);
                playerDataSO.SetHasCollectedRewardStatus(index, true);
                saveManagerSO.Save();

                makeButtonCollected(index);
            }
        }
    }

    private void makeButtonCollected(int index)
    {
        // Change the image
        var texture = collectedSprites[index].texture;
        goalButtons[index].style.backgroundImage = new StyleBackground(texture);
    }

    private void Awake()
    {
        InstantiateGoals();
        if (DateTime.Now.Day == 1)
        {
            ResetAllRewards();
        }

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

        // Loop through the names "Goal1" to "Goal15" and find the corresponding buttons
        for (int i = 1; i <= 15; i++)
        {
            string buttonName = "Goal" + i; // Construct button name
            Button button = document.rootVisualElement.Q<Button>(buttonName);

            if (button != null)
            {
                int index = i - 1;
                goalButtons.Add(button); // Add the button to the list
                if (playerDataSO.GetHasCollectedRewardStatus(index))
                {
                    makeButtonCollected(index);
                } else
                {
                    button.clicked += () => PurchaseReward(index); // Example: adding a click event
                }
            }
            else
            {
                Debug.LogWarning("Button with name " + buttonName + " not found.");
            }
        }

        int steps = counter.GetStepCount();
        stepCount.text = $"Total steps: {steps}";

        int daysRemaining = GetDaysUntilNextMonth();
        string dayText = daysRemaining == 1 ? "day" : "days";
        countdown.text = $"{daysRemaining} {dayText} until reset";
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
    }

    private void ResetAllRewards()
    {
        Debug.Log("Resetting all rewards");
        for (int i = 0; i < playerDataSO.hasCollectedRewardStatus.Count; i++)
        {
            playerDataSO.SetHasCollectedRewardStatus(i, false);
        }
        saveManagerSO.Save();
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
