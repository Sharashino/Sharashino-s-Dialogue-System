using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace SDS.DialogueSystem.Actions
{
    public static class Helper
    {
        public static List<T> FindAllObjectFromResources<T>()
        {
            List<T> tmp = new List<T>();
            string resourcesPath = Application.dataPath + "/Resources";
            string[] directories = Directory.GetDirectories(resourcesPath, "*", SearchOption.AllDirectories);

            foreach (string directory in directories)
            {
                string directoryPath = directory.Substring(resourcesPath.Length + 1);
                T[] result = Resources.LoadAll(directoryPath, typeof(T)).Cast<T>().ToArray();

                foreach (T item in result)
                {
                    if (!tmp.Contains(item))
                    {
                        tmp.Add((item));
                    }
                }
            }

            return tmp;
        }
    } 
}


