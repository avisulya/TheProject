using UnityEngine;
using UnityEngine.InputSystem;
 
public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    [SerializeField] private LayerMask groundLayer;
 
    private BuildingData _pendingData;
    private GameObject   _preview;
    private bool         _placing;
 
    public void BeginPlacement(BuildingData data)
    {
        if (_placing) CancelPlacement();
 
        if (!CanAfford(data))
        {
            Debug.Log("Cannot afford " + data.buildingName);
            return;
        }
 
        _pendingData = data;
        _preview     = Instantiate(data.prefab);
        SetPreviewAlpha(_preview, 0.5f);
        _placing     = true;
    }
 
    private void Update()
    {
        if (!_placing) return;
 
        // Move preview to mouse world position
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit, 100f, groundLayer))
        {
            _preview.transform.position = SnapToGrid(hit.point);
 
            if (Mouse.current.leftButton.wasPressedThisFrame)
                ConfirmPlacement(hit.point);
        }
 
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            CancelPlacement();
    }
 
    private void ConfirmPlacement(Vector3 pos)
    {
        gameState.SpendResource(ResourceType.Wood,  _pendingData.costWood);
        gameState.SpendResource(ResourceType.Stone, _pendingData.costStone);
        GameEvents.ResourceChanged(ResourceType.Wood,  gameState.wood);
        GameEvents.ResourceChanged(ResourceType.Stone, gameState.stone);
 
        Destroy(_preview);
        var placed = Instantiate(_pendingData.prefab, SnapToGrid(pos), Quaternion.identity);
        placed.GetComponent<Building>(); // ensure component is present
 
        _placing     = false;
        _pendingData = null;
        _preview     = null;
    }
 
    private void CancelPlacement()
    {
        if (_preview != null) Destroy(_preview);
        _placing     = false;
        _pendingData = null;
        _preview     = null;
    }
 
    private bool CanAfford(BuildingData d)
        => gameState.wood >= d.costWood && gameState.stone >= d.costStone;
 
    private Vector3 SnapToGrid(Vector3 pos)
        => new Vector3(Mathf.Round(pos.x), pos.y, Mathf.Round(pos.z));
 
    private void SetPreviewAlpha(GameObject obj, float alpha)
    {
        foreach (var r in obj.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in r.materials)
            {
                var c = mat.color;
                c.a      = alpha;
                mat.color = c;
            }
        }
    }
}