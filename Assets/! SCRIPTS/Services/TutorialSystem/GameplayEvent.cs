namespace Services.TutorialSystem
{
    public enum GameplayEvent : byte
    {
        StartGame = 0,
        JoysticInput = 1,
        BuyEquipment = 2,
        PushTrainButton = 3,
        PushSleepButton = 4,
        PushFightButton = 5,
        Void = 254,
    }
}
