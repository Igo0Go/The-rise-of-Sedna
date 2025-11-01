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
    private bool dirty = true;
    public List<QuestDetails> details;
    public QuestState state = QuestState.waitStart;
    public QuestType typeOfQuest = QuestType.Activation;

    [Header("Activation")]
    public List<int> activationIds;


    public QuestBase ToQuestBase()
    {
        switch(typeOfQuest)
        {
            case QuestType.Activation:
                return new ActivationQuest()
                {
                    id = this.id,
                    name = this.name,
                    description = this.description,
                    State = this.state,
                    activationObjectsIds = activationIds,
                    details = this.details,
                    dirty = this.dirty
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
        sO.dirty = questBase.dirty;

        if(questBase is ActivationQuest activationQuest)
        {
            sO.typeOfQuest = QuestType.Activation;
            sO.activationIds = activationQuest.activationObjectsIds;
        }

        return sO;
    }
}
