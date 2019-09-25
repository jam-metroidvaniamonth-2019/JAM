using SaveSystem;
using SpeechSystem;
using UnityEngine;

namespace Scenes.Main.Enemy
{
    public class EnemyDeathBagDrop : EnemyController
    {
        [Header("Enemy CutScene Data")]
        [SerializeField] private Rigidbody2D _bag;
        [SerializeField] [TextArea] private string[] _dialogues;

        [Header("Save")]
        [SerializeField] private Transform _safePosition;
        [SerializeField] private PlayerSaveHelper _playerSaveHelper;
        [SerializeField] private EnemyController[] _enemyControllers;

        protected override void RunEffectOnZero()
        {
            _bag.isKinematic = false;
            _bag.transform.SetParent(null);

            SimpleSpeechController.Instance.DisplayDialogues(_dialogues);

            base.RunEffectOnZero();

            _playerSaveHelper.SavePlayerWithSafePosition(_safePosition.position);
            SaveManager.Instance.SaveStructure.bagDroppedEnemy = true;
            SaveManager.Instance.SaveStructure.enemyControllers = _enemyControllers;
            SaveManager.Instance.SaveData();
        }
    }
}