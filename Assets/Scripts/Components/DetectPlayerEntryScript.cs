using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerEntryScript : MonoBehaviour
{
    [SerializeField]
    private FinalBoss finalBoss;

    private bool battleStartedAlready = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        {
            var playerCol = collision.GetComponent<Player.Movement.PlayerCollision>();
            if (playerCol)
            {
                finalBoss.playerCol = playerCol;
                finalBoss.StartBossBattle();
                this.GetComponent<Collider2D>().enabled = false;
            }
        }
    }

}
