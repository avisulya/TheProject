using UnityEngine;
 
public class DeathScreen : MonoBehaviour
{
    [SerializeField] private GameObject panel;
 
    private void OnEnable()  => GameEvents.OnPlayerDied += Show;
    private void OnDisable() => GameEvents.OnPlayerDied -= Show;
 
    private void Show()     => panel.SetActive(true);
 
    public void OnRetryPressed()
    {
        panel.SetActive(false);
        SceneTransition.LoadWorldMap();
    }
 
    public void OnQuitPressed() => SceneTransition.LoadMainMenu();
}