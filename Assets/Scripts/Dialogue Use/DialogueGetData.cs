using UnityEngine;
using SDS.DialogueSystem.SO;

/// <summary>
/// Napisane przez sharashino
/// 
/// Skrypt pobierający dane z ScriptableObject'a z dialogiem
/// </summary>
public class DialogueGetData : MonoBehaviour
{
    [SerializeField] protected DialogueContainerSO currentDialogue;

    protected BaseNodeData GetNodeByGuid(string targetNodeGuid)
    {
        return currentDialogue.AllNodes.Find(node => node.NodeGuid == targetNodeGuid);
    }

    protected BaseNodeData GetNodeByNodePort(DialogueNodePort nodePort)
    {
        return currentDialogue.AllNodes.Find(node => node.NodeGuid == nodePort.InputGuid);
    }

    protected BaseNodeData GetNextNode(BaseNodeData baseNodeData)
    {
        NodeLinkData nodeLinkData = currentDialogue.NodeLinkDatas.Find(egde => egde.BaseNodeGuid == baseNodeData.NodeGuid);

        return GetNodeByGuid(nodeLinkData.TargetNodeGuid);
    }
}
