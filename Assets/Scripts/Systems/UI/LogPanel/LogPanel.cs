using TMPro;
using UnityEngine;

public class LogPanel : MonoBehaviour
{
    [SerializeField, Min(1)]
    private int maxLogStrings = 1;
    [SerializeField, Min(1)]
    private float logStringsLifetime = 1;
    [SerializeField]
    private Transform logstringsContainer;
    [SerializeField]
    private GameObject logStringPrefab;

    public static LogPanel instance;

    private void Awake()
    {
        instance = this;
    }

    public void ShowStringInLog(string str)
    {
        if(logstringsContainer.childCount >= maxLogStrings)
        {
            Destroy(logstringsContainer.GetChild(0).gameObject);
        }

        GameObject stringItem = Instantiate(logStringPrefab, logstringsContainer);
        stringItem.GetComponent<TMP_Text>().text = str;
        Destroy(stringItem, logStringsLifetime);
    }
}
