using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;

public class SnapGameEvents : MonoBehaviour
{
    private AudioSource audioSource;
    private UIDocument document;

    private Button button1;
    private Button button2;
    private Button button3;

    private VisualElement popUp;
    private bool isActive = true;
    private VisualElement playerCard;
    private VisualElement deckCard;
    private VisualElement[] plants;
    private Label nCorrect;
    private Label nWrong;
    private Label timer;

    private List<Button> menuButtons = new List<Button>();

    // Game Fields
    private int[] level = new int[] {35, 6}; 
    private float[] timePerSnap = new float[] {6, 4, 1};
    private int intervalToPlayGame = 10;
    private float timeLeft;
    private int currentLevelId;
    private int currentDeckCard;
    private bool deckDrawn;
    private bool lockout;
    private List<int> deckList;

    private int currentIndex;
    private int totalMoves;
    private int nCorrects;
    private int nWrongs;

    [SerializeField]
    Sprite[] cardSprites;
    [SerializeField]
    Sprite[] popupSprites; // 0 is instructions, 1 is incorrect, 2 is correct, 3 draw when not same, 4 buy when same, 5 is game end
    [SerializeField]
    PlayerDataSO playerDataSO;
    [SerializeField]
    SaveManagerSO saveManager;
   
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        document = GetComponent<UIDocument>();

        button1 = document.rootVisualElement.Q("ExitButton") as Button;
        button1.RegisterCallback<ClickEvent>(OnExitClick);

        button2 = document.rootVisualElement.Q("DrawButton") as Button;
        button2.RegisterCallback<ClickEvent>(OnDrawClick);

        button3 = document.rootVisualElement.Q("BuyButton") as Button;
        button3.RegisterCallback<ClickEvent>(OnBuyClick);

        popUp = document.rootVisualElement.Q("PopUp") as VisualElement;
        popUp.RegisterCallback<ClickEvent>(OnPopUpClick);
        StartCoroutine(HidePopUpAfterDelay(5f));

        playerCard = document.rootVisualElement.Q("Card1") as VisualElement;
        deckCard = document.rootVisualElement.Q("Card2") as VisualElement;

        plants = new VisualElement[6];
        for (int i = 0; i < 6; i++)
        {
            plants[i] = document.rootVisualElement.Q($"Plant{i + 1}") as VisualElement;
        }

        nCorrect = document.rootVisualElement.Q("nCorrect") as Label;
        nWrong = document.rootVisualElement.Q("nWrong") as Label;
        timer = document.rootVisualElement.Q("timer") as Label;

        menuButtons = document.rootVisualElement.Query<Button>().ToList();

        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }

        currentLevelId = ResourceCollectionEvents.GameData.difficulty;
        deckDrawn = false;
        deckList = new List<int>();
        lockout = false;
        SetupGame();
    }

    private void OnDisable()
    {
        button1.UnregisterCallback<ClickEvent>(OnExitClick);
        button2.UnregisterCallback<ClickEvent>(OnDrawClick);
        button3.UnregisterCallback<ClickEvent>(OnBuyClick);
        popUp.UnregisterCallback<ClickEvent>(OnPopUpClick);

        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void Update()
    {
        if (deckDrawn && !lockout)
        {
            subtractTimer(Time.deltaTime);
            if (timeLeft < 0)
            {
                incrementWrong();
            }
        }
    }

    private void SetupGame()
    {
        currentIndex = 0;
        totalMoves = 0;
        nCorrects = 0;
        nWrongs = 0;
        deckDrawn = false;
        deckList.Clear();
        while(deckList.Count < level[1]) // Fill up deck list with 6 cards (these cards are the matches)
        {
            int num = UnityEngine.Random.Range(0, level[0] + level[1] - 1); 
            if (deckList.Contains(num)) continue;
            deckList.Add(num); // This number determines how many cards we must flip to eventually "reach" the desired card
            Debug.Log("Deck List has " + num);
        }
        updateTextUI();
        spawnPopup(0);
    }

    private void setTimer(float val)
    {
        timeLeft = val;
        timer.text = timeLeft.ToString("F2");
    }

    private void subtractTimer(float val)
    {
        timeLeft -= val;
        timer.text = timeLeft.ToString("F2");
    }

    private void updateTextUI()
    {
        nCorrect.text = nCorrects.ToString();
        nWrong.text = nWrongs.ToString();
    }

    private void removePlant()
    {
        if (nCorrects <= plants.Length)
        {
            plants[nCorrects - 1].style.backgroundImage = new StyleBackground((Sprite)null);
        }
    }

    private void incrementCorrect()
    {
        nCorrects++;
        updateTextUI();
        removePlant();
        spawnPopup(2);
        StartCoroutine(HidePopUpAfterDelay(2f));

        StartCoroutine(CheckCompletionCondition());
    }

    private void incrementWrong()
    {
        deckDrawn = false;
        nWrongs++;
        updateTextUI();
        spawnPopup(1);
        StartCoroutine(HidePopUpAfterDelay(2f));

        StartCoroutine(CheckCompletionCondition());
    }

    private IEnumerator CheckCompletionCondition()
    {
        while (isActive)
        {
            yield return null;
        }

        if (deckList.Max() == currentIndex || nWrongs + nCorrects >= level[1])
        {
            CompleteGame();
        } 
        else 
        {
            currentIndex++;
            StartCoroutine(MoveCardsOut());
        }
    }

    private void CompleteGame()
    {
        deckDrawn = false;
        setTimer(0);
        playerDataSO.SetSnapTimer(DateTime.Now.AddMinutes(intervalToPlayGame));
        saveManager.Save();
        spawnPopup(5);
        StartCoroutine(RewardPlayer());
    }

    private IEnumerator RewardPlayer()
    {
        while (isActive)
        {
            yield return null;
        }

        int reward = (nCorrects - nWrongs) < 0 ? 1 : (nCorrects - nWrongs);
        EndGameEvents.Rewards.waterReward = reward;
        EndGameEvents.Rewards.fertReward = 0;
        playerDataSO.SetWater(playerDataSO.GetWater() + reward);
        SceneManager.LoadScene("EndGameScene");
    }

    private void OnExitClick(ClickEvent evt)
    {
        Debug.Log("You pressed Exit Button");

        SceneManager.LoadScene("ResourceCollectionSceneJia");
    }

    private void OnDrawClick(ClickEvent evt)
    {
        Debug.Log("You pressed Draw Button");

        if (lockout || isActive) return;
        DrawNext();
    }

    private void DrawNext()
    {
        if (deckDrawn) // When draw is pressed, deckDrawn = true
        {
            totalMoves++;
            if (deckList.Contains(currentIndex)) // Prevent drawing when the cards are the same
            {
                Debug.Log(currentIndex);
                spawnPopup(4);
                StartCoroutine(HidePopUpAfterDelay(2f));
                return;
            }
            setTimer(timePerSnap[currentLevelId]);
            currentIndex++;
            updateTextUI();
            StartCoroutine(FlipNewPlayerCard());
        } 
        else // When we press draw at the very start of the game to "begin" the game and when snap occurs
        { 
            Debug.Log("Attempt start");
            updateTextUI();
            setTimer(timePerSnap[currentLevelId]);
            StartCoroutine(FlipBothCards());
        }
    }

    private void OnBuyClick(ClickEvent evt)
    {
        Debug.Log("You pressed Buy Button");

        if (lockout || isActive) return;
        totalMoves++;
        if (!deckList.Contains(currentIndex)) // Current index
        {
            spawnPopup(3);
            StartCoroutine(HidePopUpAfterDelay(2f));
            return;
        } 
        else 
        {
            deckDrawn = false;
            incrementCorrect();
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

    private void OnAllButtonsClick(ClickEvent evt)
    {
        audioSource.Play();
    }

    private IEnumerator FlipBothCards() // Only used for game setup
    {
        lockout = true;
        int newDeckCard = UnityEngine.Random.Range(0, cardSprites.Length);
        while (newDeckCard == currentDeckCard)
        {
            newDeckCard = UnityEngine.Random.Range(0, cardSprites.Length);
        }
        int newPlayerCard = UnityEngine.Random.Range(0, cardSprites.Length);
        if (deckList.Contains(currentIndex))
        {
            newPlayerCard = newDeckCard;
        }
        else
        {
            while(newPlayerCard == newDeckCard) newPlayerCard = UnityEngine.Random.Range(0, cardSprites.Length);
        }

        playerCard.style.backgroundImage = new StyleBackground(cardSprites[newPlayerCard]);
        yield return new WaitForSeconds(0.25f);
        
        deckCard.style.backgroundImage = new StyleBackground(cardSprites[newDeckCard]);
        yield return new WaitForSeconds(0.25f);
        currentDeckCard = newDeckCard;
        deckDrawn = true;
        lockout = false;
    }

    private IEnumerator FlipNewPlayerCard()
    {
        lockout = true;
        int newPlayerCard = UnityEngine.Random.Range(0, cardSprites.Length);
        if (deckList.Contains(currentIndex))
        {
            newPlayerCard = currentDeckCard;
        }
        else
        {
            while (newPlayerCard == currentDeckCard) newPlayerCard = UnityEngine.Random.Range(0, cardSprites.Length);
        }

        playerCard.style.backgroundImage = new StyleBackground(cardSprites[newPlayerCard]);
        yield return new WaitForSeconds(0.25f);
        lockout = false;
    }

    private IEnumerator MoveCardsOut()
    {
        lockout = true;
        deckDrawn = false;
        playerCard.style.backgroundImage = new StyleBackground((Sprite)null);
        deckCard.style.backgroundImage = new StyleBackground((Sprite)null);
        yield return new WaitForSeconds(0.5f);
        lockout = false;

        if (!(nWrongs + nCorrects >= level[1]))
        {
            DrawNext();
        }
    }

    private void spawnPopup(int index) 
    {
        if (!isActive)
        {
            popUp.style.display = DisplayStyle.Flex;  
        }
        popUp.style.backgroundImage = new StyleBackground(popupSprites[index]);
        isActive = true;
    }
}
