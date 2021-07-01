using System;
using System.Linq;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using SDS.DialogueSystem.SO;
using System.Collections.Generic;
using SDS.DialogueSystem.Enums;
using SDS.DialogueSystem.Editor;
using UnityEditor.Experimental.GraphView;

namespace SDS.DialogueSystem.Nodes
{
    public class DialogueNode : BaseNode
    {
        private List<LanguageGeneric<string>> texts = new List<LanguageGeneric<string>>();
        private List<LanguageGeneric<AudioClip>> audioClips = new List<LanguageGeneric<AudioClip>>();
        private Sprite npcFaceImage;
        private Sprite playerFaceImage;
        private string nameText = "";

        private List<DialogueNodePort> dialogueNodePorts = new List<DialogueNodePort>();
        public List<LanguageGeneric<string>> Texts { get => texts; set => texts = value; }
        public List<LanguageGeneric<AudioClip>> AudioClips { get => audioClips; set => audioClips = value; }
        public Sprite NpcFaceImage { get => npcFaceImage; set => npcFaceImage = value; }
        public Sprite PlayerFaceImage { get => playerFaceImage; set => playerFaceImage = value; }
        public string NameText { get => nameText; set => nameText = value; }
        public List<DialogueNodePort> DialogueNodePorts { get => dialogueNodePorts; set => dialogueNodePorts = value; }

        private TextField textsField;
        private ObjectField audioClipsField;
        private ObjectField npcImageField;
        private ObjectField playerImageField;
        private TextField nameField;

        public DialogueNode()
        {
            
        }

        public DialogueNode(Vector2 position, DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("DialogueNodeStyleSheet");
            styleSheets.Add(styleSheet);

            editorWindow = newEditorWindow;
            graphView = newGraphView;

            title = "Dialogue";
            SetPosition(new Rect(position, defaultNodeSize));
            nodeGuid = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);

            foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
            {
                texts.Add(new LanguageGeneric<string>
                {
                    LanguageType = language,
                    LanguageGenericType = ""
                });

                audioClips.Add(new LanguageGeneric<AudioClip>
                {
                    LanguageType = language,
                    LanguageGenericType = null
                });
            }

            //Enemy Face Image
            npcImageField = new ObjectField
            {
                label = "NPC image: ",
                objectType = typeof(Sprite),
                allowSceneObjects = false,
                value = npcFaceImage
            };
            npcImageField.RegisterValueChangedCallback(value =>
            {
                npcFaceImage = value.newValue as Sprite;
            });            
            
            mainContainer.Add(npcImageField);
            
            playerImageField = new ObjectField
            {
                label = "Player image: ",
                objectType = typeof(Sprite),
                allowSceneObjects = false,
                value = playerFaceImage
            };
            playerImageField.RegisterValueChangedCallback(value =>
            {
                playerFaceImage = value.newValue as Sprite;
            });   
            
            mainContainer.Add(playerImageField);

            // Audio Chilp
            audioClipsField = new ObjectField()
            {
                objectType = typeof(AudioClip),
                allowSceneObjects = false,
                value = audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType,
            };
            audioClipsField.RegisterValueChangedCallback(value =>
            {
                audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue as AudioClip;
            });
            audioClipsField.SetValueWithoutNotify(audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType);
            mainContainer.Add(audioClipsField);

            // Text Name
            Label label_name = new Label("Name");
            label_name.AddToClassList("label_name");
            label_name.AddToClassList("Label");
            mainContainer.Add(label_name);

            nameField = new TextField("");
            nameField.RegisterValueChangedCallback(value =>
            {
                nameText = value.newValue;
            });
            nameField.SetValueWithoutNotify(nameText);
            nameField.AddToClassList("TextName");
            mainContainer.Add(nameField);

            // Text Box
            Label label_texts = new Label("Text Box");
            label_texts.AddToClassList("label_texts");
            label_texts.AddToClassList("Label");
            mainContainer.Add(label_texts);

            textsField = new TextField("");
            textsField.RegisterValueChangedCallback(value =>
            {
                texts.Find(text => text.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue;
            });
            textsField.SetValueWithoutNotify(texts.Find(text => text.LanguageType == editorWindow.LanguageType).LanguageGenericType);
            textsField.multiline = true;

            textsField.AddToClassList("TextBox");
            mainContainer.Add(textsField);

            Button button = new Button()
            {
                text = "Add Choice"
            };
            button.clicked += () =>
            {
                AddChoicePort(this);
            };

            titleButtonContainer.Add(button);
        }

        public void ReloadLanguage()
        {
            textsField.RegisterValueChangedCallback(value =>
            {
                texts.Find(text => text.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue;
            });
            textsField.SetValueWithoutNotify(texts.Find(text => text.LanguageType == editorWindow.LanguageType).LanguageGenericType);

            audioClipsField.RegisterValueChangedCallback(value =>
            {
                audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue as AudioClip;
            });
            audioClipsField.SetValueWithoutNotify(audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType);

            foreach (DialogueNodePort nodePort in dialogueNodePorts)
            {
                nodePort.TextField.RegisterValueChangedCallback(value =>
                {
                    nodePort.TextLanguages.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue;
                });
                nodePort.TextField.SetValueWithoutNotify(nodePort.TextLanguages.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType);
            }
        }

        public override void LoadValueInToField()
        {
            textsField.SetValueWithoutNotify(texts.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType);
            audioClipsField.SetValueWithoutNotify(audioClips.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType);
            npcImageField.SetValueWithoutNotify(npcFaceImage);
            playerImageField.SetValueWithoutNotify(playerFaceImage);
            nameField.SetValueWithoutNotify(nameText);
        }

        public Port AddChoicePort(BaseNode newBaseNode, DialogueNodePort newDialogueNodePort = null)
        {
            Port port = GetPortInstance(Direction.Output);

            int outputPortCount = newBaseNode.outputContainer.Query("connector").ToList().Count();
            string outputPortName = $"Continue";

            DialogueNodePort dialogueNodePort = new DialogueNodePort();
            dialogueNodePort.PortGuid = Guid.NewGuid().ToString();

            foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
            {
                dialogueNodePort.TextLanguages.Add(new LanguageGeneric<string>()
                {
                    LanguageType = language,
                    LanguageGenericType = outputPortName
                });
            }

            if (newDialogueNodePort != null)
            {
                dialogueNodePort.InputGuid = newDialogueNodePort.InputGuid;
                dialogueNodePort.OutputGuid = newDialogueNodePort.OutputGuid;
                dialogueNodePort.PortGuid = newDialogueNodePort.PortGuid;

                foreach (LanguageGeneric<string> languageGeneric in newDialogueNodePort.TextLanguages)
                {
                    dialogueNodePort.TextLanguages.Find(language => language.LanguageType == languageGeneric.LanguageType).LanguageGenericType = languageGeneric.LanguageGenericType;
                }
            }

            // Text for the port
            dialogueNodePort.TextField = new TextField();
            dialogueNodePort.TextField.RegisterValueChangedCallback(value =>
            {
                dialogueNodePort.TextLanguages.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue;
            });
            dialogueNodePort.TextField.SetValueWithoutNotify(dialogueNodePort.TextLanguages.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType);
            port.contentContainer.Add(dialogueNodePort.TextField);

            // Delete button
            Button deleteButton = new Button(() => DeletePort(newBaseNode, port))
            {
                text = "X",
            };
            port.contentContainer.Add(deleteButton);

            dialogueNodePort.MyPort = port;
            port.portName = "";

            dialogueNodePorts.Add(dialogueNodePort);

            newBaseNode.outputContainer.Add(port);

            // Refresh
            newBaseNode.RefreshPorts();
            newBaseNode.RefreshExpandedState();

            return port;
        }


        private void DeletePort(BaseNode delNode, Port delPort)
        {
            DialogueNodePort tmp = dialogueNodePorts.Find(port => port.MyPort == delPort);
            dialogueNodePorts.Remove(tmp);

            IEnumerable<Edge> portEdge = graphView.edges.ToList().Where(edge => edge.output == delPort);

            if (portEdge.Any())
            {
                Edge edge = portEdge.First();
                edge.input.Disconnect(edge);
                edge.output.Disconnect(edge);
                graphView.RemoveElement(edge);
            }

            delNode.outputContainer.Remove(delPort);

            delNode.RefreshPorts();
            delNode.RefreshExpandedState();
        }
    }
}

