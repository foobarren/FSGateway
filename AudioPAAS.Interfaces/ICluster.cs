using Orleans;
using System;
using System.Threading.Tasks;

namespace AudioPAAS.Interfaces
{
    public interface IClusterTenantManagerGrain : IGrain
    {
        Task Enter(TenantInfo Info);
        Task Exit(TenantInfo Info);
    }

    public interface IClusterInboundNumberManagerGrain : IGrain
    {
        Task Enter(InboundAudioNumberInfo Info);
        Task Exit(InboundAudioNumberInfo Info);
    }

    public interface IClusterMediaServerManagerGrain : IGrain
    {
        Task Enter(MediaServerInfo member);
        Task Exit(MediaServerInfo member);
    }

}
