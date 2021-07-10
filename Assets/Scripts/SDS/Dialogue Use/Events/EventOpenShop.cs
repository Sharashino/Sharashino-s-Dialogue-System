using SDS.DialogueSystem.SO;
using UnityEngine;

// Simple dialogue event for entering the shop
namespace SDS.DialogueSystem.Events
{
    [System.Serializable]
    [CreateAssetMenu(menuName ="Dialogue/New OpenShop Event", fileName = "OpenShop Event")]
    public class EventOpenShop : DialogueEventSO
    {
        public override void RunEvent()
        {
            base.RunEvent();
            OpenShop();
        }

        private void OpenShop()
        {
            Debug.Log("Buy yourself something nice!");
        }
    }
}

