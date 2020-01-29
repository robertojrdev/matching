using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}


    private const string SAVE_GAME = "savegame";
    private const string PATH_UNIT_CARDS = "Unit_Cards";
    private const string PATH_UNIT_CARDS_LOCK = "Unit_Cards_Lock";


    [SerializeField] private GameObject loginView;
    [SerializeField] private GameObject gameView;
    [SerializeField] private Match match;
    

    public Sprite[] unitSprites {get; private set;}
    public Sprite[] unitLockSprites {get; private set;}


    public GameState gameState { get; private set; }
    private string currentPlayer;


    private void Awake()
    {
        if(!instance)
            instance = this;
        else
        {
            Debug.LogWarning("Multiple GameManagers in the scene", gameObject);
            enabled = false;
            return;
        }

        LoadGameOrDefault();

        unitSprites = Resources.LoadAll(PATH_UNIT_CARDS, typeof(Sprite)).
            Cast<Sprite>().ToArray();
        unitLockSprites = Resources.LoadAll(PATH_UNIT_CARDS, typeof(Sprite)).
            Cast<Sprite>().ToArray();
    }

    private void LoadGameOrDefault()
    {
        if (PlayerPrefs.HasKey(SAVE_GAME))
        {
            var jsonString = PlayerPrefs.GetString(SAVE_GAME);
            gameState = JsonUtility.FromJson<GameState>(jsonString);
            print(jsonString);
        }
        else
        {
            gameState = new GameState();
        }
    }

    private void SaveGame()
    {
        var json = JsonUtility.ToJson(gameState);
        PlayerPrefs.SetString(SAVE_GAME, json);
        print(json);
    }

    public static void StartSession(string playerName)
    {
        if(!instance)
        {
            Debug.LogError("No GameManager instance in the scene");
            return;
        }

        instance.currentPlayer = playerName;
        
        if(!instance.gameState.ContainsPlayer(playerName))
            instance.gameState.AddPlayer(playerName);

        instance.StartGame();
    }

    private void StartGame()
    {
        loginView.SetActive(false);
        gameView.SetActive(true);
        match.NewMatch();
    }

    public static void OnFinishMatch(int time, int tries)
    {
        if(!instance)
        {
            Debug.LogError("No GameManager instance in the scene");
            return;
        }

        instance.gameState.UpdateScore(instance.currentPlayer, time, tries);

        foreach (var item in instance.gameState.Players)
        {
            print(item.name);
            print(item.score);
            print(item.time);
            print(item.tries);
        }

        instance.SaveGame();
    }
}