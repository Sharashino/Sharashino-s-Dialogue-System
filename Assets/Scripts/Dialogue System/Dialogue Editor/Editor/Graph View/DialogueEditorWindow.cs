using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using SDS.DialogueSystem.SO;
using SDS.DialogueSystem.Enums;
using SDS.DialogueSystem.SaveLoad;

// Script responsible for displaying editor window with dialogue editor
namespace SDS.DialogueSystem.Editor
{
    public class DialogueEditorWindow : EditorWindow
    {
        private DialogueContainerSO currentDialogueContainer;   // Dialogue that is getting loaded
        private DialogueGraphView graphView;    // Graph view displayed in editor
        private DialogueSaveAndLoad saveAndLoad;    // System saving and loading our dialogue

        private LanguageType languageType = LanguageType.English;    // Dialogue language type
        private ToolbarMenu toolbarMenu;    // This editor toolbar
        private Label nameOfDialougeContainer;  // Name of this dialogue

        public LanguageType LanguageType { get => languageType; set => languageType = value; }

        [OnOpenAsset(1)]
        // Loading and showing editor window
        public static bool ShowWindow(int instanceId, int line)
        {
            UnityEngine.Object item = EditorUtility.InstanceIDToObject(instanceId);

            if (item is DialogueContainerSO)
            {
                DialogueEditorWindow window = (DialogueEditorWindow)GetWindow(typeof(DialogueEditorWindow));
                window.titleContent = new GUIContent("Dialogue Editor");
                window.currentDialogueContainer = item as DialogueContainerSO;
                window.minSize = new Vector2(500, 250);
                window.Load();
            }
            return false;
        }

        private void OnEnable()
        {
            ConstructGraphView();   
            GenerateToolbar();
            Load();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
        }
        
        // Spawning graph view based on dialogue we're loading
        private void ConstructGraphView()
        {
            graphView = new DialogueGraphView(this);
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);

            saveAndLoad = new DialogueSaveAndLoad(graphView);
        }
        
        // Generating toolbar
        private void GenerateToolbar()
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("GraphViewStyleSheet");
            rootVisualElement.styleSheets.Add(styleSheet);

            Toolbar toolbar = new Toolbar();

            // Save button
            Button saveBtn = new Button()
            {
                text = "Save"
            };
            saveBtn.clicked += () =>
            {
                Save();
            };
            toolbar.Add(saveBtn);

            // Load button
            Button loadBtn = new Button()
            {
                text = "Load"
            };
            loadBtn.clicked += () =>
            {
                Load();
            };
            toolbar.Add(loadBtn);

            // Dropdown menu for languages
            toolbarMenu = new ToolbarMenu();
            foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
            {
                toolbarMenu.menu.AppendAction(language.ToString(), new Action<DropdownMenuAction>(x => Language(language, toolbarMenu)));
            }
            toolbar.Add(toolbarMenu);

            // Name of current DialogueContainer you have open
            nameOfDialougeContainer = new Label("");
            toolbar.Add(nameOfDialougeContainer);
            nameOfDialougeContainer.AddToClassList("nameOfDialogueContainer");

            rootVisualElement.Add(toolbar);
        }
        
        // Saving our dialogue
        private void Save()
        {
            if (currentDialogueContainer != null)
            {
                saveAndLoad.Save(currentDialogueContainer);
            }
        }
        
        // Loading dialogue
        private void Load()
        {
            if (currentDialogueContainer != null)
            {
                Language(LanguageType.Polish, toolbarMenu);
                nameOfDialougeContainer.text = "Name:   " + currentDialogueContainer.name;
                saveAndLoad.Load(currentDialogueContainer);
            }
        }
        
        // Dialogue language picker
        private void Language(LanguageType language, ToolbarMenu _toolbarMenu)
        {
            toolbarMenu.text = "Language: " + language.ToString();
            languageType = language;
            graphView.LanguageReload();
        }
    }
}
