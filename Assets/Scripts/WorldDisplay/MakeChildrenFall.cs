using System;
using System.Collections;
using UnityEngine;

namespace WorldDisplay
{
    public class MakeChildrenFall : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D[] _children;
        [SerializeField] private float _downwardInitialForce;
        [SerializeField] private bool _triggerOnCollide;
        [SerializeField] private float _destroyChildrenAfterTime;
        [SerializeField] private GameObject _destroyEffect;

        #region Unity Functions

        private void Start()
        {
            foreach (Rigidbody2D child in _children)
            {
                child.isKinematic = true;
            }
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

        #endregion
    }
}