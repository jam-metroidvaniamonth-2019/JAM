using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "CameraShakeModifier", menuName = "Camera/ShakeModifier", order = 0)]
    public class CameraShakeModifiers : ScriptableObject
    {
        public float shakeMultiplier;
        public float shakeTimer;
    }
}