using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Match : MonoBehaviour
{
    #region Constant Variables
    private const string TEXT_TIME = "Elapsed time: ";
    private const string TEXT_TRIES = "Tries: ";
    #endregion

    #region Inspector Variables
    public int cardsAmount = 28; //to avoid magic numbers and allow more game setups
    public int pairsSize = 3; //to avoid magic numbers and allow more game setups
    [SerializeField] private Transform winView;
    [SerializeField] private Transform cardsView;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Text textTime;
    [SerializeField] private Text textTries;
    [SerializeField] private Text textPlayer;
    [SerializeField] private Transform foundCardsHolder;
    #endregion

    #region Private Variables
    private List<Card> cards = new List<Card>();
    private HashSet<Card> selectedCards = new HashSet<Card>();
    private float startTime = 0;
    #endregion

    private int pairsAmount => cardsAmount / pairsSize;

    private void Update()
    {
        var hasPlayer = GameManager.currentPlayer?.currentGame != null;
        if (!hasPlayer || !GameManager.currentPlayer.currentGame.isOn)
            return;

        var time = Time.time - startTime;
        var mins = (int)(time / 60);
        var secs = (int)(time % 60);
        textTime.text = TEXT_TIME + string.Format("{0:D2}:{1:D2}", mins, secs);
        GameManager.currentPlayer.currentGame.time = (int)time;
    }

    [ContextMenu("New Match")]
    public void NewMatch()
    {
        winView.gameObject.SetActive(false);
        textTries.text = TEXT_TRIES + "00";

        ClearCards();
        InstantiateNewCards();

        int loadedCards = GameManager.instance.unitSprites.Length;
        int[] units = GetRandomCardsId(cardsAmount, loadedCards);
        int[] order = GetShuffleSequence(cardsAmount);

        var sequence = new CardsSequence(units, order);
        ApplySequenceToCards(cards, sequence);

        startTime = Time.time;
        GameManager.currentPlayer.currentGame = new MatchStats(sequence);
    }

    public void LoadMatch()
    {
        winView.gameObject.SetActive(false);

        ClearCards();
        InstantiateNewCards();

        var game = GameManager.currentPlayer.currentGame;
        ApplySequenceToCards(cards, game.cardsSequence);

        startTime = Time.time - game.time;
        textTries.text = TEXT_TRIES + (game.matchedCards.Count / pairsSize).ToString("D2");

        StartCoroutine(FlipMatchedCards());
    }

    /// <summary>
    /// Flip all matched cards from a previous game
    /// </summary>
    /// <returns></returns>
    private IEnumerator FlipMatchedCards()
    {

        int count = 0;
        foreach (var index in GameManager.currentPlayer.currentGame.matchedCards)
        {
            yield return new WaitForSeconds(0.2f);
            var card = cardsView.GetChild(index).GetComponent<Card>();
            card.Flip();
            card.MatchFound(0);
            card.interactable = false;

            if(count % pairsSize == 0)
            {
                var position = count / pairsSize;
                var foundCard = foundCardsHolder.GetChild(position).GetComponent<Card>();
                foundCard.SetAsFoundCard(card.unitImage, .7f);
            }

            count++;
        }
    }

    [ContextMenu("Reveal")]
    private void ShowAll() //for debugging
    {
        foreach (var card in cards)
        {
            card.SetFlippedState(true);
        }
    }

    /// <summary>
    /// Remove all cards from the game and reset the selected cards tab
    /// </summary>
    public void ClearCards()
    {
        //remove all cards
        for (int i = 0; i < cards.Count; i++)
        {
            Destroy(cards[i].gameObject);
        }
        cards.Clear();

        //clear found cards deck
        foreach (Transform child in foundCardsHolder)
        {
            child.GetComponent<Card>().image.sprite = null;
        }
    }

    private void InstantiateNewCards()
    {
        for (int i = 0; i < cardsAmount; i++)
        {
            var card = Instantiate(cardPrefab, cardsView);
            card.button.onClick.AddListener(() => OnSelectCard(card));
            cards.Add(card);
            card.name = i.ToString("D2");
        }
    }

    /// <summary>
    /// Get random unique index list of size in a range
    /// </summary>
    /// <param name="size">size of the return list</param>
    /// <param name="range">range of unique numbers to be selected</param>
    private int[] GetRandomCardsId(int size, int range)
    {
        int leftAmount = size - pairsAmount * pairsSize;

        //get random units
        HashSet<int> randomChosenCards = new HashSet<int>();
        do
        {
            randomChosenCards.Add(UnityEngine.Random.Range(0, range));
        }
        while (randomChosenCards.Count < pairsAmount + leftAmount);

        return randomChosenCards.ToArray();
    }

    private int[] GetShuffleSequence(int size)
    {
        int[] order = new int[size];
        for (int i = 0; i < size; i++)
            order[i] = i;

        return order.OrderBy(x => UnityEngine.Random.value).ToArray();
    }

    private void ApplySequenceToCards(IList<Card> cards, CardsSequence sequence)
    {
        int count = cards.Count;

        //add unit sprites to cards
        int i = 0;
        foreach (var unitId in sequence.units)
        {
            for (int j = 0; j < pairsSize && i < count; j++, i++)
            {
                cards[i].unitImage = GameManager.instance.unitSprites[unitId];
            }
        }

        //set the order
        for (int j = 0; j < count; j++)
        {
            var position = sequence.order[j];
            cards[position].transform.SetSiblingIndex(j);
        }
    }

    private void OnSelectCard(Card card)
    {
        card.Flip();
        card.interactable = false;
        selectedCards.Add(card);

        if (selectedCards.Count == pairsSize)
        {
            var tries = ++GameManager.currentPlayer.currentGame.tries;
            textTries.text = TEXT_TRIES + tries.ToString("D2");
            StartCoroutine(CheckResult());
        }
    }

    private IEnumerator CheckResult()
    {
        //lock all cards interaction;
        foreach (var card in cards)
        {
            card.interactable = false;
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

        if (cardsMatch) //!CORRECT
        {
            int id = 0;
            foreach (var card in selectedCards)
            {
                var cardId = card.transform.GetSiblingIndex();
                GameManager.currentPlayer.currentGame.matchedCards.Add(cardId);
                card.MatchFound(id * 0.1f);
                id++;
            }

            //Add card to found cards deck
            var deckId = GameManager.currentPlayer.currentGame.matchedCards.Count / pairsSize -1;
            var foundCard = foundCardsHolder.GetChild(deckId).GetComponent<Card>();
            foundCard.SetAsFoundCard(img, .7f);
        }
        else            //!WRONG
        {
            foreach (var card in selectedCards)
                card.Flip();
        }

        //check for win game
        var foundPairs = GameManager.currentPlayer.currentGame.matchedCards.Count / pairsSize;
        if (foundPairs >= pairsAmount)
        {
            Won();
        }
        else
        {
            //enable cards interaction
            foreach (var card in cards)
                if (!card.match)
                    card.interactable = true;
        }

        selectedCards.Clear();
    }

    [ContextMenu("Win game")]
    private void Won()
    {
        GameManager.currentPlayer.FinishGame();
        GameManager.OnFinishMatch();
        winView.gameObject.SetActive(true);
    }
}
