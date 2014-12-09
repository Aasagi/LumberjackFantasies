


using DatabaseConnection;

namespace Assets.Scripts.Game
{
    internal class HighScoreManager
    {
        private readonly DBOperations database;

        public HighScoreManager()
        {
            database = new DBOperations();
        }

        #region Public Methods and Operators
        public void AddHighScore(string playerName, int collectedLogs)
        {
            database.UpdateUserHighScore(playerName, collectedLogs);
        }
        #endregion
    }
}