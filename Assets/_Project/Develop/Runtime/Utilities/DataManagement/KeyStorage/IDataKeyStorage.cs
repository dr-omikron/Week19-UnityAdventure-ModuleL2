namespace _Project.Develop.Runtime.Utilities.DataManagement.KeyStorage
{
    public interface IDataKeyStorage
    {
        string GetKeyFor<TData>() where TData : ISaveData;
    }
}
