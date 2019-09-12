using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerEntryScript : MonoBehaviour
{
    [SerializeField]
    private FinalBoss finalBoss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var playerCol = collision.GetComponent<Player.Movement.PlayerCollision>();
        if (playerCol)
        {
            finalBoss.playerCol = playerCol;
            // add everything on detecting player by boss
            // later call finalBoss in action
            finalBoss.StartBossBattle();

        }
    }

}
