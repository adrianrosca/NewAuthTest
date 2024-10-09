using System.Threading.Tasks;

namespace WcfCoreService
{
    public class WcfService : IWcfService
    {
        public Task<string> GetData(string value)
        {
            return Task.FromResult($"You entered: {value}");
        }
    }
}
