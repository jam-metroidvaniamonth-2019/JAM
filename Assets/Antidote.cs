using System;
using SpeechSystem;
using UnityEngine;

public class Antidote : MonoBehaviour
{
    [SerializeField] [TextArea] private string[] _dialogues;

    private Collider2D antidoteCollider;

    // Use this delegate
    // for calling methods on obtaining antidote
    public delegate void AntidoteObtained();
    public AntidoteObtained OnObtainingAntidote;

    private void Start() => SimpleSpeechController.Instance.DisplayDialogues(_dialogues);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collider = collision.gameObject.GetComponent<Player.General.PlayerController>();
        if (collider)
        {
            collider.PlayerCollectAntidote();
            OnObtainingAntidote?.Invoke();

            Destroy(gameObject);
        }
    }
}