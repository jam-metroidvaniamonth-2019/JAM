using UnityEngine;

namespace SaveSystem
{
    [System.Serializable]
    public class SaveStructure
    {
        // Player Data
        public float playerXPosition;
        public float playerYPosition;
        public float playerHealth;

        // Boss Battles
        public bool firstBossFightCompleted;
        public bool finalBossFightCompleted;

        // Firefly
        public bool fireflySceneCompleted;
    }
}