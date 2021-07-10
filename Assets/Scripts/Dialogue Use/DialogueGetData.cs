using UnityEngine;
using SDS.DialogueSystem.SO;

// Script fetching data from ScriptableObject with dialogue
public class DialogueGetData : MonoBehaviour
{
    [SerializeField] protected DialogueContainerSO currentDialogue;

    // Getting node by its GUID
    protected BaseNodeData GetNodeByGuid(string targetNodeGuid)
    {
        return currentDialogue.AllNodes.Find(node => node.NodeGuid == targetNodeGuid);
    }
    
    // Getting node by its port
    protected BaseNodeData GetNodeByNodePort(DialogueNodePort nodePort)
    {
        return currentDialogue.AllNodes.Find(node => node.NodeGuid == nodePort.InputGuid);
    }
    
    // Getting next node by its link data
    protected BaseNodeData GetNextNode(BaseNodeData baseNodeData)
    {
        NodeLinkData nodeLinkData = currentDialogue.NodeLinkDatas.Find(egde => egde.BaseNodeGuid == baseNodeData.NodeGuid);

        return GetNodeByGuid(nodeLinkData.TargetNodeGuid);
    }
}
