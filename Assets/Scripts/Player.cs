using System.Collections.Generic;

/// <summary>
/// Class to hold player state and scores
/// </summary>
[System.Serializable]
public class Player
{
    public string name;
    public MatchStats bestGame;
    public MatchStats currentGame;

    public Player(string name)
    {
        this.name = name;
    }

    /// <summary>
    /// Update best score and clear current game
    /// </summary>
    public void FinishGame()
    {
        bool updateBestScore = 
            bestGame == null ||
            bestGame.score == 0 ||
            bestGame.score > currentGame.score; //bigger score is bad

        if(updateBestScore)
            bestGame = currentGame;

        currentGame = null;
    }

}