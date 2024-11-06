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
    private float[] timePerSnap = new float[] {6, 5, 4, 3, 2, 1};
    private int intervalToPlayGame = 5;
    private float timeLeft;
    private int currentLevelId;
    private int currentDeckCard;
    private bool deckDrawn;
    private List<int> deckList;

    private int currentIndex;
    private int totalMoves;
    private int nCorrects;
    private int nWrongs;

    [SerializeField]
    Sprite[] cardSprites;
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

        currentLevelId = 0;
        deckDrawn = false;
        deckList = new List<int>();
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
        if (deckDrawn)
        {
            subtractTimer(Time.deltaTime);
            if (timeLeft < 0)
            {
                // StartCoroutine(Popup("You did not snap in time!"));
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
        updateTextUI(); // Edge case where UI does not update when the game ends after we win using Snap.
    }

    private void incrementWrong()
    {
        //currentIndex++;
        deckDrawn = false;
        nWrongs++;
        updateTextUI();

        if (nWrongs + nCorrects >= level[1])
        {
            CompleteGame();
        } 
        else 
        {
            StartCoroutine(MoveCardsOut());
        }
    }

    private void CompleteGame()
    {
        deckDrawn = false;
        RewardPlayer();
        playerDataSO.SetSnapTimer(DateTime.Now.AddMinutes(intervalToPlayGame));
        saveManager.Save();
        if (currentLevelId < timePerSnap.Length) currentLevelId++;
        deckDrawn = false;
        setTimer(0);
        //SetupGame();
    }

    private void RewardPlayer()
    {
        int reward = (nCorrects - nWrongs) < 0 ? 1 : (nCorrects - nWrongs);
        playerDataSO.SetWater(playerDataSO.GetWater() + reward);
        if (reward > 1)
        {
            // StartCoroutine(Popup(String.Format("Well done! With {1} correct and {2} wrongs, you've earned {0} water!", reward, nCorrects, nWrongs)));
        } else
        {
            // StartCoroutine(Popup(String.Format("Oh no! You made too many mistakes. you've earned 1 water!", reward, nCorrects, nWrongs)));
        }
    }

    private void OnExitClick(ClickEvent evt)
    {
        Debug.Log("You pressed Exit Button");

        SceneManager.LoadScene("ResourceCollectionSceneJia");
    }

    private void OnDrawClick(ClickEvent evt)
    {
        Debug.Log("You pressed Draw Button");

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
                // StartCoroutine(Popup("Buy when the cards are the same!"));
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

        totalMoves++;
        if (!deckList.Contains(currentIndex)) // Current index
        {
            //StartCoroutine(Popup("Draw the next card when the cards are not the same!"));
            return;
        } 
        else 
        {
            incrementCorrect();
            updateTextUI();
            removePlant();
            if (deckList.Max() == currentIndex || nWrongs + nCorrects >= level[1])
            {
                Debug.Log("Game ended!");
                CompleteGame();
                return;
            }
            currentIndex++;
            StartCoroutine(MoveCardsOut());
            deckDrawn = false;
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
        yield return new WaitForSeconds(0.8f);
        
        deckCard.style.backgroundImage = new StyleBackground(cardSprites[newDeckCard]);
        yield return new WaitForSeconds(0.5f);
        currentDeckCard = newDeckCard;
        deckDrawn = true;
    }

    private IEnumerator FlipNewPlayerCard()
    {
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
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator MoveCardsOut()
    {
        playerCard.style.backgroundImage = new StyleBackground((Sprite)null);
        deckCard.style.backgroundImage = new StyleBackground((Sprite)null);
        yield return new WaitForSeconds(0.8f);

        if (!(nWrongs + nCorrects >= level[1]))
        {
            DrawNext();
        }
    }
}
