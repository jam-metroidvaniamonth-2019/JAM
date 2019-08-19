namespace JamSpace
{
    public enum EEnemyState
    {
        NONE,

        // This is the state of the enemy when its unreachable by the player 
        // and is thus inactive to optimize on performace
        Resting,

        // This is the inital state of the enemy
        // that is when the enemy is guaring the platform or
        // waiting for player to get around this platform
        Patroling,

        // this is the state of the enemy when it senses the player presence
        // and is trying to find a way to attack the player
        Investigate,

        // this is the state of the enemy when it is charging or attacking player
        Charging,

        // this is the state of the enemy when it has finished its attack
        // and will have to wait for certain amount of time before
        // making another attempt at attacking the enemy
        Cooldown,

        MAX,
    }



}