using System.Collections.Generic;
using Common;
using Interactibles.ColliderModifier;
using Interactibles.Followers;
using Player.General;
using Player.Movement;
using Player.Shooting;
using SpeechSystem;
using UI.CutScene;
using UnityEngine;
using Utils;
using WorldDisplay;

namespace Scenes.Main
{
    public class PlayerInitialBossFightController : MonoBehaviour
    {
        [Header("Boss Scene")]
        [SerializeField] private Rigidbody2D _fallingStone;
        [SerializeField] private float _fallingStoneMass;
        [SerializeField] private TriggerExplosionOnChildren _rightBoundaryLocker;
        [SerializeField] private GameObject _bossEnemy;
        [SerializeField] private CollisionNotifier _bossSceneActivator;

        [Header("CutScene Controls")]
        [SerializeField] private float _cutSceneDisplayTime;
        [SerializeField] private Sprite _cutSceneImage;
        [SerializeField] [TextArea] private string[] _dialogues;

        [Header("Player")]
        [SerializeField] private HealthSetter _playerHealthSetter;
        [SerializeField] [Range(0.1f, 0.9f)] private float _playerLowHealthLimit;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerShooter _playerShooter;
        [SerializeField] private Transform _player;

        [Header("Enemy Controls")]
        [SerializeField] private BoxCollider2D _enemyDetectorCollider;
        [SerializeField] private Vector2 _enemyInitialColliderSize;
        [SerializeField] private Vector2 _enemyFinalColliderSize;
        [SerializeField] private Transform _enemyLeftMovementTransformBoundary;
        [SerializeField] private Transform _enemyRightMovementTransformBoundary;
        [SerializeField] private Vector2 _leftPointInitialLocalPosition;
        [SerializeField] private Vector2 _rightPointInitialLocalPosition;
        [SerializeField] private Vector2 _leftPointFinalLocalPosition;
        [SerializeField] private Vector2 _rightPointFinalLocalPosition;

        [Header("Fireflies")]
        [SerializeField] private int _fireflySpawnCount = 5;
        [SerializeField] private CollisionNotifier _firefliesExitNotifier;

        private List<int> _fireflyIndices;

        #region Unity Functions

        private void Start()
        {
            _fireflyIndices = new List<int>();
            _fallingStone.isKinematic = true;

            _firefliesExitNotifier.OnTriggerEntered += HandlePlayerEnteredFireflyExit;
            _bossSceneActivator.OnTriggerEntered += ActivateBossScene;

            MakeEnemyRestricted();
        }

        #endregion

        #region Utility Functions

        private void MakeEnemyRestricted()
        {
            _enemyDetectorCollider.size = _enemyInitialColliderSize;
            _enemyLeftMovementTransformBoundary.localPosition = _leftPointInitialLocalPosition;
            _enemyRightMovementTransformBoundary.localPosition = _rightPointInitialLocalPosition;
        }

        private void RestoreOriginalEnemyValues()
        {
            _enemyDetectorCollider.size = _enemyFinalColliderSize;
            _enemyLeftMovementTransformBoundary.localPosition = _leftPointFinalLocalPosition;
            _enemyRightMovementTransformBoundary.localPosition = _rightPointFinalLocalPosition;
        }

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

            _rightBoundaryLocker.MakeChildrenExplode();
            _playerController.PlayerLoseBag();
            Destroy(_bossEnemy);

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
            SimpleSpeechController.Instance.DisplayDialogues(_dialogues);
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
                _fallingStone.isKinematic = false;
                _fallingStone.mass = _fallingStoneMass;

                RestoreOriginalEnemyValues();

                _playerHealthSetter.OnHealthChanged += HandlePlayerHealthChange;
                _bossSceneActivator.OnTriggerEntered -= ActivateBossScene;
            }
        }

        private void ActivateCutSceneSequence()
        {
            _playerMovement.DisableMovement();
            _playerShooter.DisableShooting();

            CutSceneDisplay.Instance.DisplayCutScene(_cutSceneImage, _cutSceneDisplayTime);

            CutSceneDisplay.Instance.OnCutSceneOpen += HandleCutSceneOpen;
            CutSceneDisplay.Instance.OnCutSceneClose += HandleCutSceneClose;
        }

        #endregion
    }
}