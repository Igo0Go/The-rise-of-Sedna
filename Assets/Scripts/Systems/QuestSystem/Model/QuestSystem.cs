using System.IO;
using System.Collections.Generic;
using System;

public class QuestSystem
{
    #region Старый код
    private List<QuestBase> quests;

    public event Action newInfoInJornal;

    public QuestSystem(string questDataString)
    {
        quests = Load(questDataString);
    }

    public void SetStateForQuestById(int id, QuestState stateType)
    {
        QuestBase quest = GetQuestById(id);
        quest.State = stateType;

        if(stateType == QuestState.active)
        {
            newInfoInJornal?.Invoke();
        }
    }

    public QuestBase GetQuestById(int id)
    {
        QuestBase quest = quests.Find(x => x.id == id);

        if (quest == null)
        {
            throw new Exception("Квест с указанным ID не найден");
        }

        return quest;
    }

    public void ActivationQuestTargetUsed(int targetId)
    {
        List<QuestBase> actQuests = quests.FindAll(q => q is  Quest_Activation);

        for (int i = 0; i < actQuests.Count; i++)
        {
            Quest_Activation actQuest = actQuests[i] as Quest_Activation;
            actQuest.OnQuestTargetActivation(targetId);
        }
    }

    public List<QuestBase> GetAllQuestsWithState(QuestState state)
    {
        return quests.FindAll(q => q.State == state);
    }

    public void UnblockQuestDetails(int questId, int detailsIndex)
    {
        QuestBase quest = GetQuestById(questId);
        quest.dirty = true;
        quest.details[detailsIndex].unblock = true;
        newInfoInJornal?.Invoke();
    }
    #endregion

    public void TryCompleteSearchQuest(int questId, FPC_InventorySystem inventorySystem)
    {
        Quest_Collecting quest = quests.Find(q => q.id == questId) as Quest_Collecting;
        quest.TryCompleteQuest(inventorySystem);
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

        string[] separators = { "{\r\n", "}\r\n", "----"};
        string[] QuestDataStrings = dataText.Split(separators,
            System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string questData in QuestDataStrings)
        {
            QuestBase quest;

            string[] s = { "\n", "\r", "type: ", "id: ", "name: ", "desc: ", "state: ", "dirty: " };
            string[] QuestSettingsStrings = questData.Split(s, 
                System.StringSplitOptions.RemoveEmptyEntries);

            switch (GetQuestTypeFromLoadString(QuestSettingsStrings[0]))
            {
                case QuestType.Activation:
                    quest = new Quest_Activation(QuestSettingsStrings);
                    break;
                case QuestType.Search:
                    quest = new Quest_Collecting(QuestSettingsStrings);
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
