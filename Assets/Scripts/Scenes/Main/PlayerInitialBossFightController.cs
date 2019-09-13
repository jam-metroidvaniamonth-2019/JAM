using System.Collections.Generic;
using Common;
using Interactibles.ColliderModifier;
using Interactibles.Followers;
using Player.Movement;
using Player.Shooting;
using UI.CutScene;
using UnityEngine;
using Utils;

namespace Scenes.Main
{
    public class PlayerInitialBossFightController : MonoBehaviour
    {
        [Header("Boss Scene")]
        [SerializeField] private GameObject _leftBoundaryLocker;
        [SerializeField] private GameObject _rightBoundaryLocker;
        [SerializeField] private GameObject _bossEnemy;
        [SerializeField] private CollisionNotifier _bossSceneActivator;

        [Header("CutScene Controls")]
        [SerializeField] private float _cutSceneDisplayTime;
        [SerializeField] private Sprite _cutSceneImage;

        [Header("Player")]
        [SerializeField] private HealthSetter _playerHealthSetter;
        [SerializeField] [Range(0.1f, 0.9f)] private float _playerLowHealthLimit;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerShooter _playerShooter;
        [SerializeField] private Transform _player;

        [Header("Fireflies")]
        [SerializeField] private int _fireflySpawnCount = 5;
        [SerializeField] private CollisionNotifier _firefliesExitNotifier;

        private List<int> _fireflyIndices;

        #region Unity Functions

        private void Start()
        {
            _fireflyIndices = new List<int>();

            _leftBoundaryLocker.SetActive(false);
            _rightBoundaryLocker.SetActive(false);

            _playerHealthSetter.OnHealthChanged += HandlePlayerHealthChange;
            _firefliesExitNotifier.OnTriggerEntered += HandlePlayerEnteredFireflyExit;
            _bossSceneActivator.OnTriggerEntered += ActivateBossScene;
        }

        #endregion

        #region Utility Functions

        private void HandlePlayerHealthChange(float currentHealth, float maxHealth)
        {
            float healthRatio = currentHealth / maxHealth;
            if (healthRatio <= _playerLowHealthLimit)
            {
                ActivateCutSceneSequence();
                _playerHealthSetter.OnHealthChanged -= HandlePlayerHealthChange;
            }
        }

        private void HandleCutSceneOpen()
        {
            CutSceneDisplay.Instance.OnCutSceneOpen -= HandleCutSceneOpen;

            for (int i = 0; i < _fireflySpawnCount; i++)
            {
                int spawnIndex = FireflyManager.Instance.AddFirefly(false, _player.position, _player);
                _fireflyIndices.Add(spawnIndex);
            }
        }

        private void HandleCutSceneClose()
        {
            CutSceneDisplay.Instance.OnCutSceneClose -= HandleCutSceneClose;

            _playerMovement.EnableMovement();
            _playerShooter.EnableShooting();
        }

        private void HandlePlayerEnteredFireflyExit(Collider2D other)
        {
            _firefliesExitNotifier.OnTriggerEntered -= HandlePlayerEnteredFireflyExit;
            if (other.CompareTag(TagManager.Player))
            {
                for (int i = 0; i < _fireflyIndices.Count; i++)
                {
                    FireflyManager.Instance.UpdateFireflyTargetToRandom(_fireflyIndices[i], true);
                }

                _fireflyIndices.Clear();
            }
        }

        private void ActivateBossScene(Collider2D other)
        {
            if (other.CompareTag(TagManager.Player))
            {
                _leftBoundaryLocker.SetActive(true);
                _rightBoundaryLocker.SetActive(true);

                _bossSceneActivator.OnTriggerEntered -= ActivateBossScene;
            }
        }

        private void ActivateCutSceneSequence()
        {
            _playerMovement.DisableMovement();
            _playerShooter.DisableShooting();

            Destroy(_bossEnemy);

            _leftBoundaryLocker.SetActive(false);
            _rightBoundaryLocker.SetActive(false);

            CutSceneDisplay.Instance.DisplayCutScene(_cutSceneImage, _cutSceneDisplayTime);

            CutSceneDisplay.Instance.OnCutSceneOpen += HandleCutSceneOpen;
            CutSceneDisplay.Instance.OnCutSceneClose += HandleCutSceneClose;
        }

        #endregion
    }
}