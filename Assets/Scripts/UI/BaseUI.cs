using TMPro;
using UnityEngine;
 
public class BaseUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI woodText;
    [SerializeField] private TextMeshProUGUI stoneText;
    [SerializeField] private TextMeshProUGUI foodText;
    [SerializeField] private TextMeshProUGUI devotionText;
    [SerializeField] private GameState       gameState;
    [SerializeField] private BuildingPlacer  placer;
 
    private void OnEnable()
    {
        GameEvents.OnResourceChanged += OnResourceChanged;
        RefreshAll();
    }
 
    private void OnDisable()
        => GameEvents.OnResourceChanged -= OnResourceChanged;
 
    private void RefreshAll()
    {
        woodText.text     = $"Wood: {gameState.wood}";
        stoneText.text    = $"Stone: {gameState.stone}";
        foodText.text     = $"Food: {gameState.food}";
        devotionText.text = $"Devotion: {gameState.devotion}";
    }
 
    private void OnResourceChanged(ResourceType type, int value)
    {
        switch (type)
        {
            case ResourceType.Wood:     woodText.text     = $"Wood: {value}"; break;
            case ResourceType.Stone:    stoneText.text    = $"Stone: {value}"; break;
            case ResourceType.Food:     foodText.text     = $"Food: {value}"; break;
            case ResourceType.Devotion: devotionText.text = $"Devotion: {value}"; break;
        }
    }
 
    // Called by each BuildingButton in the build menu
    public void OnBuildingButtonPressed(BuildingData data)
        => placer.BeginPlacement(data);
 
    public void OnEnterDungeonPressed()
        => SceneTransition.LoadWorldMap();
}