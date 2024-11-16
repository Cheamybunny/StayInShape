using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class MatchingCardManager : MonoBehaviour
{
    [SerializeField] AudioClip selectClip;
    [SerializeField] AudioClip matchCorrectClip;
    [SerializeField] AudioClip matchWrongClip;
    [SerializeField] AudioClip winGameClip;
    [SerializeField] CardDB db;
    [SerializeField] PlayerDataSO player;
    [SerializeField] SaveManagerSO saveManager;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] ARTrackedImageManager trackedImageManager;
    [SerializeField] SceneChanger sceneChanger;
    [SerializeField] GameObject particlePrefab;

    // Jiawei UI stuff
    private UIDocument document;
    private Button button1;
    private Button button2;
    private Button button3;
    private VisualElement L;
    private VisualElement qr_popUp;
    private VisualElement popup;
    private Label popupLabel;
    private bool isActive = true;
    private bool isMessageActive = false;

    public int timeToDisplayText = 3;
    public int intervalToPlayGame = 1;
    public int reward = 0;
    public int heightOfCards = 4;
    public int spawnRange = 5;
    private CardLogic selectedCard;
    private int nCardsLeft;
    private int gameLevel;
    private AudioSource audioSource;
    private Transform parentTransform;
    private MatchingCardsPrefab gamePrefab;
    private int[] nCards = new int[] { 10, 14, 18 };

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        document = GetComponent<UIDocument>();

        button1 = document.rootVisualElement.Q("ExitButton") as Button;
        button1.RegisterCallback<ClickEvent>(OnExitClick);

        button2 = document.rootVisualElement.Q("CancelButton") as Button;
        button2.RegisterCallback<ClickEvent>(OnCancelClick);

        button3 = document.rootVisualElement.Q("StartButton") as Button;
        button3.RegisterCallback<ClickEvent>(OnStartClick);
        button3.style.display = DisplayStyle.None;

        L = document.rootVisualElement.Q("L") as VisualElement;
        L.style.display = DisplayStyle.None;

        qr_popUp = document.rootVisualElement.Q("QRPopUp") as VisualElement;
        qr_popUp.RegisterCallback<ClickEvent>(OnQRPopUpClick);
        StartCoroutine(HidePopUpAfterDelay(5f));

        popup = document.rootVisualElement.Q("Popup") as VisualElement;
        popupLabel = document.rootVisualElement.Q("PopLabel") as Label;
        popup.style.display = DisplayStyle.None;
        popup.RegisterCallback<ClickEvent>(OnPopupClick);
        gameLevel = ResourceCollectionEvents.GameData.difficulty;
    }

    private void RewardPlayer()
    {
        reward = reward * (gameLevel + 1);
        PlaySound(winGameClip);
        player.SetFertilizer(player.GetFertilizer() + reward);
        player.SetWater(player.GetWater() + reward);
        EndGameEvents.Rewards.waterReward = reward;
        EndGameEvents.Rewards.fertReward = reward;
        saveManager.Save();
        SceneManager.LoadScene("EndGameScene");
    }

    private void AddReward(int add, string plantName) // TODO: Find a way to utilise reward
    {
        reward += add;
        // DisplayText(String.Format("You have matched a pair of {0} cards!", plantName));
    }

    private void CompleteGame()
    {
        player.SetMatchingCardTimer(DateTime.Now.AddMinutes(intervalToPlayGame));
        // instructions.text = "You have won the game. Tap on the back button to go to the home screen.\n Next time to play is " + player.GetMatchingCardTimer();

        RewardPlayer();
    }

    private void SpawnCards()
    {
        // Spawn cards first
        for (int i = 0; i < nCards[gameLevel]; i++)
        {
            GameObject instance = Instantiate(cardPrefab, Vector3.zero, transform.rotation);
            instance.gameObject.name = "Card " + i.ToString();
            instance.transform.SetParent(parentTransform);
            instance.transform.rotation = parentTransform.rotation;
        }
    }

    public void SetupGame(Transform pTransform, MatchingCardsPrefab callee)
    {
        gamePrefab = callee;

        L.style.display = DisplayStyle.Flex;
        if (isActive) {
            qr_popUp.style.display = DisplayStyle.None;
            isActive = false;
        }
        button3.style.display = DisplayStyle.Flex;
        // instructions.text = "Don't see anything? Try moving closer or further to the QR code.";
        // status.text = "To properly spawn AR, move your phone so that the blue and red lines fit inside the L on your screen.\nOnce ready, press START GAME.";
        reward = 0;
        nCardsLeft = nCards[gameLevel];
        parentTransform = pTransform;
    }

    public void StartGame()
    {
        PopUpSpawn("Look up and match all the cards in front of you. Good Luck!");
        L.style.display = DisplayStyle.None;
        button3.style.display = DisplayStyle.None;
        // status.text = "Look up, the cards are in front of you. Good Luck!";
        // instructions.text = "Match every card to another similar card!\r\nYou can select a card by tapping on them!";
        trackedImageManager.enabled = false;
        SpawnCards();
        Transform[] myCards = GetCards(parentTransform, nCards[gameLevel]);
        RandomiseCards(myCards, nCards[gameLevel]);
        ArrangeCards(myCards, spawnRange, nCards[gameLevel]);
        gamePrefab.StartGame();
    }

    private void RandomiseCards(Transform[] cards, int nCards)
    {
        if (nCards % 2 != 0)
        {
            throw new System.Exception("Number of cards cannot be an odd number!");
        }

        MatchingCardSO[] list = db.getRandomCardList();

        for (int i = 0; i < nCards/2; i++)
        {
            MatchingCardSO cardData = list[i];
            CardLogic cardA = cards[i].gameObject.GetComponent<CardLogic>();
            CardLogic cardB = cards[nCards - 1 - i].gameObject.GetComponent<CardLogic>();
            cardA.SetCard(cardData, true);
            cardB.SetCard(cardData, false);
        }
    }

    private Transform[] GetCards(Transform parentTransform, int nCards)
    {
        Transform[] cards = new Transform[nCards];
        int i = 0;
        foreach (Transform child in parentTransform)
        {
            if (child.TryGetComponent<CardLogic>(out CardLogic _))
            {
                cards[i] = child;
                i++;
            }
        }
        
        return cards;
    }

    private Vector3[] CalculateSpawnPositions(int nCards, int spawnRange)
    {
        Vector3[] spawnPositions = new Vector3[nCards];
        int z_offset = 3;
        float angleStep = 360f / nCards;
        float x, z;
        int numberOfElementsPerRow = nCards / 2;
        float unitSpacing = 2f;
        float totalWidth = (numberOfElementsPerRow - 1) * unitSpacing;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < nCards; i++)
        {
            float angleDegrees = i * angleStep;
            float angleRadians = angleDegrees * Mathf.Deg2Rad;
            Vector3 newSpawnPosition;
            if (i % 2 == 0)
            {
                x = Mathf.Cos(angleRadians) * spawnRange;
                newSpawnPosition = new Vector3(startX + (i/2) * unitSpacing, heightOfCards, z_offset);
            } else
            {
                x = Mathf.Cos(angleRadians) * spawnRange;
                newSpawnPosition = new Vector3(startX + ((i-1) / 2) * unitSpacing, heightOfCards * 1.5f, z_offset);
            }
            spawnPositions[i] = newSpawnPosition;
        }

        return spawnPositions;
    }

    private void shuffleCards(Transform[] cards)
    {
        System.Random random = new System.Random();
        int n = cards.Length;
        for (int i = n - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);  // Pick a random index from 0 to i
                                            // Swap array[i] with array[j]
            Transform temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }
    }
    private void PositionCards(Transform[] cards, Vector3[] spawnPositions)
    {
        float yRotation = 90; // Trial and error Tested value
        float rotateStep = 0; // 360 / cards.Length;
        shuffleCards(cards);

        int i = 0;
        foreach (Transform card in cards)
        {
            card.localPosition = spawnPositions[i];
            card.transform.Rotate(0, 0, 0);
            i += 1;
        }
    }

    private void ArrangeCards(Transform[] myCards, int spawnRange, int nCards)
    {
        Vector3[] spawnPositions = CalculateSpawnPositions(nCards, spawnRange);
        PositionCards(myCards, spawnPositions);
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }


    private void MatchCards(CardLogic card1, CardLogic card2)
    {
        PopUpSpawn($"You matched the {card1.plantName} cards!");
        Unselect();
        SpawnParticles(card1.gameObject.transform.position);
        SpawnParticles(card2.gameObject.transform.position);
        Destroy(card1.gameObject);
        Destroy(card2.gameObject);
        PlaySound(matchCorrectClip);
        nCardsLeft -= 2;
        AddReward(1, card1.plantName);
        if (nCardsLeft == 0)
        {
            CompleteGame();
        }
    }

    private void SpawnParticles(Vector3 pos)
    {
        // Instantiate the particle system at the position
        GameObject particleEffect = Instantiate(particlePrefab, pos, Quaternion.identity);

        // Optionally, destroy the particle effect after 5 seconds
        Destroy(particleEffect, 3f);
    }

    public void SelectCard(CardLogic card)
    {
        if (selectedCard != null && selectedCard != card)
        {
            if (selectedCard.IsMatching(card)) {
                Debug.Log("IsMatching found!");
                MatchCards(selectedCard, card);
            } else
            {
                PlaySound(matchWrongClip); // Can play this if we can overcome infinite looping
                Debug.Log("Unable to match!");
            }
        }
        else if (selectedCard == null || card != selectedCard)
        {
            selectedCard = card;
            selectedCard.Select();
            PlaySound(selectClip);
        }
    }

    public void Unselect()
    {
        if (selectedCard)
        {
            selectedCard.Deselect();
            selectedCard = null;
            PlaySound(selectClip);
        }
    }

    private void OnExitClick(ClickEvent evt)
    {
        Debug.Log("You pressed Exit Button");

        sceneChanger.GoToScene("ResourceCollectionSceneJia");
    }

    private void OnCancelClick(ClickEvent evt)
    {
        Debug.Log("You pressed Cancel Button");

        Unselect();
    }

    private void OnStartClick(ClickEvent evt)
    {
        Debug.Log("You pressed Start Button");

        StartGame();
    }

    // Message Popup
    private void PopUpSpawn(String message)
    {
        popupLabel.text = message;
        popup.style.display = DisplayStyle.Flex;
        isMessageActive = true;
    }

    private void OnPopupClick(ClickEvent evt)
    {
        if (isMessageActive)
        {
            popup.style.display = DisplayStyle.None;
            isMessageActive = false;
        }
    }

    // QR Popup

    private void OnQRPopUpClick(ClickEvent evt)
    {
        if (isActive)
        {
            qr_popUp.style.display = DisplayStyle.None;
            isActive = false;
        }
    }

    private IEnumerator HidePopUpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (isActive)
        {
            qr_popUp.style.display = DisplayStyle.None;
            isActive = false;
        }
    }
}
