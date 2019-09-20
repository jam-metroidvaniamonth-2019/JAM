using Common;
using Player.Display;
using Player.Movement;
using Player.Shooting;
using SaveSystem;
using Scenes.GameOver;
using Scenes.Main;
using UI;
using UnityEngine;

namespace Player.General
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Health")]
        [SerializeField] private HealthSetter _playerHealthSetter;
        [SerializeField] [Range(0, 1)] private float _lowHealthActivationRatio;
        [SerializeField] private ImageFader _lowHealthFader;

        [Header("Player Death")]
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerShooter _playerShooter;
        [SerializeField] private Fader _playerDeadFader;
        [SerializeField] private PlayerAnimator _playerNormalAnimator;
        [SerializeField] private PlayerAnimator _playerBagAnimator;

        public delegate void PlayerBagStatusChanged(bool playerHasBag);
        public PlayerBagStatusChanged OnPlayerBagStatusChanged;

        private bool _playerHasBag;
        private bool _playerHasBow;
        private bool _playerHasAntidote;
        public bool _playerHasDash;

        #region Unity Functions

        private void Start()
        {
            _playerHasBag = true;
            _playerHasDash = false;

            _playerHealthSetter.OnHealthChanged += HandleHealthChange;
            _playerHealthSetter.OnHealthZero += HandleHealthZero;
            _playerDeadFader.OnFadeOutComplete += HandlePlayerDeadFadeOut;

            _lowHealthFader.StopFading();
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

        public void PlayerCollectAntidote() => _playerHasAntidote = true;

        public bool PlayerHasBag => _playerHasBag;

        public bool PlayerHasBow => _playerHasBow;

        public bool PlayerHasDash => _playerHasDash;

        public bool PlayerHasAntidote => _playerHasAntidote;

        #endregion

        #region Utility Functions

        private void HandleHealthChange(float currentHealth, float maxHealth)
        {
            float healthRatio = currentHealth / maxHealth;
            if (healthRatio <= _lowHealthActivationRatio)
            {
                _lowHealthFader.StartFading();
            }
            else
            {
                _lowHealthFader.StopFading();
            }
        }

        private void HandleHealthZero()
        {
            bool gameSavedAtLeastOnce = SaveManager.Instance.GameSavedAtLeastOnce;
            if (gameSavedAtLeastOnce)
            {
                _playerDeadFader.StartFadeOut(true);
            }
            else
            {
                GameOverSceneData.didPlayerWin = false;
                MainSceneController.Instance.FadeAndSwitchScene();
            }
        }

        private void HandlePlayerDeadFadeOut()
        {
            float savedHealth = SaveManager.Instance.SaveStructure.playerHealth;
            _playerHealthSetter.SetCurrentHealth(savedHealth);

            Vector2 playerSafePosition = new Vector2(
                SaveManager.Instance.SaveStructure.playerXPosition,
                SaveManager.Instance.SaveStructure.playerYPosition
            );

            transform.position = playerSafePosition;

            _playerNormalAnimator.RevivePlayer();
            _playerBagAnimator.RevivePlayer();

            _playerDeadFader.StartFadeIn();

            _playerMovement.EnableMovement();
            _playerShooter.EnableShooting();
        }

        private void NotifyBagStatusChanged() => OnPlayerBagStatusChanged?.Invoke(_playerHasBag);

        #endregion
    }
}