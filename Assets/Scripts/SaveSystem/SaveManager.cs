using UnityEngine;

namespace SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] private bool _clearOnStart;

        private const string SavePref = "MtJam";

        public delegate void SaveComplete();
        public delegate void LoadComplete();

        public SaveComplete OnSaveComplete;
        public LoadComplete OnLoadComplete;

        private bool _gameSavedAtLeastOnce;

        private SaveStructure _saveStructure;
        private bool _disableSaving;

        public SaveStructure SaveStructure
        {
            get => _saveStructure;
            set => _saveStructure = value;
        }

        #region Unity Functions

        private void Start()
        {
            Initialize();

            if (_clearOnStart)
            {
                ClearData();
            }
            else
            {
                LoadData();
            }
        }

        #endregion

        #region Save/Load Data

        public void SaveData()
        {
            if (_disableSaving)
            {
                return;
            }

            string jsonData = JsonUtility.ToJson(_saveStructure);
            PlayerPrefs.SetString(SavePref, jsonData);
            OnSaveComplete?.Invoke();

            _gameSavedAtLeastOnce = true;
        }

        public void ClearData()
        {
            if (PlayerPrefs.HasKey(SavePref))
            {
                PlayerPrefs.DeleteKey(SavePref);
            }
        }

        private void LoadData()
        {
            if (PlayerPrefs.HasKey(SavePref))
            {
                _saveStructure = JsonUtility.FromJson<SaveStructure>(PlayerPrefs.GetString(SavePref));
                OnLoadComplete?.Invoke();
            }
        }

        #endregion

        #region External Functions

        public void DisableSaving() => _disableSaving = true;

        public void EnableSaving() => _disableSaving = false;

        public bool IsSavingDisabled() => _disableSaving;

        public bool GameSavedAtLeastOnce => _gameSavedAtLeastOnce;

        #endregion

        #region Utility Functions

        private void Initialize() => _saveStructure = new SaveStructure();

        #endregion

        #region Singleton

        private static SaveManager _instance;
        public static SaveManager Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }

            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        #endregion
    }
}