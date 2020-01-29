using UnityEngine;
using UnityEngine.UI;

public class LeaderboardLine : MonoBehaviour
{
    [SerializeField] private Text rankText;
    [SerializeField] private Text playerNameText;
    [SerializeField] private Text triesText;
    [SerializeField] private Text timeText;
    [SerializeField] private Text scoreText;

    public string rank { set => rankText.text = value; }
    public string playerName { set => playerNameText.text = value; }
    public string tries { set => triesText.text = value; }
    public string time { set => timeText.text = value; }
    public string score { set => scoreText.text = value; }
}