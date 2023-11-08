namespace Services.InputSystem
{
    public readonly struct TapData
    {
        public readonly float HoldTime;
        public readonly int TapCount;

        public TapData(float holdDelay, int count)
        {
            HoldTime = holdDelay;
            TapCount = count;
        }
    }
}
