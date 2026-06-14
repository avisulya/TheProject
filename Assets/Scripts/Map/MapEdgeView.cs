using UnityEngine;
using UnityEngine.UI;
 
public class MapEdgeView : MonoBehaviour
{
    public Image line;
 
    public void Connect(Vector2 from, Vector2 to, float width = 6f)
    {
        var delta    = to - from;
        var length   = delta.magnitude;
        var angle    = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        var mid      = (from + to) * 0.5f;
        var rt       = line.rectTransform;
        rt.anchoredPosition = mid;
        rt.sizeDelta        = new Vector2(length, width);
        rt.rotation         = Quaternion.Euler(0, 0, angle);
    }
}