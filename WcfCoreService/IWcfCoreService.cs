using CoreWCF;

namespace WcfCoreService
{
    [ServiceContract]
    public interface IWcfService
    {
        [OperationContract]
        Task<string> GetData(string value);
    }
}
