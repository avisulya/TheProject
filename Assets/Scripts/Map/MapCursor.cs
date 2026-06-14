using UnityEngine;
 
public class MapCursor : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    private RectTransform _rt;
    private Vector2       _target;
 
    private void Awake() => _rt = GetComponent<RectTransform>();
 
    public void SnapTo(Vector2 pos) { _target = pos; _rt.anchoredPosition = pos; }
    public void MoveTo(Vector2 pos)   => _target = pos;
 
    private void Update()
        => _rt.anchoredPosition = Vector2.Lerp(_rt.anchoredPosition, _target, Time.deltaTime * speed);
}