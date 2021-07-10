using UnityEngine;
using SDS.DialogueSystem.SO;

// Simple dialogue event for fighting
namespace jbzdy.DialogueSystem.Events
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Dialogue/New Fight Event", fileName = "Fight Event")]
    public class EventFight : DialogueEventSO
    {
        public override void RunEvent()
        {
            base.RunEvent();
            Fight();
        }

        private void Fight()
        {
            Debug.Log("Im gonna beat you up!");
        }
    }
}
