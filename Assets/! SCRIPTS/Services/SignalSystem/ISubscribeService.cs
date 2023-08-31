namespace Services.SignalSystem
{
    public interface ISubscribeService
    {
        void Subscribe(object listener);
        void Unsubscribe(object listener);
    }
}