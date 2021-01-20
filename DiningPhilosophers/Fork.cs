namespace DiningPhilosophers
{
    public class Fork
    {
        private Philosopher _owner;
        private readonly int _index;

        public Philosopher Owner
        {
            get => _owner;
            set
            {
                if (value is null || _owner is null)
                {
                    _owner = value;
                }
            }
        }
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