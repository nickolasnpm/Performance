namespace Performance.Application.Interface.Hashing
{
    public interface IIdHelper
    {
        string EncryptId (long id);
        long DecryptId (string encodedId);
    }
}