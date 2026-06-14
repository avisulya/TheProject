using UnityEngine;
 
public class WanderTask : FollowerTask
{
    private float _wanderRadius = 8f;
    private float _wanderTimer  = 0f;
    private float _wanderInterval = 5f;
 
    public override void Begin(Follower follower)
    {
        Owner = follower;
    }
 
    public override void Tick()
    {
        _wanderTimer += Time.deltaTime;
        if (_wanderTimer >= _wanderInterval)
        {
            _wanderTimer = 0f;
            var offset   = Random.insideUnitSphere * _wanderRadius;
            offset.y     = 0;
            Owner.WalkTo(Owner.transform.position + offset);
        }
    }
 
    public override bool IsComplete() => false;   // wander runs until overridden
}