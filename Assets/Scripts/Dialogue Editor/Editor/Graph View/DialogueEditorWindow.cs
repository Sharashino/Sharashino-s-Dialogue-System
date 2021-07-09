﻿using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using SDS.DialogueSystem.SO;
using SDS.DialogueSystem.Enums;
using SDS.DialogueSystem.SaveLoad;

// <summary>
// Napisane przez sharashino
// 
// Skrypt odpowiadający za wyświetlanie okna z edytorem dialogów
// </summary>
namespace SDS.DialogueSystem.Editor
{
    public class DialogueEditorWindow : EditorWindow
    {
        private DialogueContainerSO currentDialogueContainer;
        private DialogueGraphView graphView;
        private DialogueSaveAndLoad saveAndLoad;

        private LanguageType languageType = LanguageType.Polish;
        private ToolbarMenu toolbarMenu;
        private Label nameOfDialougeContainer;

        public LanguageType LanguageType { get => languageType; set => languageType = value; }

        [OnOpenAsset(1)]
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
            ConstructGeaphView();
            GenerateToolbar();
            Load();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
        }

        private void ConstructGeaphView()
        {
            graphView = new DialogueGraphView(this);
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);

            saveAndLoad = new DialogueSaveAndLoad(graphView);
        }

        private void GenerateToolbar()
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("GraphViewStyleSheet");
            rootVisualElement.styleSheets.Add(styleSheet);

            Toolbar toolbar = new Toolbar();

            // Save button.
            Button saveBtn = new Button()
            {
                text = "Save"
            };
            saveBtn.clicked += () =>
            {
                Save();
            };
            toolbar.Add(saveBtn);

            // Load button.
            Button loadBtn = new Button()
            {
                text = "Load"
            };
            loadBtn.clicked += () =>
            {
                Load();
            };
            toolbar.Add(loadBtn);

            // Dropdown menu for languages.
            toolbarMenu = new ToolbarMenu();
            foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
            {
                toolbarMenu.menu.AppendAction(language.ToString(), new Action<DropdownMenuAction>(x => Language(language, toolbarMenu)));
            }
            toolbar.Add(toolbarMenu);

            // Name of current DialigueContainer you have open.
            nameOfDialougeContainer = new Label("");
            toolbar.Add(nameOfDialougeContainer);
            nameOfDialougeContainer.AddToClassList("nameOfDialogueContainer");

            rootVisualElement.Add(toolbar);
        }
        
        private void Save()
        {
            if (currentDialogueContainer != null)
            {
                saveAndLoad.Save(currentDialogueContainer);
            }
        }
        
        private void Load()
        {
            if (currentDialogueContainer != null)
            {
                Language(LanguageType.Polish, toolbarMenu);
                nameOfDialougeContainer.text = "Name:   " + currentDialogueContainer.name;
                saveAndLoad.Load(currentDialogueContainer);
            }
        }
        
        private void Language(LanguageType language, ToolbarMenu _toolbarMenu)
        {
            toolbarMenu.text = "Language: " + language.ToString();
            languageType = language;
            graphView.LanguageReload();
        }
    }
}