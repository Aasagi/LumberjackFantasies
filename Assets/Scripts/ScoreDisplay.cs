using System;
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

    public EventHandler LogsChanged;
    public EventHandler LevelChanged;
    public EventHandler TreesChanged;

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
            if (TreesChanged != null) TreesChanged(_choppedTrees, null);
            TreeDisplay.text = _choppedTrees.ToString();
        }

        get { return _choppedTrees; }
    }

    public int CollectedLogs
    {
        set
        {
            _collectedLogs = value;
            if (LogsChanged != null) LogsChanged(_collectedLogs, null);
            LogsDisplay.text = _collectedLogs.ToString();
        }
        get { return _collectedLogs; }
    }

    public int CurrentLevel
    {
        set
        {
            _currentLevel = value;
            if (LevelChanged != null) LevelChanged(_currentLevel, null);
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