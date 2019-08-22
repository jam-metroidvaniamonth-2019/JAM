using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace WorldDisplay
{
    public class TriggerExplosionOnChildren : MonoBehaviour
    {
        [Header("Explosion")]
        [SerializeField] private float _minExplosionForce;
        [SerializeField] private float _maxExplosionForce;
        [SerializeField] private Vector3 _explosionOffset;
        [SerializeField] private float _explosionRadius;

        [Header("Children Display Control")]
        [SerializeField] private float _alphaReductionRate;

        private List<Rigidbody2D> _explodingChildren;
        private List<SpriteRenderer> _childrenSprites;
        private Vector3 _explosionPoint;

        private bool _explosionStarted;

        #region Unity Functions

        private void Start()
        {
            _explodingChildren = GetComponentsInChildren<Rigidbody2D>().ToList();
            _childrenSprites = GetComponentsInChildren<SpriteRenderer>().ToList();
            _explosionPoint = transform.position + _explosionOffset;
        }

        private void Update()
        {
            if (_explosionStarted)
            {
                CountdownAlphaZero();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(_explosionPoint, _explosionRadius);
        }

        #endregion

        #region Utility Functions

        private void CountdownAlphaZero()
        {
            bool allAlphaZero = true;
            foreach (SpriteRenderer childrenSprite in _childrenSprites)
            {
                Color color = childrenSprite.color;
                color.a -= _alphaReductionRate * Time.deltaTime;
                childrenSprite.color = color;

                if (color.a > 0)
                {
                    allAlphaZero = false;
                }
            }

            if (allAlphaZero)
            {
                ResetAndDisableObject();
            }
        }

        private void ResetAndDisableObject()
        {
            _explosionStarted = false;
            foreach (SpriteRenderer childrenSprite in _childrenSprites)
            {
                Color color = childrenSprite.color;
                color.a = 1;
                childrenSprite.color = color;
            }

            gameObject.SetActive(false);
        }

        #endregion

        #region External Functions

        public void MakeChildrenExplode()
        {
            if (_explosionStarted)
            {
                return;
            }

            foreach (Rigidbody2D explodingChild in _explodingChildren)
            {
                explodingChild.bodyType = RigidbodyType2D.Dynamic;
                explodingChild.AddExplosionForce(Random.Range(_minExplosionForce, _maxExplosionForce), _explosionPoint, _explosionRadius);
            }

            _explosionStarted = true;
        }

        #endregion
    }
}