namespace Services.SignalSystem
{
    public class Receiver
    {
        public readonly object Listener;
        public readonly int Priority;

        public Receiver(object listener, int priority)
        {
            Listener = listener;
            Priority = priority;
        }
    }
}
