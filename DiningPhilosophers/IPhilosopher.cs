namespace DiningPhilosophers
{
    public enum PhilosopherState
    {
        Waiting = 0,
        Eating = 1
    }

    public interface IPhilosopher
    {
        PhilosopherState CurrentState { get; }
        void Start();
        string ToString();
    }
}