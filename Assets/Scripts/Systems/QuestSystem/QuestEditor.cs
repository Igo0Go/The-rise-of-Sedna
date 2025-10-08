using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

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

            foreach (QuestSO quest in quests)
            {
                s += quest.ConvertToString();
            }

            string path = GetAssetPath();

            using (StreamWriter sw = new StreamWriter(path))
            {
                 sw.Write(s);
            }
        }
    }

    [ContextMenu("Загрузить")]
    public void Load()
    {
        if (_textAsset == null)
            return;

        string[] chars = { "{\n", "}\n" };
        string[] QuestDataStrings = _textAsset.text.Split(chars, System.StringSplitOptions.RemoveEmptyEntries);

        quests.Clear();

        foreach (string questData in QuestDataStrings)
        {
            quests.Add(QuestSO.ConvertFromString(questData));
        }
    }

    private string GetAssetPath()
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

    public string ConvertToString()
    {
        string s = "{\n";
        s += "id: " + id + "\n";
        s += "name: " + name + "\n";
        s += "desc: " + description + "\n";
        s += "state: " + (int)state + "\n";
        s += "type: " + (int)typeOfQuest + "\n";
        s += "}\n";
        return s;
    }

    public static QuestSO ConvertFromString(string input)
    {
        string[] s = {"\n", "id: ", "name: ", "desc: ", "state: ", "type: ", "spec: " };
        string[] QuestDataStrings = input.Split(s, System.StringSplitOptions.RemoveEmptyEntries);

        QuestSO questSO = new QuestSO();
        questSO.id = int.Parse(QuestDataStrings[0]);
        questSO.name = QuestDataStrings[1];
        questSO.description = QuestDataStrings[2];
        questSO.state = (QuestState)int.Parse(QuestDataStrings[3]);
        questSO.typeOfQuest = (QuestType)int.Parse(QuestDataStrings[4]);

        return questSO;
    }
}

public enum QuestType
{
    Activation = 1
}
