using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private LeaderboardLine linePrefab;

    private HashSet<LeaderboardLine> lines = new HashSet<LeaderboardLine>();

    private void OnEnable()
    {
        Fullfill();
    }

    private void OnDisable()
    {
        Clear();
    }

    private void Fullfill()
    {
        if (!GameManager.instance)
            return;

        var players = GameManager.instance.gameState.Players.
            FindAll(x => x.score != 0).
            OrderBy(x => x.score).
            ToArray();

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].score == 0)
                continue;

            var line = Instantiate(linePrefab, container);
            line.rank = i.ToString();
            line.playerName = players[i].name;
            line.tries = players[i].tries.ToString();
            line.score = players[i].score.ToString();

            var mins = players[i].time / 60;
            var secs = players[i].time % 60;
            line.time = string.Format("{0:D2}:{1:D2}", mins, secs);

            line.gameObject.SetActive(true);
            lines.Add(line);
        }

    }

    private void Clear()
    {
        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }

        lines.Clear();
    }
}