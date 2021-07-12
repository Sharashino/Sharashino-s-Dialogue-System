using System;
using SDS.DialogueSystem.SO;
using SDS.DialogueSystem.Enums;
using System.Collections.Generic;

// Class used to update game language
namespace SDS.DialogueSystem.Actions
{
    public class UpdateLanguageType
    {
        // Updating dialogue language
        public void UpdateLanguage()
        {
            List<DialogueContainerSO> dialogueContainers = Helper.FindAllObjectFromResources<DialogueContainerSO>();

            foreach (DialogueContainerSO DialogueContainer in dialogueContainers)
            {
                foreach (DialogueNodeData nodeData in DialogueContainer.DialogueNodeDatas)
                {
                    nodeData.TextLanguages = UpdateLanguageGeneric(nodeData.TextLanguages);
                    nodeData.AudioClips = UpdateLanguageGeneric(nodeData.AudioClips);

                    foreach (DialogueNodePort nodePort in nodeData.DialogueNodePorts)
                    {
                        nodePort.TextLanguages = UpdateLanguageGeneric(nodePort.TextLanguages);
                    }
                }
            }
        }
        
        // Updating game language
        private List<LanguageGeneric<T>> UpdateLanguageGeneric<T>(List<LanguageGeneric<T>> languageGenerics)
        {
            List<LanguageGeneric<T>> tmp = new List<LanguageGeneric<T>>();

            foreach (LanguageType languageType in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
            {
                tmp.Add(new LanguageGeneric<T>
                {
                    LanguageType = languageType
                });
            }

            foreach (LanguageGeneric<T> languageGeneric in languageGenerics)
            {
                if (tmp.Find(languag => languag.LanguageType == languageGeneric.LanguageType) != null)
                {
                    tmp.Find(languag => languag.LanguageType == languageGeneric.LanguageType).LanguageGenericType = languageGeneric.LanguageGenericType;
                }
            }

            return tmp;
        }
    }
}

