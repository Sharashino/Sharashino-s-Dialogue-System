using UnityEngine;

// Main script for creating events in EventNode
namespace SDS.DialogueSystem.SO
{
    [System.Serializable]
    public class DialogueEventSO : ScriptableObject
    {
        // Overwritable void running event in EventNode 
        public virtual void RunEvent()
        {
        
        }
    }
}