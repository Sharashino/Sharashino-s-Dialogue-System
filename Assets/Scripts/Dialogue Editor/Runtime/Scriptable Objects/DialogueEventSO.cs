using UnityEngine;

/// <summary>
/// Napisane przez sharashino
/// 
/// Bazowy skrypt do tworzenia eventów w dialogach
/// </summary>
namespace SDS.DialogueSystem.SO
{
    [System.Serializable]
    public class DialogueEventSO : ScriptableObject
    {
        public virtual void RunEvent()
        {

        }
    }
}