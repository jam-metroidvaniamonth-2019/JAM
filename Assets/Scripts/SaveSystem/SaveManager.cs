using UnityEngine;

namespace SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        private const string SavePref = "MtJam";

        private SaveStructure _saveStructure;

        public SaveStructure SaveStructure
        {
            get => _saveStructure;
            set => _saveStructure = value;
        }

        #region Unity Functions

        private void Start()
        {
            Initialize();
            LoadData();
        }

        #endregion


        #region Save/Load Data

        public void SaveData()
        {
            string jsonData = JsonUtility.ToJson(_saveStructure);
            PlayerPrefs.SetString(SavePref, jsonData);
        }

        public void LoadData()
        {
            if (PlayerPrefs.HasKey(SavePref))
            {
                _saveStructure = JsonUtility.FromJson<SaveStructure>(PlayerPrefs.GetString(SavePref));
            }
        }

        #endregion

        #region Utility Functions

        private void Initialize() => _saveStructure = new SaveStructure();

        #endregion

        #region Singleton

        public static SaveManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        #endregion
    }
}