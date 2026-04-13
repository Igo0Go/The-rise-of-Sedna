using System;
using System.Linq;

public class Quest_Collecting : QuestBase
{
    public int startingObjectId;
    public int startObjectsCount;

    public int collectedObjectId;
    public int collectedObjectsCount;

    public event Action<Quest_Collecting> CollectedQuestStartedWithItem;

    public Quest_Collecting(string[] settingsStrings) : base(settingsStrings)
    {
    }
    public Quest_Collecting()
    {
        
    }

    protected override void OnActivateQuest()
    {
        base.OnActivateQuest();

        if (startingObjectId < 0 || startObjectsCount <= 0)
            return;
        CollectedQuestStartedWithItem?.Invoke(this);
        InventarySystem.Instance.
          AddToInventory(startingObjectId, startObjectsCount);
    }
    public void TryCompleteQuest()
    {
        if (InventarySystem.Instance.
            TrySpendItem(collectedObjectId, collectedObjectsCount))
        {
            State = QuestState.complete;
        }
    }

    protected override int GetQuestTypeIndex()
    {
        return (int)QuestType.Collecting;
    }
    protected override string GetSpecificData()
    {
        string s = "startObjects:[" + startingObjectId + "," + startObjectsCount +"]";
        s += "collectedObjects:[" + collectedObjectId + "," + collectedObjectsCount + "]";
        return s;
    }
    protected override void SetSpecificData(string inputString)
    {
        string[] s = { "\n", "collectedObjects:", "startObjects:", "[", "]", "," };
        string[] dataStrings = inputString.Split(s, System.StringSplitOptions.RemoveEmptyEntries);

        dataStrings = dataStrings.Where(x => x != "").ToArray();

        startingObjectId = int.Parse(dataStrings[0]);
        startObjectsCount = int.Parse(dataStrings[1]);
        collectedObjectId = int.Parse(dataStrings[2]);
        collectedObjectsCount = int.Parse(dataStrings[3]);
    }
}
