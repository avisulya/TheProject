using UnityEngine;
using UnityEngine.AI;
 
[RequireComponent(typeof(NavMeshAgent))]
public class Follower : MonoBehaviour
{
    [SerializeField] private FollowerData data;
    [SerializeField] private GameState    gameState;
 
    private NavMeshAgent _agent;
    private FollowerTask _currentTask;
 
    public FollowerData Data => data;
 
    private void Awake() => _agent = GetComponent<NavMeshAgent>();
 
    private void Start() => AssignIdleTask();

    private void Update()
    {
        _currentTask?.Tick();
    }
 
    public void OnDayTick()
    {
        data.age++;
        data.hunger = Mathf.Min(100, data.hunger + 20f);
 
        TryEat();
        UpdateMood();
        CheckDeath();
 
        if (data.isDissident)
            data.loyalty = Mathf.Max(0, data.loyalty - 5);
    }
 
    private void TryEat()
    {
        if (gameState.food > 0)
        {
            gameState.food -= 1;
            data.hunger     = Mathf.Max(0, data.hunger - 40f);
            GameEvents.ResourceChanged(ResourceType.Food, gameState.food);
        }
    }
 
    private void UpdateMood()
    {
        // Hunger drains happiness
        if (data.hunger > 70f)
            data.happiness = Mathf.Max(0, data.happiness - 15f);
        else
            data.happiness = Mathf.Min(100, data.happiness + 5f);
 
        // Low happiness breeds dissidence
        data.isDissident = data.happiness < 20f;
    }
 
    private void CheckDeath()
    {
        if (data.age >= data.maxAge || data.hunger >= 100f)
        {
            data.isDead = true;
            GameEvents.FollowerDied(data);
            Destroy(gameObject);
        }
    }
 
    public void WalkTo(Vector3 destination)
        => _agent.SetDestination(destination);
 
    public void AssignTask(FollowerTask task)
    {
        _currentTask = task;
        task.Begin(this);
    }
 
    public void AssignIdleTask()
    {
        AssignTask(new WanderTask());
    }
}