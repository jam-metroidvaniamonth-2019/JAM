﻿using Common;
using Scenes.Main;
using UnityEngine;

namespace Player.General
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private HealthSetter _playerHealthSetter;
        [SerializeField] private float _deadWaitTime;

        public delegate void PlayerBagStatusChanged(bool playerHasBag);
        public PlayerBagStatusChanged OnPlayerBagStatusChanged;

        private bool _playerHasBag;
        private bool _playerHasBow;
        private bool _playerHasDash;

        private bool _playerDead;
        private bool _sceneSwitchActive;
        private float _playerDeadCountdown;


        #region Unity Functions

        private void Start()
        {
            _playerHasBag = true;
            _playerHasDash = false;

            _playerHealthSetter.OnHealthZero += HandleHealthZero;

            NotifyBagStatusChanged();
        }

        private void Update()
        {
            if (!_playerDead || _sceneSwitchActive)
            {
                return;
            }

            _playerDeadCountdown -= Time.deltaTime;

            if (_playerDeadCountdown <= 0)
            {
                _sceneSwitchActive = true;
                MainSceneController.Instance.FadeAndSwitchScene();
            }
        }

        #endregion

        #region External Functions

        public void PlayerCollectedBag()
        {
            _playerHasBag = true;
            NotifyBagStatusChanged();
        }

        public void PlayerLoseBag()
        {
            _playerHasBag = false;
            NotifyBagStatusChanged();
        }

        public void PlayerCollectBow() => _playerHasBow = true;

        public void PlayerLoseBow() => _playerHasBow = false;

        public void PlayerLoseDash() => _playerHasDash = false;

        public void PlayerCollectDash() => _playerHasDash = true;

        public bool PlayerHasBag => _playerHasBag;

        public bool PlayerHasBow => _playerHasBow;

        public bool PlayerHasDash => _playerHasDash;

        #endregion

        #region Utility Functions

        private void HandleHealthZero()
        {
            _playerHealthSetter.OnHealthZero -= HandleHealthZero;

            _playerDead = true;
            _playerDeadCountdown = _deadWaitTime;
        }

        private void NotifyBagStatusChanged() => OnPlayerBagStatusChanged?.Invoke(_playerHasBag);

        #endregion
    }
}