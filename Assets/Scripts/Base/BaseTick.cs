using System.Collections;
using UnityEngine;
 
public class BaseTick : MonoBehaviour
{
    [SerializeField] private float dayDuration = 120f;   // seconds per in-game day
    [SerializeField] private GameState gameState;
 
    private void Start() => StartCoroutine(DayCycle());
 
    private IEnumerator DayCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(dayDuration);
            PerformTick();
        }
    }
 
    private void PerformTick()
    {
        // All buildings produce
        foreach (var b in FindObjectsOfType<Building>())
            b.OnDayTick();
 
        // All followers consume food and age
        foreach (var f in FindObjectsOfType<Follower>())
            f.OnDayTick();
 
        GameEvents.BaseTick();
    }
}