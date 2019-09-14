using System;
using SpeechSystem;
using UnityEngine;

namespace Scenes.Main.Enemy
{
    public class EnemyDeathBagDrop : EnemyDeathController
    {
        [SerializeField] private Rigidbody2D _bag;
        [SerializeField] [TextArea] private string[] _dialogues;

        protected override void RunEffectOnZero()
        {
            _bag.isKinematic = false;
            _bag.transform.SetParent(null);

            SimpleSpeechController.Instance.DisplayDialogues(_dialogues);

            base.RunEffectOnZero();
        }
    }
}