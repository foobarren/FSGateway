using Orleans;
using Orleans.Concurrency;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AudioPAAS.Interfaces
{

    [Flags]
    public enum AgentState
    {
        Active,
        Block
    }

    [Serializable]
    [Immutable]
    public class AgentInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DeptId { get; set; }
        public AgentState state { get; set; }
        public string Number { get; set; }

        public AgentInfo(string id, string name, string deptid)
        {
            Id = id;
            Name = name;
            DeptId = deptid;
            state = AgentState.Active;
        }
    }

    [Flags]
    public enum TenantState
    {
        Active,
        Block
    }

    [Serializable]
    [Immutable]
    public class TenantInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NumberPrefix { get; set; }
        public string Description { get; set; }
        public TenantState state { get; set; }
        public Dictionary<string, string> Info { get; set; }
    }

    public interface ITenantGrain : IGrainWithStringKey
    {
        Task<string> Name();
        Task SetName(string name);
        Task SetInfo(string key, string value);

        // Players have names
        Task Enter(AgentRuntimeInfo member);
        Task Exit(AgentRuntimeInfo member);

        Task Enter(QueueInfo member);
        Task Exit(QueueInfo member);

        Task Enter(InboundAudioNumberInfo member);
        Task Exit(InboundAudioNumberInfo member);

        Task<bool> RegisterAgent(AgentInfo Info);
        Task UnRegisterAgent(AgentInfo Info);
        Task<bool> BatchRegisterAgent(long start,long end,string prefix);
        Task BatchUnRegisterAgent(string[] agentIds);

        Task BlockAgent(string agentid);
        Task ResumeAgent(string agentid);
    }

    [Serializable]
    [Immutable]
    public class InboundAudioNumberInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DeptId { get; set; }
        public Dictionary<string, string> Info { get; set; }
        public string MainServiceIVR { get; set; }
        public string WaitServiceIVR { get; set; }
        public string SataServiceIVR { get; set; }
    }

    [Flags]
    public enum CustomerState
    {
        Active,
        Block
    }
    [Serializable]
    [Immutable]
    public class CustomerInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Guid channel { get; set; }
        public CustomerState state { get; set; }
        public string LastServiceAgentId { get; set; }
        public DateTime LastServiceTime { get; set; }
        public DateTime EnterTime { get; set; }
    }

    [Serializable]
    [Immutable]
    public class IvrInfo
    {
        public Guid Key { get; set; }
        public string Name { get; set; }
    }

    public interface IInboundAudioServiceNumberGrain : IGrainWithStringKey
    {
        Task<string> Name();
        Task SetName(string name);
        Task SetInfo(string key, string value);

        Task Enter(AudioRoomInfo member);
        Task Exit(AudioRoomInfo member);
        // Customer can enter or exit a InboundServiceNumber
        Task Enter(CustomerInfo member);
        Task Exit(CustomerInfo member);
    }

    [Flags]
    public enum QueueState
    {
        Active,
        Block
    }
    [Serializable]
    [Immutable]
    public class QueueInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TenantId { get; set; }
        public QueueState state { get; set; }
        public Dictionary<string, string> Info { get; set; }
    }

    public interface IQueueGrain : IGrainWithStringKey
    {
        Task<string> Name();
        Task SetName(string name);
        //Task<string> Description();
        Task SetInfo(string key, string value);

        // Customer can enter or exit a queue
        Task Enter(CustomerInfo member, AudioRoomInfo room);
        Task Exit(CustomerInfo member);

        // Agent can enter or exit a queue
        Task Enter(AgentRuntimeInfo member, int postion);
        Task Exit(AgentRuntimeInfo member);

        Task Block();
        Task Resume();

    }

    public class AgentPostion
    {
        public int Iter { get; set; }
        public int Postion { get; set; }
    }

    public interface IQueueWithAgentGrain : IGrainWithStringKey
    {
        Task AddAgentToQueue(string agentId, string queueId, int iter, int postion);
        Task RemoveAgentToQueue(string agentId, string queueId);

        Task BatchAddAgentToQueue(string[] agentIds, string queueId, int iter, int postion);
        Task BatchRemoveAgentToQueue(string[] agentIds, string queueId);

        Task<Dictionary<string, int>> QueryQueues(string agentId);
        Task<Dictionary<string, AgentPostion>> QueryAgents(string queueId);

        Task ClearAgent(string agentId);
        Task ClearQueue(string queueId);
    }

    [Flags]
    public enum AgentRuntimeState
    {
        Login,
        Waiting,
        Working,
        Leave,
        Logout
    }

    [Serializable]
    [Immutable]
    public class AgentRuntimeInfo
    {
        public AgentInfo BaseInfo { get; set; }
        public string TelNumber { get; set; }
        public AgentRuntimeState state { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public DateTime LastStateChangeTime { get; set; }
    }


    public interface IAgentRuntimeGrain : IGrainWithStringKey
    {
        //Task<string> Description();
        Task SetInfo(string key, string value);
        Task SetRuntimeInfo(string key, string value);
        Task SetPhoneStatus(string status);

        Task Login();
        Task Logout();
        Task InCall(CustomerInfo member, AudioRoomInfo room);
        Task Accept();
        Task Reject();
    }
}
