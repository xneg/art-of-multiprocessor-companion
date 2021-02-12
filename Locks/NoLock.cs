namespace Locks
{
    public class NoLock: ILock
    {
        public void Lock()
        {
        }

        public void Unlock()
        {
        }
    }
}