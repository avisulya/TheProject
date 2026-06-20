// BuildingButton.cs — place on each building button GameObject
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] private BuildingData   data;
    [SerializeField] private BuildingPlacer placer;

    private void Awake()
        => GetComponent<Button>().onClick.AddListener(
               () => placer.BeginPlacement(data));
}