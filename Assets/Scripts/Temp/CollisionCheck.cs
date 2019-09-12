using UnityEngine;

namespace Temp
{
    public class CollisionCheck : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Object Collided");
        }
    }
}