using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadPlayerSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject deadPanel;

    void Awake()
    {
        deadPanel.SetActive(false);
        FindFirstObjectByType<FPC_HealhSystem>().OnDead += OnDead;
    }

    private void OnDead()
    {
        FindFirstObjectByType<PauseControlSystem>().IsDead = true;
        deadPanel.SetActive(true);
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
