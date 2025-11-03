public class Quest_Search : QuestBase
{
    public int objectId;
    public int count;

    public Quest_Search() { }

    public Quest_Search(string[] settingsStrings) : base(settingsStrings)
    {
    }

    protected override int GetQuestTypeIndex()
    {
        return (int)QuestType.Search;
    }

    protected override string GetSpecificData()
    {
        return "targetObjects: [" + objectId + "," + count +"]";
    }

    protected override void SetSpecificData(string inputString)
    {
        string[] s = { "\n", "targetObjects: ", "[", "]", "," };
        string[] dataStrings = inputString.Split(s, System.StringSplitOptions.RemoveEmptyEntries);

        objectId = int.Parse(dataStrings[0]);
        count = int.Parse(dataStrings[1]);
    }

    public void TryCompleteQuest(FPC_InventorySystem inventorySystem)
    {
        if(inventorySystem.TrySpendItem(objectId, count))
        {
            State = QuestState.complete;
        }
    }
}
