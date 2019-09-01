namespace JamSpace
{


    public class AnimationTags
    {
        public static string ANIMATION_INVESTIGATE = "ANIMATION_INVESTIGATE";
        public static string ANIMATION_IDLE = "ANIMATION_IDLE";
        public static string ANIMATION_CHARGE = "ANIMATION_CHARGE";
        public static string ANIMATION_BITE = "ANIMATION_BITE";
        public static string ANIMATION_COOLDOWN = "ANIMATION_COOLDOWN";
    }


    [System.Serializable]
    public enum EState
    {
        NONE,

        IDLE,
        MONITORING,
        ATTACKING,
        COOLDOWN,

        MAX,
    }


    [System.Serializable]
    public struct StructEnemyStats
    {
        public float damageValue;
        public float cooldownValue;
        public EEnemyAttackType enemyAttackType;
    }

    public enum EEnemyAttackType
    {
        NONE,
        Ground_Leaping,
        GrundC_harging,
        Aerial_Droping,
        Aerial_Charging,
        MAX,
    }

    [System.Serializable]
    public enum EEnemyState
    {
        NONE,

        Idle,
        Investigation,
        Charging,
        Bite,
        Cooldown_B,

        MAX,
    }



}