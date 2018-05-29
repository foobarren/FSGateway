using System;
using Orleans;
using AudioPAAS.Interfaces;
using System.Threading.Tasks;
using Orleans.MultiCluster;
using System.Collections.Generic;

namespace AudioPAAS.Grains
{
    [OneInstancePerCluster]
    public class ClusterTenantManagerGrain : Orleans.Grain,IClusterTenantManagerGrain
    {
        Dictionary<string, TenantInfo> Tenants = new Dictionary<string, TenantInfo>();

        Task IClusterTenantManagerGrain.Enter(TenantInfo Info)
        {
            Tenants[Info.Id] = Info;
            return Task.CompletedTask;
        }
        Task IClusterTenantManagerGrain.Exit(TenantInfo Info)
        {
            Tenants.Remove(Info.Id);
            return Task.CompletedTask;
        }
    }

    [OneInstancePerCluster]
    public class ClusterInboundNumberManagerGrain : Orleans.Grain, IClusterInboundNumberManagerGrain
    {
        Dictionary<string, InboundAudioNumberInfo> Tenants = new Dictionary<string, InboundAudioNumberInfo>();

        Task IClusterInboundNumberManagerGrain.Enter(InboundAudioNumberInfo Info)
        {
            Tenants[Info.Id] = Info;
            return Task.CompletedTask;
        }
        Task IClusterInboundNumberManagerGrain.Exit(InboundAudioNumberInfo Info)
        {
            Tenants.Remove(Info.Id);
            return Task.CompletedTask;
        }
    }

    [OneInstancePerCluster]
    public class ClusterMediaServerManagerGrain : Orleans.Grain, IClusterMediaServerManagerGrain
    {
        Dictionary<Guid, MediaServerInfo> Tenants = new Dictionary<Guid, MediaServerInfo>();

        Task IClusterMediaServerManagerGrain.Enter(MediaServerInfo Info)
        {
            Tenants[Info.Id] = Info;
            return Task.CompletedTask;
        }
        Task IClusterMediaServerManagerGrain.Exit(MediaServerInfo Info)
        {
            Tenants.Remove(Info.Id);
            return Task.CompletedTask;
        }
    }

}
