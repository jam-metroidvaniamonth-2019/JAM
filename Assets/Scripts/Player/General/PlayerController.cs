using UnityEngine;

namespace Player.General
{
    public class PlayerController : MonoBehaviour
    {
        public delegate void PlayerBagStatusChanged(bool playerHasBag);
        public PlayerBagStatusChanged OnPlayerBagStatusChanged;

        private bool _playerHasBag;
        private bool _playerHasBow;

        #region Unity Functions

        private void Start()
        {
            _playerHasBag = true;
            NotifyBagStatusChanged();
        }

        #endregion

        #region External Functions

        public void PlayerCollectedBag()
        {
            _playerHasBag = true;
            NotifyBagStatusChanged();
        }

        public void PlayerLostBag()
        {
            _playerHasBag = false;
            NotifyBagStatusChanged();
        }

        public void PlayerCollectBow() => _playerHasBow = true;

        public void PlayerLoseBow() => _playerHasBow = false;

        public bool PlayerHasBag => _playerHasBag;

        public bool PlayerHasBow => _playerHasBow;

        #endregion

        #region Utility Functions

        private void NotifyBagStatusChanged() => OnPlayerBagStatusChanged?.Invoke(_playerHasBag);

        #endregion
    }
}