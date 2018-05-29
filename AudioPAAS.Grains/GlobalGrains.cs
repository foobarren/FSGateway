using System;
using Orleans;
using AudioPAAS.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Orleans.MultiCluster;

namespace AudioPAAS.Grains
{
    [GlobalSingleInstance]
    public class GlobalTenantIDRegistyGrain : Orleans.Grain, IGlobalTenantIDRegistyGrain
    {
        Dictionary<string, TenantInfo> Tenants = new Dictionary<string, TenantInfo>();

        Task<bool> IGlobalTenantIDRegistyGrain.Register(string TenantID, TenantInfo info)
        {
            bool ret = true;
            if (Tenants.ContainsKey(TenantID))
                ret = false;
            else
                Tenants[TenantID] = info;
            return Task.FromResult(ret);
        }
    }

    [GlobalSingleInstance]
    public class GlobalInboundNumberRegistyGrain : Orleans.Grain, IGlobalInboundNumberRegistyGrain
    {
        Dictionary<string, string> InboundNumbers = new Dictionary<string, string>();

        Task<bool> IGlobalInboundNumberRegistyGrain.Register(string InboundNumber, string TenantID)
        {
            bool ret = true;
            if (InboundNumbers.ContainsKey(InboundNumber))
                ret = false;
            else
                InboundNumbers[InboundNumber] = TenantID;
            return Task.FromResult(ret);
        }
        Task IGlobalInboundNumberRegistyGrain.UnRegister(string InboundNumber)
        {
            InboundNumbers.Remove(InboundNumber);
            return Task.CompletedTask;
        }
    }
}