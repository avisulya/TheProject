using UnityEngine;
using UnityEngine.UI;
 
public class MapNodeView : MonoBehaviour
{
    public Image      icon;
    public GameObject lockedOverlay;
    public GameObject clearedOverlay;
 
    [HideInInspector] public MapNode node;
 
    public void Initialise(MapNode n, Sprite sprite, bool locked, bool cleared)
    {
        node               = n;
        icon.sprite        = sprite;
        lockedOverlay .SetActive(locked);
        clearedOverlay.SetActive(cleared);
    }
 
    public void SetHighlight(bool on)
        => transform.localScale = on ? Vector3.one * 1.15f : Vector3.one;
 
    public void Refresh(bool locked, bool cleared)
    {
        lockedOverlay .SetActive(locked);
        clearedOverlay.SetActive(cleared);
    }
}