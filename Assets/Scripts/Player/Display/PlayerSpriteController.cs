using System;
using Player.General;
using UnityEngine;

namespace Player.Display
{
    public class PlayerSpriteController : MonoBehaviour
    {
        [SerializeField] private GameObject _playerNormalSprite;
        [SerializeField] private GameObject _playerBagSprite;
        [SerializeField] private PlayerController _playerController;

        #region Unity Functions

        private void Start() => _playerController.OnPlayerBagStatusChanged += HandlePlayerBagStatusChanged;

        private void OnDestroy() => _playerController.OnPlayerBagStatusChanged -= HandlePlayerBagStatusChanged;

        #endregion

        #region Utility Functions

        private void HandlePlayerBagStatusChanged(bool playerHasBag)
        {
            if (playerHasBag)
            {
                _playerBagSprite.SetActive(true);
                _playerNormalSprite.SetActive(false);
            }
            else
            {
                _playerNormalSprite.SetActive(true);
                _playerBagSprite.SetActive(false);
            }
        }

        #endregion
    }
}