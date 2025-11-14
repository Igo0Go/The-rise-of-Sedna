using System.Linq;

public class Quest_Hunting : QuestBase
{
    public int targetEnemyId;
    public int targetsCount;

    private int currentDeadEnemy = 0;

    public Quest_Hunting(string[] settingsStrings) : base(settingsStrings)
    {
    }
    public Quest_Hunting()
    {

    }

    protected override int GetQuestTypeIndex()
    {
        return (int)QuestType.Hunting;
    }

    protected override string GetSpecificData()
    {
        string s = "targetInfo:[" + targetEnemyId + "," + targetsCount + "]";
        return s;
    }

    protected override void SetSpecificData(string inputString)
    {
        string[] s = { "\n", "targetInfo:", "[", "]", "," };
        string[] dataStrings = inputString.Split(s, System.StringSplitOptions.RemoveEmptyEntries);

        dataStrings = dataStrings.Where(x => x != "").ToArray();

        targetEnemyId = int.Parse(dataStrings[0]);
        targetsCount = int.Parse(dataStrings[1]);
    }

    public void OnEnemyDead()
    {
        currentDeadEnemy++;
        if (currentDeadEnemy >= targetsCount)
        {
            State = QuestState.complete;
        }
    }
}
