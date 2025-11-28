using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;

public class QuestSystem
{
    #region Старый код
    private List<QuestBase> quests;

    public event Action newInfoInJornal;
    public event Action<QuestBase> QuestStateChanged;

    public QuestBase GetQuestById(int id)
    {
        QuestBase quest = quests.Find(x => x.id == id);

        if (quest == null)
        {
            throw new Exception("Квест с указанным ID не найден");
        }

        return quest;
    }
    public List<QuestBase> GetAllQuestsWithState(QuestState state)
    {
        return quests.FindAll(q => q.State == state);
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
    public void UnblockQuestDetails(int questId, int detailsIndex)
    {
        QuestBase quest = GetQuestById(questId);

        if (quest.details[detailsIndex].unblock)
        {
            return;
        }
        quest.dirty = true;
        quest.details[detailsIndex].unblock = true;
        SkillHolder.Instance.AddExperience(quest.details[detailsIndex].EXP);
        newInfoInJornal?.Invoke();
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

    public void TryCompleteCollectingQuest(int questId)
    {
        Quest_Collecting quest = quests.Find(q => q.id == questId) as Quest_Collecting;
        quest.TryCompleteQuest();
    }

    public void TryCompleteHuntingQuest(int questId)
    {
        Quest_Collecting quest = quests.Find(q => q.id == questId) as Quest_Collecting;
        quest.TryCompleteQuest();
    }
    public void SubscribeQuestsToEnemyDeadEvent(List<EnemyBase> enemies)
    {
        List<QuestBase> huntingQuests = quests.FindAll(q => q is Quest_Hunting);

        foreach(Quest_Hunting quest in huntingQuests)
        {
            foreach(EnemyBase enemy in enemies)
            {
                if(enemy.ID == quest.targetEnemyId)
                {
                    enemy.deadEvent.AddListener((e) => quest.OnEnemyDead());
                }
            }
        }
    }
    #endregion

    public QuestSystem(string questDataString)
    {
        quests = Load(questDataString);

        foreach(QuestBase quest in quests)
        {
            quest.StateChanged += OnQuestStateChanged;
        }
    }

    private void OnQuestStateChanged(QuestBase quest)
    {
        QuestStateChanged?.Invoke(quest);
        if(quest.State == QuestState.complete)
        {
            CheckAllCompletedQuests();
        }
    }

    private void CheckAllCompletedQuests()
    {
        for(int i = 0; i < quests.Count; i++)
        {
            List<QuestBase> previousQuests = new List<QuestBase> ();
            for(int j = 0; j < quests[i].completedQuestsToStart.Count; j++)
            {
                previousQuests.Add(GetQuestById(quests[i].completedQuestsToStart[j]));
            }

            if(previousQuests.Count > 0)
            {
                bool key = true;
                foreach(QuestBase qpreviousQuest in previousQuests)
                {
                    if(qpreviousQuest.State != QuestState.complete)
                    {
                        key = false;
                        break;
                    }
                }

                if(key)
                {
                    SetStateForQuestById(quests[i].id, QuestState.active);
                }
            }
        }
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

            string[] s = { "\n", "\r", "type: ", "id: ", "name: ", "desc: ", "exp: ", "state: ", "dirty: " };
            string[] QuestSettingsStrings = questData.Split(s, StringSplitOptions.RemoveEmptyEntries);

            switch (GetQuestTypeFromLoadString(QuestSettingsStrings[0]))
            {
                case QuestType.Activation:
                    quest = new Quest_Activation(QuestSettingsStrings);
                    break;
                case QuestType.Collecting:
                    quest = new Quest_Collecting(QuestSettingsStrings);
                    break;
                case QuestType.Hunting:
                    quest = new Quest_Hunting(QuestSettingsStrings);
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
