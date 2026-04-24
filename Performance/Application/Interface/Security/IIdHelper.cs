namespace Performance.Application.Interface.Security
{
    public interface IIdHelper
    {
        string EncryptId (long id);
        long DecryptId (string encodedId);
    }
}