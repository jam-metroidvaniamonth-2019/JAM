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


        // Boss Battle
        public bool firstBossBattleCompleted;

        // Bag Enemy
        public bool bagDroppedEnemy;

        // Collectibles
        public bool bowCollected;
        public bool dashCollected;
    }
}