public interface IDataService
{
    bool CheckPath(string RelativePath);
    bool Save<T>(string RelativePath, T Data, bool Encrypted);
    T Load<T>(string RelativePath, bool Encrypted);
}
