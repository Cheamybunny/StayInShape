using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

public class EndGameEvents : MonoBehaviour
{
    private UIDocument document;

    private Button button1;
    private Label fertLabel;
    private Label waterLabel;

    private AudioSource audioSource;

    public static class Rewards
    {
        public static int fertReward = 0;
        public static int waterReward = 0;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        document = GetComponent<UIDocument>();

        button1 = document.rootVisualElement.Q("FinishButton") as Button;
        button1.RegisterCallback<ClickEvent>(OnFinishClick);

        fertLabel = document.rootVisualElement.Q("FertLabel") as Label;
        fertLabel.text = Rewards.fertReward.ToString();

        waterLabel = document.rootVisualElement.Q("WaterLabel") as Label;
        waterLabel.text = Rewards.waterReward.ToString();
    }

    private void OnDisable()
    {
        button1.UnregisterCallback<ClickEvent>(OnFinishClick);
    }

    private void OnFinishClick(ClickEvent evt)
    {
        Debug.Log("You have pressed Finish button");

        SceneManager.LoadScene("ResourceCollectionSceneJia");
    }
}