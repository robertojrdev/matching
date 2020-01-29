using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameState
{
    [SerializeField]private List<Player> players = new List<Player>();

    public List<Player> Players { get => players;}

    public bool ContainsPlayer(string name)
    {
        return Players.FindIndex(x => x.name == name) != -1;
    }

    public void AddPlayer(string name)
    {
        Players.Add(new Player(name, 0, 0));
    }

    public bool UpdateScore(string name, int time, int tries)
    {
        if (!ContainsPlayer(name))
        {
            Debug.LogError("Player not found");
            return false;
        }

        var newScore = Player.GetScore(time, tries);
        var player = Players.Find(x => x.name == name);

        if (player.score < newScore)
        {
            player.time = time;
            player.tries = tries;
            return true;
        }

        return false;
    }
}

[System.Serializable]
public class Player
{
    public string name;
    public int time;
    public int tries;
    public int score => GetScore(time, tries);

    public Player(string name, int time, int tries)
    {
        this.name = name;
        this.time = time;
        this.tries = tries;
    }

    public static int GetScore(int time, int tries) => tries * 5 + time;
    
}