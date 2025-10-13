using System.IO;
using System.Collections.Generic;
using System;

public class QuestSystem
{
    private List<QuestBase> quests;

    public event Action<QuestBase> questStateChanged;

    public QuestSystem(string questDataString)
    {
        quests = Load(questDataString);
    }

    public void SetStateForQuestById(int id, QuestState stateType)
    {
        QuestBase quest = GetQuestById(id);
        quest.state = stateType;
        questStateChanged?.Invoke(quest);
    }

    private QuestBase GetQuestById(int id)
    {
        QuestBase quest = quests.Find(x => x.id == id);

        if (quest == null)
        {
            throw new Exception("Квест с указанным ID не найден");
        }

        return quest;
    }


    #region Работа с файлом
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
    #endregion
}
