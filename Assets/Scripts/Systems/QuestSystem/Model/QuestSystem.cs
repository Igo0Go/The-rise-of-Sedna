using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class QuestSystem : MonoBehaviour
{
    [SerializeField]
    private TextAsset _textAsset;

    private List<QuestBase> quests;

    [ContextMenu("Сохранить")]
    public static void Save(List<QuestBase> quests, string path)
    {
        string s = string.Empty;

        foreach (QuestBase quest in quests)
        {
            s += quest.ConvertToSaveString();
            s += "----";
        }

        using (StreamWriter sw = new StreamWriter(path))
        {
            sw.Write(s);
        }
    }

    [ContextMenu("Загрузить")]
    public static List<QuestBase> Load(string dataText)
    {
        List<QuestBase> quests = new List<QuestBase>();

        string[] separators = { "{\n", "}\n", "----"};
        string[] QuestDataStrings = dataText.Split(separators,
            System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string questData in QuestDataStrings)
        {
            QuestBase quest;

            string[] s = { "\n", "type: ", "id: ", "name: ", "desc: ", "state: " };
            string[] QuestSettingsStrings = questData.Split(s, 
                System.StringSplitOptions.RemoveEmptyEntries);

            switch (GetQuestTypeFromLoadString(QuestSettingsStrings[0]))
            {
                case QuestType.Activation:
                    quest = new ActivationQuest(QuestSettingsStrings);
                    break;
                default:
                    quest = null;
                    break;  
            }

            quests.Add(quest);
        }

        return quests;
    }

    public static QuestType GetQuestTypeFromLoadString(string strings)
    {
        return (QuestType)int.Parse(strings);
    }
}
