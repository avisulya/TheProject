using UnityEngine;
using UnityEngine.SceneManagement;
 
public static class SceneTransition
{
    public static void Load(string sceneName) => SceneManager.LoadScene(sceneName);
    public static void LoadWorldMap()         => SceneManager.LoadScene("WorldMap");
    public static void LoadBase()             => SceneManager.LoadScene("Base");
    public static void LoadMainMenu()         => SceneManager.LoadScene("MainMenu");
}