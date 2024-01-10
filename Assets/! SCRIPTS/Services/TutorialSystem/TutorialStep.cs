namespace Services.TutorialSystem
{
    public enum TutorialStep : byte
    {
        StartTutorial = 0,
        Movement = 1,
        Equipment = 2,
        Simulator = 3,
        Relaxer = 4,
        Arena = 5,
        EndTutorial = 254,
    }
}
