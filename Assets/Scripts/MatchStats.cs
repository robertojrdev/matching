using System.Collections.Generic;

[System.Serializable]
public class MatchStats
{
    public bool isOn;
    public int time;
    public int tries;
    public List<int> matchedCards = new List<int>();
    public CardsSequence cardsSequence;
    public int score => GetScore(time, tries);

    public MatchStats(CardsSequence cardsSequence, bool isOn = true)
    {
        this.cardsSequence = cardsSequence;
        this.isOn = isOn;
    }

    /// <summary>
    /// Calculate player score based on time and tries
    /// </summary>
    /// <returns>score result - tries * 5 + time</returns>
    public static int GetScore(int time, int tries) => tries * 5 + time;
}