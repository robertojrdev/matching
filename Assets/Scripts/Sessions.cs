using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to hold the game state and also can be serialized and saved as a JSON
/// </summary>
[System.Serializable]
public class Sessions
{
    [SerializeField] private List<Player> players = new List<Player>();

    public List<Player> Players { get => players; }

    public bool ContainsPlayer(string name)
    {
        return Players.FindIndex(x => x.name == name) != -1;
    }

    public Player GetPlayer(string name)
    {
        return Players.Find(x => x.name == name);
    }

    public Player AddPlayer(string name)
    {
        var player = new Player(name);
        Players.Add(player);
        return player;
    }
}