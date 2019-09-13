using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Interactibles.Followers
{
    public partial class FireflyManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _fireflyPrefab;
        [SerializeField] private float _constantZPosition = 5;

        [Header("Control Points")]
        [SerializeField] private Transform _firefliesHolder;
        [SerializeField] private Transform[] _outOfScreenPoints;

        private List<FireflyInformation> _fireflies = new List<FireflyInformation>();
        private int _currentIndex;

        #region Unity Functions

        private void Update()
        {
            for (int i = _fireflies.Count - 1; i >= 0; i--)
            {
                FireflyInformation fireflyInformation = _fireflies[i];
                if (fireflyInformation.destroyOnReachTarget && fireflyInformation.fireflyInstance.TargetReachedPosition())
                {
                    DestroyFirefly(fireflyInformation.fireflyInstance.gameObject);
                    _fireflies.RemoveAt(i);
                }
            }
        }

        #endregion

        #region External Functions

        public int AddFirefly(bool destroyOnReachTarget, Vector3 spawnPoint, Transform target = null)
        {
            if (target == null)
            {
                target = GetRandomTransformPoint();
            }

            if (ExtensionFunctions.IsVectorZero(spawnPoint))
            {
                spawnPoint = GetRandomTransformPoint().position;
            }

            _currentIndex += 1;

            GameObject fireflyInstance = SpawnFirefly(spawnPoint);
            FloatyFollowTarget followTarget = fireflyInstance.GetComponent<FloatyFollowTarget>();
            FireflyInformation fireflyInformation = new FireflyInformation
            {
                destroyOnReachTarget = destroyOnReachTarget,
                fireflyIndex = _currentIndex,
                fireflyInstance = followTarget
            };

            followTarget.UpdateTarget(target);
            _fireflies.Add(fireflyInformation);

            return _currentIndex;
        }

        public void UpdateFireflyTargetToRandom(int fireflyIndex, bool destroyOnReach = false)
        {
            FireflyInformation fireflyInformation = _fireflies.FirstOrDefault(_ => _.fireflyIndex == fireflyIndex);
            if (fireflyInformation == null)
            {
                Debug.Log("Invalid Firefly Requested");
                return;
            }

            Transform randomTarget = GetRandomTransformPoint();
            fireflyInformation.destroyOnReachTarget = destroyOnReach;
            fireflyInformation.fireflyInstance.GetComponent<FloatyFollowTarget>().UpdateTarget(randomTarget);
        }

        public void UpdateFireflyTarget(Transform target, int fireflyIndex)
        {
            FireflyInformation fireflyInformation = _fireflies.FirstOrDefault(_ => _.fireflyIndex == fireflyIndex);
            if (fireflyInformation == null)
            {
                Debug.Log("Invalid Firefly Requested");
                return;
            }

            fireflyInformation.fireflyInstance.GetComponent<FloatyFollowTarget>().UpdateTarget(target);
        }

        #endregion

        #region Utility Functions

        private Transform GetRandomTransformPoint() => _outOfScreenPoints[Mathf.FloorToInt(Random.value * _outOfScreenPoints.Length)];

        private GameObject SpawnFirefly(Vector3 spawnPosition)
        {
            GameObject fireflyInstance = Instantiate(_fireflyPrefab, spawnPosition, Quaternion.identity);
            fireflyInstance.transform.SetParent(_firefliesHolder);

            return fireflyInstance;
        }

        private void DestroyFirefly(GameObject firefly) => Destroy(firefly);

        #endregion

        #region Singleton

        private static FireflyManager _instance;
        public static FireflyManager Instance => _instance;

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

        private class FireflyInformation
        {
            public bool destroyOnReachTarget;
            public int fireflyIndex;
            public FloatyFollowTarget fireflyInstance;
        }
    }
}