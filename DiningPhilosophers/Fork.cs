namespace DiningPhilosophers
{
    public class Fork
    {
        private readonly int _index;

        public Fork(int index)
        {
            _index = index;
        }

        public override string ToString()
        {
            return _index.ToString();
        }
    }
}