namespace Performance.Application.Interface.Hashing
{
    public interface IIdHelper
    {
        string EncodeId (long id, string prefix);
        long DecodeId (string encodedId, string expectedPrefix);
    }
}