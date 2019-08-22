using UnityEngine;

namespace Utils
{
    public static class RigidBodyExtensions
    {
        public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
        {
            var dir = (body.transform.position - explosionPosition);
            float wearoff = 1 - (dir.magnitude / explosionRadius);
            body.AddForce(wearoff * explosionForce * dir.normalized, ForceMode2D.Impulse);
        }

        public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upliftModifier)
        {
            var dir = (body.transform.position - explosionPosition);
            float wearOff = 1 - (dir.magnitude / explosionRadius);
            Vector3 baseForce = wearOff * explosionForce * dir.normalized;
            body.AddForce(baseForce);

            float upliftWearOff = 1 - upliftModifier / explosionRadius;
            Vector3 upliftForce = upliftWearOff * explosionForce * Vector2.up;
            body.AddForce(upliftForce);
        }
    }
}