﻿using Common;
using UnityEngine;
using Utils;

namespace SaveSystem
{
    public class PlayerSaveHelper : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private HealthSetter _playerHealthSetter;
        [SerializeField] private GameObject _saveEffect;
        [SerializeField] private float _saveEffectZPosition;

        #region Unity Functions

        private void Start()
        {
            SaveManager.Instance.OnLoadComplete += HandleLoadData;
        }

        private void OnDestroy()
        {
            SaveManager.Instance.OnLoadComplete -= HandleLoadData;
        }

        private void Update()
        {
            if (Input.GetButtonDown(ControlConstants.SaveButton))
            {
                if (!SaveManager.Instance.IsSavingDisabled())
                {
                    SavePlayerData();
                }
            }
        }

        #endregion

        #region Utility Functions

        private void HandleLoadData()
        {
            float playerXPosition = SaveManager.Instance.SaveStructure.playerXPosition;
            float playerYPosition = SaveManager.Instance.SaveStructure.playerYPosition;
            float playerHealth = SaveManager.Instance.SaveStructure.playerHealth;

            _player.transform.position = new Vector3(playerXPosition, playerYPosition, 0);
            _playerHealthSetter.SetCurrentHealth(playerHealth);
        }

        private void SavePlayerData()
        {
            if (_playerHealthSetter.CurrentHealth <= 0)
            {
                return;
            }

            Vector3 playerPosition = _player.position;
            float playerHealth = _playerHealthSetter.CurrentHealth;

            SaveManager.Instance.SaveStructure.playerHealth = playerHealth;
            SaveManager.Instance.SaveStructure.playerXPosition = playerPosition.x;
            SaveManager.Instance.SaveStructure.playerYPosition = playerPosition.y;

            SaveManager.Instance.SaveData();

            Vector3 spawnPosition = new Vector3(_player.position.x, _player.position.y, _player.position.z + _saveEffectZPosition);
            Instantiate(_saveEffect, spawnPosition, Quaternion.identity);
        }

        #endregion
    }
}