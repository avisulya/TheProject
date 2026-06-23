using UnityEngine;
 
public class GameInitialiser : MonoBehaviour
{
    [SerializeField] private GameState  gameState;
    [SerializeField] private RunState   runState;
    [SerializeField] private MapGraph   defaultGraph;
 
    public void StartNewGame()
    {
        gameState.Reset();
        runState.StartNewRun(defaultGraph);
        SceneTransition.LoadBase();   // was LoadWorldMap
    }
 
    public void ContinueGame()
    {
        SceneTransition.Load(gameState.lastScene);
    }
}