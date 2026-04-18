namespace Performance.Application.Interface.Hashing
{
    public interface IIdHelper
    {
        string EncodeId (long id);
        long DecodeId (string encodedId);
    }
}