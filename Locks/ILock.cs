namespace Locks
{
    public interface ILock
    {
        void Lock();
        void Unlock();
    }
}