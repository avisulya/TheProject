using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
 
public class MapManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private RunState runState;
 
    [Header("Prefabs")]
    [SerializeField] private MapNodeView nodeViewPrefab;
    [SerializeField] private MapEdgeView edgeViewPrefab;
    [SerializeField] private MapCursor   cursorPrefab;
 
    [Header("Containers")]
    [SerializeField] private RectTransform nodeContainer;
    [SerializeField] private RectTransform edgeContainer;
    [SerializeField] private RectTransform cursorContainer;
 
    [Header("Icons")]
    [SerializeField] private Sprite iconStart;
    [SerializeField] private Sprite iconCombat;
    [SerializeField] private Sprite iconElite;
    [SerializeField] private Sprite iconTreasure;
    [SerializeField] private Sprite iconShop;
    [SerializeField] private Sprite iconBoss;
 
    [Header("Base Button")]
    [SerializeField] private GameObject returnToBaseButton;
 
    private readonly Dictionary<MapNode, MapNodeView> _views = new();
    private MapCursor      _cursor;
    private MapNode        _selected;
    private List<MapNode>  _reachable;
 
    private void Start()
    {
        BuildMap();
        _cursor = Instantiate(cursorPrefab, cursorContainer);
        RefreshReachable();
        SelectNode(runState.currentNode);
 
        // Show base button only when current node is cleared (run finished)
        returnToBaseButton.SetActive(runState.IsCleared(runState.currentNode));
    }
 
    private void Update() => HandleInput();
 
    private void BuildMap()
    {
        foreach (var edge in runState.graph.edges)
        {
            var view    = Instantiate(edgeViewPrefab, edgeContainer);
            var fromPos = NormToCanvas(edge.from.mapPosition);
            var toPos   = NormToCanvas(edge.to.mapPosition);
            view.Connect(fromPos, toPos);
        }
 
        foreach (var node in runState.graph.nodes)
        {
            var view = Instantiate(nodeViewPrefab, nodeContainer);
            var rt   = view.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = node.mapPosition;
            rt.anchoredPosition = Vector2.zero;
            view.Initialise(node, GetIcon(node.nodeType),
                !IsAccessible(node), runState.IsCleared(node));
            _views[node] = view;
        }
    }
 
    private void RefreshReachable()
        => _reachable = runState.graph.GetNeighbours(runState.currentNode);
 
    private bool IsAccessible(MapNode n)
        => n == runState.currentNode || _reachable.Contains(n);
 
    private void SelectNode(MapNode n)
    {
        if (_selected != null && _views.TryGetValue(_selected, out var prev))
            prev.SetHighlight(false);
        _selected = n;
        if (_views.TryGetValue(n, out var view))
        {
            view.SetHighlight(true);
            _cursor.MoveTo(view.GetComponent<RectTransform>().anchoredPosition);
        }
    }
 
    private void HandleInput()
    {
        var kb = Keyboard.current;
        if (kb == null) return;
        if (kb.wKey.wasPressedThisFrame || kb.upArrowKey.wasPressedThisFrame)
            TryMove(Vector2.up);
        else if (kb.sKey.wasPressedThisFrame || kb.downArrowKey.wasPressedThisFrame)
            TryMove(Vector2.down);
        else if (kb.aKey.wasPressedThisFrame || kb.leftArrowKey.wasPressedThisFrame)
            TryMove(Vector2.left);
        else if (kb.dKey.wasPressedThisFrame || kb.rightArrowKey.wasPressedThisFrame)
            TryMove(Vector2.right);
        if (kb.enterKey.wasPressedThisFrame || kb.spaceKey.wasPressedThisFrame)
            TryEnter();
    }
 
    private void TryMove(Vector2 dir)
    {
        MapNode best    = null;
        float   bestDot = 0.3f;
        foreach (var candidate in _reachable)
        {
            if (candidate == _selected) continue;
            var delta = candidate.mapPosition - _selected.mapPosition;
            var dot   = Vector2.Dot(delta.normalized, dir);
            if (dot > bestDot) { bestDot = dot; best = candidate; }
        }
        if (best != null) SelectNode(best);
    }
 
    private void TryEnter()
    {
        if (_selected == null || _selected == runState.currentNode) return;
        if (!_reachable.Contains(_selected)) return;
        runState.currentNode = _selected;
        SceneTransition.Load(_selected.sceneName);
    }
 
    public void OnReturnToBase() => SceneTransition.LoadBase();
 
    private Vector2 NormToCanvas(Vector2 n)
    {
        var size = nodeContainer.rect.size;
        return new Vector2(n.x * size.x, n.y * size.y) - size * 0.5f;
    }
 
    private Sprite GetIcon(NodeType t) => t switch
    {
        NodeType.Start    => iconStart,
        NodeType.Combat   => iconCombat,
        NodeType.Elite    => iconElite,
        NodeType.Treasure => iconTreasure,
        NodeType.Shop     => iconShop,
        NodeType.Boss     => iconBoss,
        _                 => iconCombat
    };
}