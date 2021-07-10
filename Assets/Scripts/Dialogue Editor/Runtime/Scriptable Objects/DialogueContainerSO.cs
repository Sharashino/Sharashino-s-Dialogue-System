using SDS.Items;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using SDS.DialogueSystem.Enums;
using UnityEditor.Experimental.GraphView;

// ScriptableObject containing whole dialogue
namespace SDS.DialogueSystem.SO
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Dialogue/New Dialogue")]
    public class DialogueContainerSO : ScriptableObject
    {
        public List<NodeLinkData> NodeLinkDatas = new List<NodeLinkData>(); // Node Connections
        public List<DialogueNodeData> DialogueNodeDatas = new List<DialogueNodeData>(); // DialogueNodes in dialogue
        public List<EndNodeData> EndNodeDatas = new List<EndNodeData>();    // EndNodes in dialogue
        public List<StartNodeData> StartNodeDatas = new List<StartNodeData>();  // StartNode in dialogue
        public List<EventNodeData> EventNodeDatas = new List<EventNodeData>();  // EventNodes in dialogue
        public List<StatCheckNodeData> StatCheckNodeDatas = new List<StatCheckNodeData>();  // StatCheckNodes in dialogue 
        public List<ItemCheckNodeData> ItemCheckNodeDatas = new List<ItemCheckNodeData>();  // ItemCheckNodes in dialogue
        
        // All nodes in dialogue
        public List<BaseNodeData> AllNodes
        {
            get
            {
                List<BaseNodeData> allNodes = new List<BaseNodeData>();
                allNodes.AddRange(DialogueNodeDatas);
                allNodes.AddRange(EndNodeDatas);
                allNodes.AddRange(StartNodeDatas);
                allNodes.AddRange(EventNodeDatas);
                allNodes.AddRange(StatCheckNodeDatas);
                allNodes.AddRange(ItemCheckNodeDatas);

                return allNodes;
            }
        }
    }

    [System.Serializable]
    public class NodeLinkData   // Node connections data
    {
        public string BaseNodeGuid;
        public string TargetNodeGuid;
    }

    [System.Serializable]
    public class BaseNodeData   // Data in BaseNode
    {
        public string NodeGuid;
        public Vector2 Position;
    }

    [System.Serializable]
    public class DialogueNodeData : BaseNodeData    // Data in DialogueNode
    {
        public List<DialogueNodePort> DialogueNodePorts;
        public List<LanguageGeneric<AudioClip>> AudioClips;
        public List<LanguageGeneric<string>> TextLanguages;
        public Sprite npcSprite;
        public Sprite playerSprite;
        public string Name;
    }

    [System.Serializable]
    public class EndNodeData : BaseNodeData // Data in EndNode
    {
        public EndNodeType EndNodeType;
    }

    [System.Serializable]
    public class StartNodeData : BaseNodeData   // Data in StartNode
    {

    }

    [System.Serializable]
    public class EventNodeData : BaseNodeData   // Data in EventNode
    {
        public DialogueEventSO DialogueEventSO;
    }

    [System.Serializable]
    public class ItemCheckNodeData : BaseNodeData   // Data in ItemCheckNode
    {
        public ItemCheckNodeType ItemCheckType;
        public int ItemCheckValue;
        public Item NodeItem;
    }

    [System.Serializable]
    public class StatCheckNodeData : BaseNodeData   // Data in StatCheckNode 
    {
        public StatCheckType StatCheckType;
        public int StatCheckValue;
    }

    [System.Serializable]
    public class LanguageGeneric<T> // Data of Language 
    {
        public LanguageType LanguageType;
        public T LanguageGenericType;
    }

    [System.Serializable]
    public class DialogueNodePort   // Dialogue choices data
    {
        public string PortGuid;
        public string InputGuid;
        public string OutputGuid;
        public Port MyPort;
        public TextField TextField;
        public List<LanguageGeneric<string>> TextLanguages = new List<LanguageGeneric<string>>();
    }
}
