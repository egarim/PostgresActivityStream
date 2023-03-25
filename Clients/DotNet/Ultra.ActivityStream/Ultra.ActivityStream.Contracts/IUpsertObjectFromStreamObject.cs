namespace Ultra.ActivityStream.Contracts
{
    public interface IUpsertObjectFromStreamObject
    {
        Task ObjectStorageUpsert(object Instace);
    }
}