using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Match : MonoBehaviour
{
    private const string TEXT_TIME = "Elapsed time: ";
    private const string TEXT_TRIES = "Tries: ";

    public int cardsAmount = 28;
    [SerializeField] private Transform winView;
    [SerializeField] private Transform cardsView;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Text textTime;
    [SerializeField] private Text textTries;
    [SerializeField] private Text textPlayer;

    private List<Card> cards = new List<Card>();
    private HashSet<Card> selectedCards = new HashSet<Card>();
    private int pairsAmount;
    private bool isGameActive = false;
    private float startTime = 0;
    private int triesCount = 0;
    private int correctTries = 0;

    private void Update()
    {
        if (!isGameActive)
            return;

        var time = Time.time - startTime;
        var mins = (int)(time / 60);
        var secs = (int)(time % 60);
        textTime.text = TEXT_TIME + string.Format("{0:D2}:{1:D2}", mins, secs);
    }

    [ContextMenu("New Match")]
    public void NewMatch()
    {
        winView.gameObject.SetActive(false);
        ClearCards();
        InstantiateNewCards();
        ShuffleCards();
        isGameActive = true;
        startTime = Time.time;
        triesCount = 0;
        correctTries = 0;
    }

    [ContextMenu("Reveal")]
    private void ShowAll()
    {
        foreach (var card in cards)
        {
            card.SetFlippedState(true);
        }
    }

    public void ClearCards()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            Destroy(cards[i].gameObject);
        }

        cards.Clear();
    }

    private void InstantiateNewCards()
    {
        int loadedCards = GameManager.instance.unitSprites.Length;
        pairsAmount = cardsAmount / 3;

        //get random cards
        HashSet<int> randomChosenCards = new HashSet<int>();
        do
        {
            randomChosenCards.Add(UnityEngine.Random.Range(0, loadedCards));
        }
        while (randomChosenCards.Count < pairsAmount);

        //instantiate cards sequence
        foreach (int id in randomChosenCards)
        {
            for (int i = 0; i < 3; i++)
            {
                var card = Instantiate(cardPrefab, cardsView);
                card.unitImage = GameManager.instance.unitSprites[id];
                cards.Add(card);
            }
        }

        //instantiate left cards - the ones that will not have enough pairs
        int leftCardId = 0;
        int leftAmount = cardsAmount - pairsAmount * 3;

        do
        {
            leftCardId = UnityEngine.Random.Range(0, loadedCards);
        }
        while (randomChosenCards.Contains(leftCardId));

        for (int i = 0; i < leftAmount; i++)
        {
            var card = Instantiate(cardPrefab, cardsView);
            card.unitImage = GameManager.instance.unitSprites[leftCardId];
            cards.Add(card);
        }

        //add event to all cards at once just to stay organized
        foreach (var card in cards)
            card.button.onClick.AddListener(() => OnSelectCard(card));
    }

    private void ShuffleCards()
    {
        int amount = cards.Count;
        var randList = cards.OrderBy(x => UnityEngine.Random.value).ToArray();
        for (int i = 0; i < randList.Length; i++)
        {
            randList[i].transform.SetSiblingIndex(i);
        }
    }

    private void OnSelectCard(Card card)
    {
        card.Flip();
        card.SetInteractionEnabled(false);
        selectedCards.Add(card);

        if (selectedCards.Count == 3)
        {
            triesCount++;
            textTries.text = TEXT_TRIES + triesCount.ToString("D2");
            StartCoroutine(CheckResult());
        }
    }

    private IEnumerator CheckResult()
    {
        //lock all cards interaction;
        foreach (var card in cards)
        {
            card.SetInteractionEnabled(false);
        }

        //w8 a bit :)
        yield return new WaitForSeconds(1f);

        //compare cards
        bool cardsMatch = true;
        Sprite img = null;
        foreach (var card in selectedCards)
        {
            if (img != null)
            {
                if (card.unitImage != img)
                {
                    cardsMatch = false;
                    break;
                }
            }
            else
                img = card.unitImage;
        }

        if (cardsMatch) //CORRECT
        {
            foreach (var card in selectedCards)
                card.MatchFound();

            correctTries++;
        }
        else            //WRONG
        {
            foreach (var card in selectedCards)
                card.Flip();
        }

        //check for win game
        if (correctTries >= pairsAmount)
        {
            Won();
        }
        else
        {
            //enable cards interaction
            foreach (var card in cards)
                if (!card.match)
                    card.SetInteractionEnabled(true);
        }

        selectedCards.Clear();
    }

    [ContextMenu("Win game")]
    private void Won()
    {
        isGameActive = false;

        GameManager.OnFinishMatch((int)(Time.time - startTime), triesCount);
        winView.gameObject.SetActive(true);
    }
}