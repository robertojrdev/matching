using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// singleton
    /// </summary>
    public static GameManager instance { get; private set; }

    #region Constant variables
    private const string SAVE_GAME = "savegame";
    private const string PATH_UNIT_CARDS = "Unit_Cards";
    private const string PATH_UNIT_CARDS_LOCK = "Unit_Cards_Lock";
    #endregion

    #region Inspector Variables
    [SerializeField] private GameObject loginView;
    [SerializeField] private GameObject gameView;
    [SerializeField] private Match match;
    #endregion

    #region Properties
    public Sprite[] unitSprites { get; private set; }
    public Sprite[] unitLockSprites { get; private set; }
    public GameState gameState { get; private set; }
    #endregion

    #region Private Variables
    private string currentPlayer;
    #endregion

    #region Class Methods
    private void Awake()
    {
        //singleton
        if (!instance)
            instance = this;
        else
        {
            Debug.LogWarning("Multiple GameManagers in the scene", gameObject);
            enabled = false;
            return;
        }

        LoadGameOrDefault();
        LoadUnits();
    }

    /// <summary>
    /// Load all unit images from Resource folder
    /// </summary>
    private void LoadUnits()
    {
        unitSprites = Resources.LoadAll(PATH_UNIT_CARDS, typeof(Sprite)).
                    Cast<Sprite>().ToArray();
        unitLockSprites = Resources.LoadAll(PATH_UNIT_CARDS, typeof(Sprite)).
            Cast<Sprite>().ToArray();
    }

    /// <summary>
    /// If available, load previous games, otherwise create a game state
    /// </summary>
    private void LoadGameOrDefault()
    {
        gameState = new GameState();

        //try to load
        if (PlayerPrefs.HasKey(SAVE_GAME))
        {
            var jsonString = PlayerPrefs.GetString(SAVE_GAME);
            JsonUtility.FromJsonOverwrite(jsonString, gameState);
        }
    }

    /// <summary>
    /// Sava game in player prefs using JSON
    /// </summary>
    private void SaveGame()
    {
        var json = JsonUtility.ToJson(gameState);
        PlayerPrefs.SetString(SAVE_GAME, json);
    }

    /// <summary>
    /// Display the game view and start the game
    /// </summary>
    private void StartGame()
    {
        loginView.SetActive(false);
        gameView.SetActive(true);
        match.NewMatch();
    }

    /// <summary>
    /// Clear all sessions from PlayerPrefs
    /// </summary>
    [ContextMenu("Reset Saved Games")]
    public void ResetSavedGames()
    {
        PlayerPrefs.DeleteKey(SAVE_GAME);
    }
    #endregion

    #region Static Methods
    /// <summary>
    /// Use from previous games or create a new player
    /// </summary>
    /// <param name="playerName"></param>
    public static void StartSession(string playerName)
    {
        if (!instance)
        {
            Debug.LogError("No GameManager instance in the scene");
            return;
        }

        instance.currentPlayer = playerName;

        if (!instance.gameState.ContainsPlayer(playerName))
            instance.gameState.AddPlayer(playerName);

        instance.StartGame();
    }

    public static void OnFinishMatch(int time, int tries)
    {
        if (!instance)
        {
            Debug.LogError("No GameManager instance in the scene");
            return;
        }

        instance.gameState.UpdateScore(instance.currentPlayer, time, tries);

        instance.SaveGame();
    }
    #endregion
}