using UnityEngine;
using SDS.DialogueSystem.SO;

// Simple dialogue event for getting quest
namespace SDS.DialogueSystem.Events
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Dialogue/New Quest Event", fileName = "Quest Event")]
    public class EventGetQuest : DialogueEventSO
    {
        public string questName;

        public override void RunEvent()
        {
            base.RunEvent();
            GetQuest();
        }

        private void GetQuest()
        {
            Debug.Log("New quest:" +questName);
        }
    }
}
