using UnityEngine;

namespace Common
{
    public abstract class VarUpdateMonoBehaviour : MonoBehaviour
    {
        private float _currentTimeScale = 1;

        #region Update

        private void Update()
        {
            float customDeltaTime = Time.deltaTime * _currentTimeScale;
            CustomUpdate(customDeltaTime);
        }

        protected virtual void CustomUpdate(float deltaTime) { }

        #endregion

        #region TimeScale Changes

        public void SetTimeScale(float timeScale) => _currentTimeScale = timeScale;

        #endregion
    }
}