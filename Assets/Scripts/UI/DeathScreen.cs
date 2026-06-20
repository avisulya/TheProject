// Assets/Scripts/UI/DeathScreen.cs
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private RunState   runState;

    private void OnEnable()  => GameEvents.OnPlayerDied += Show;
    private void OnDisable() => GameEvents.OnPlayerDied -= Show;

    private void Show() => panel.SetActive(true);

    public void OnRetryPressed()
    {
        panel.SetActive(false);
        runState.graph = null;          // run abandoned, no active dungeon
        runState.currentNode = null;
        SceneTransition.LoadBase();
    }

    public void OnQuitPressed() => SceneTransition.LoadMainMenu();
}