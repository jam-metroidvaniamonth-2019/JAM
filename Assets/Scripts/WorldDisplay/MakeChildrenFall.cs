using System;
using System.Collections;
using UnityEngine;

namespace WorldDisplay
{
    public class MakeChildrenFall : MonoBehaviour
    {
        [SerializeField] private float _downwardInitialForce;
        [SerializeField] private bool _triggerOnCollide;
        [SerializeField] private float _destroyChildrenAfterTime;
        [SerializeField] private GameObject _destroyEffect;
        [SerializeField] private Rigidbody2D[] _children;

        private bool _initialized;

        #region Unity Functions

        private void Start()
        {
            Initialize();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_triggerOnCollide)
            {
                ThrowChildrenWithForce();
            }
        }

        #endregion

        #region External Functions

        public void ThrowChildrenWithForce()
        {
            Initialize();

            foreach (Rigidbody2D child in _children)
            {
                child.isKinematic = false;
                child.AddForce(Vector2.down * _downwardInitialForce, ForceMode2D.Impulse);
            }

            StartCoroutine(DelayedDestroyChildren());
        }

        #endregion

        #region Utility Functions

        private IEnumerator DelayedDestroyChildren()
        {
            yield return new WaitForSeconds(_destroyChildrenAfterTime);

            foreach (Rigidbody2D child in _children)
            {
                Vector3 destroyEffectPosition = child.transform.position;
                Instantiate(_destroyEffect, destroyEffectPosition, Quaternion.identity);

                Destroy(child.gameObject);
            }

            Destroy(gameObject);
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            foreach (Rigidbody2D child in _children)
            {
                child.isKinematic = true;
            }

            _initialized = true;
        }

        #endregion
    }
}