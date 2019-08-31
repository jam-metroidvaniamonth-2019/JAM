using UnityEngine;

namespace SaveSystem
{
    [System.Serializable]
    public class SaveStructure
    {
        // Player Position
        public float playerXPosition;
        public float playerYPosition;

        // Boss Battles
        public bool firstBossFightCompleted;
        public bool finalBossFightCompleted;
        public bool bridgeBroken;

        // Firefly
        public bool fireflySceneCompleted;
    }
}