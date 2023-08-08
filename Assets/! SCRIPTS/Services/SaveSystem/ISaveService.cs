namespace Services.SaveSystem
{
    public interface ISaveService
    {
        T Load<T>() where T : AbstractSaveData, new();
        void Save<T>(T data) where T : AbstractSaveData;
    }
}
