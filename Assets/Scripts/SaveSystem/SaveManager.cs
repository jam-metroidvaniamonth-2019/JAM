using UnityEngine;

namespace SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
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

        private void Start() => Initialize();

        #endregion

        #region Save/Load Data

        public void SaveData()
        {
            if (_disableSaving)
            {
                return;
            }

            _gameSavedAtLeastOnce = true;

            OnSaveComplete?.Invoke();
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