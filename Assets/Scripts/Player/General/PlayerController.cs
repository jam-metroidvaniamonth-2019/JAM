using UnityEngine;

namespace Player.General
{
    public class PlayerController : MonoBehaviour
    {
        public delegate void PlayerBagStatusChanged(bool playerHasBag);
        public PlayerBagStatusChanged OnPlayerBagStatusChanged;

        private bool _playerHasBag;
        private bool _playerHasBow;
        private bool _playerHasDash;


        #region Unity Functions

        private void Start()
        {
            _playerHasBag = true;
            _playerHasDash = false;

            NotifyBagStatusChanged();
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

        private void NotifyBagStatusChanged() => OnPlayerBagStatusChanged?.Invoke(_playerHasBag);

        #endregion
    }
}