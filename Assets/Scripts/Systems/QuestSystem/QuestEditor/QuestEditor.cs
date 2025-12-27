using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestEditor", menuName = "IgoGoTools/QuestEditor")]
public class QuestEditor : ScriptableObject
{
    [SerializeField]
    private TextAsset _textAsset;

    [SerializeField]
    private List<QuestSO> quests;

    [ContextMenu("Сохранить")]
    public void Save()
    {
        if(_textAsset != null)
        {
            string s = string.Empty;

            List<QuestBase> list = new List<QuestBase>();

            foreach (QuestSO quest in quests)
            {
                list.Add(quest.ToQuestBase());
            }

            string path = GetAssetPath(_textAsset);

            QuestSystem.Save(list, path);
        }
    }

    [ContextMenu("Загрузить")]
    public void Load()
    {
        if (_textAsset == null)
            return;

        List<QuestBase> baseQuests = QuestSystem.Load(_textAsset.text);

        quests.Clear();

        foreach (QuestBase quest in baseQuests)
        {
            quests.Add(QuestSO.GetFromBase(quest));
        }
    }

    private string GetAssetPath(TextAsset _textAsset)
    {
        string path = AssetDatabase.GetAssetPath(_textAsset);

        path = path.Remove(0, 7);

        path = Path.Combine(Application.dataPath, path);

        return path;
    }
}

[System.Serializable]
public class QuestSO
{
    [Header("Base")]
    public int id;
    [TextArea(2, 2)]
    public string name;
    [TextArea(4, 8)]
    public string description;
    public QuestState state = QuestState.waitStart;
    public QuestType typeOfQuest = QuestType.Activation;
    [Min(1)]
    public int exp;
    public List<QuestDetails> details;
    public List<int> questsToStart;

    private bool containcNewInfo = true;

    [Space(20)]
    [Header("Activation")]
    public List<int> activationIds;

    [Space(20)]
    [Header("Collecting")]
    [Min(0)]
    public int startObjectId = 0;
    [Min(0)]
    public int startObjectsCount = 0;
    [Min(0)]
    public int collectedObjectId = 0;
    [Min(1)]
    public int collectedObjectsCount = 1;

    [Space(20)]
    [Header("Hunting")]
    [Min(1)]
    public int targetEnemyId = 1;
    [Min(1)]
    public int targetEnemyCount = 1;


    public QuestBase ToQuestBase()
    {
        switch(typeOfQuest)
        {
            case QuestType.Activation:
                return new Quest_Activation()
                {
                    id = this.id,
                    name = this.name,
                    description = this.description,
                    exp = this.exp,
                    State = this.state,
                    details = this.details,
                    containceNewInfo = this.containcNewInfo,
                    activationObjectsIds = activationIds,
                    completedQuestsToStart = questsToStart
                };
            case QuestType.Collecting:
                return new Quest_Collecting()
                {
                    id = this.id,
                    name = this.name,
                    description = this.description,
                    exp = this.exp,
                    State = this.state,
                    details = this.details,
                    containceNewInfo = this.containcNewInfo,
                    startObjectsCount = this.startObjectsCount,
                    startingObjectId = this.startObjectId,
                    collectedObjectId = collectedObjectId,
                    collectedObjectsCount = this.collectedObjectsCount,
                    completedQuestsToStart = questsToStart
                };
            case QuestType.Hunting:
                return new Quest_Hunting()
                {
                    id = this.id,
                    name = this.name,
                    description = this.description,
                    exp = this.exp,
                    State = this.state,
                    details = this.details,
                    containceNewInfo = this.containcNewInfo,
                    targetEnemyId = this.targetEnemyId,
                    targetsCount = targetEnemyCount,
                    completedQuestsToStart = questsToStart
                };
            default:
                return null;
        }
    }

    public static QuestSO GetFromBase(QuestBase questBase)
    {
        QuestSO sO = new QuestSO();

        sO.id = questBase.id;
        sO.name = questBase.name;
        sO.description = questBase.description;
        sO.state = questBase.State;
        sO.details = questBase.details;
        sO.containcNewInfo = questBase.containceNewInfo;
        sO.exp = questBase.exp;
        sO.questsToStart = questBase.completedQuestsToStart;

        if(questBase is Quest_Activation activationQuest)
        {
            sO.typeOfQuest = QuestType.Activation;
            sO.activationIds = activationQuest.activationObjectsIds;
        }
        else if(questBase is Quest_Collecting collectingQuest)
        {
            sO.typeOfQuest = QuestType.Collecting;
            sO.startObjectId = collectingQuest.startingObjectId;
            sO.startObjectsCount = collectingQuest.startObjectsCount;
            sO.collectedObjectId = collectingQuest.collectedObjectId;
            sO.collectedObjectsCount = collectingQuest.collectedObjectsCount;
        }
        else if(questBase is Quest_Hunting huntingQuest)
        {
            sO.typeOfQuest = QuestType.Hunting;
            sO.targetEnemyId = huntingQuest.targetEnemyId;
            sO.targetEnemyCount = huntingQuest.targetsCount;
        }

        return sO;
    }
}
