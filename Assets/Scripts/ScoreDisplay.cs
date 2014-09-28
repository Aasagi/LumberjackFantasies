using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public UILabel PlayerDisplay;
    public UILabel LevelDisplay;
    public UILabel LogsDisplay;
    public UILabel TreeDisplay;

    private int _playerNumber;
    private int _choppedTrees;
    private int _collectedLogs;
    private int _currentLevel;

    public int PlayerNumber
    {
        set
        {
            _playerNumber = value;
            PlayerDisplay.text = _playerNumber.ToString();
        }

        get { return _playerNumber; }
    }
    public int ChoppedTrees
    {
        set
        {
            _choppedTrees = value;
            TreeDisplay.text = _choppedTrees.ToString();
        }

        get { return _choppedTrees; }
    }

    public int CollectedLogs
    {
        set
        {
            _collectedLogs = value;
            LogsDisplay.text = _collectedLogs.ToString();
        }
        get { return _collectedLogs; }
    }

    public int CurrentLevel
    {
        set
        {
            _currentLevel = value;
            LevelDisplay.text = _currentLevel.ToString();
        }
        get { return _currentLevel; }
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}