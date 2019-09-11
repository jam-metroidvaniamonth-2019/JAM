using System;
using Player.Movement;
using Player.Shooting;
using UnityEngine;
using Utils;

namespace UI.CutScene
{
    [RequireComponent(typeof(Collider2D))]
    public class CutSceneTrigger : MonoBehaviour
    {
        [SerializeField] private Sprite _cutSceneSprite;
        [SerializeField] private float _cutSceneDisplayTime;

        private PlayerMovement _playerMovement;
        private PlayerShooter _playerShooter;

        private bool _cutSceneTriggered;

        #region Unity Functions

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(TagManager.Player) && !_cutSceneTriggered)
            {
                CutSceneDisplay.Instance.DisplayCutScene(_cutSceneSprite, _cutSceneDisplayTime);
                CutSceneDisplay.Instance.OnCutSceneClose += HandleCutSceneClose;

                _playerMovement = other.GetComponent<PlayerMovement>();
                _playerShooter = other.GetComponent<PlayerShooter>();

                DisablePlayerControls();
                _cutSceneTriggered = true;
            }
        }

        #endregion

        #region Utility Functions

        private void HandleCutSceneClose()
        {
            CutSceneDisplay.Instance.OnCutSceneClose -= HandleCutSceneClose;
            EnablePlayerControls();
        }

        private void DisablePlayerControls()
        {
            _playerMovement.DisableMovement();
            _playerShooter.DisableShooting();
        }

        private void EnablePlayerControls()
        {
            _playerMovement.EnableMovement();
            _playerShooter.EnableShooting();
        }

        #endregion
    }
}