using Orleans;
using System;
using System.Threading.Tasks;

namespace AudioPAAS.Interfaces
{
    public interface IGlobalTenantIDRegistyGrain : IGrain
    {
        Task<bool> Register(string TenantID, TenantInfo info);
    }

    public interface IGlobalInboundNumberRegistyGrain : IGrain
    {
        Task<bool> Register(string InboundNumber, string TenantID);
        Task UnRegister(string InboundNumber);
    }
}
