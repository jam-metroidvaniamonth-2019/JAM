using System;
using Cinemachine;
using ScriptableObjects;
using UnityEngine;

namespace CustomCamera
{
    public class CameraShaker : MonoBehaviour
    {
        [Header("CameraTarget")]
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        private CinemachineBasicMultiChannelPerlin _cinemachineNoise;

        private bool _isShakeActive;
        private float _shakeTimer;

        #region Unity Functions

        private void Start() => _cinemachineNoise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        private void Update()
        {
            if (!_isShakeActive)
            {
                return;
            }

            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0)
            {
                StopShaking();
            }
        }

        #endregion

        #region External Functions

        public void StartShaking(CameraShakeModifiers cameraShakeModifiers)
        {
            _isShakeActive = true;
            _shakeTimer = cameraShakeModifiers.shakeTimer;

            _cinemachineNoise.m_AmplitudeGain = cameraShakeModifiers.shakeAmplitude;
            _cinemachineNoise.m_FrequencyGain = cameraShakeModifiers.shakeFrequency;
        }

        public void StopShaking()
        {
            _isShakeActive = false;
            _cinemachineNoise.m_AmplitudeGain = 0;
            _cinemachineNoise.m_FrequencyGain = 0;
        }

        #endregion

        #region Singleton


        private static CameraShaker _instance;
        public static CameraShaker Instance => _instance;

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