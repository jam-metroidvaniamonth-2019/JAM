using CustomCamera;
using ScriptableObjects;
using UnityEngine;

namespace Temp
{
    public class CameraShakerTest : MonoBehaviour
    {
        [SerializeField] private CameraShakeModifiers _cameraShakeModifiers;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                CameraShaker.Instance.StartShaking(_cameraShakeModifiers);
            }
        }
    }
}