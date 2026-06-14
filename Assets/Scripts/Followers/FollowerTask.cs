public abstract class FollowerTask
{
    protected Follower Owner;
    public abstract void Begin(Follower follower);
    public abstract void Tick();
    public abstract bool IsComplete();
}