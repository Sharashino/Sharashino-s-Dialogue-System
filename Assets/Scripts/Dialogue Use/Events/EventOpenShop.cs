using SDS.DialogueSystem.SO;
using UnityEngine;

/// <summary>
/// Napisane przez sharashino
/// 
/// Event w dialogu odpowiadający za otwarcie sklepu
/// </summary>
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
            Debug.Log("We coś kup byczq");
        }
    }
}

