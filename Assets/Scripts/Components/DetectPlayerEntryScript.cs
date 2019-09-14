using System.Collections;
using System.Collections.Generic;
using CustomCamera;
using Interactibles.Teleporters;
using UnityEngine;
using WorldDisplay;

public class DetectPlayerEntryScript : MonoBehaviour
{
    [SerializeField] private FinalBoss finalBoss;
    [SerializeField] private Teleporter _teleporter;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Vector3 _offset;

    private bool battleStartedAlready = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        {
            var playerCol = collision.GetComponent<Player.Movement.PlayerCollision>();
            if (playerCol)
            {
                finalBoss.playerCol = playerCol;
                finalBoss.StartBossBattle();

                GetComponent<Collider2D>().enabled = false;

                _cameraController.UpdatePlayerOffset(_offset);
                _teleporter.DisableTeleporter();
            }
        }
    }

}
