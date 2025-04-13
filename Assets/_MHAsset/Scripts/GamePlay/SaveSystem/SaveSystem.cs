

using UnityEngine;

namespace MH.SaveSystem
{
    public interface ISaveSystem
    {
        GameSave GameSave { get; }
        GameSave Load();
        void Save();

        void Clean();
    }

    public class SaveSystem : ISaveSystem
    {

        private readonly string _saveKey = "Game Save";
        private GameSave _gameSave;

        public GameSave GameSave => _gameSave ?? Load();

        public SaveSystem()
        {
            Load();
        }

        public GameSave Load()
        {
            if (PlayerPrefs.HasKey(_saveKey))
            {
                string saveText = PlayerPrefs.GetString(_saveKey);
                _gameSave = JsonUtility.FromJson<GameSave>(saveText);
            }
            else
            {
                _gameSave = new();
            }

            return _gameSave;
           
        }

        public void Save()
        {
            string jsonText = JsonUtility.ToJson(GameSave);    
            PlayerPrefs.SetString(_saveKey, jsonText);
        }

        public void Clean()
        {
            PlayerPrefs.DeleteKey(_saveKey);
            _gameSave = new();
        }
    }
}
