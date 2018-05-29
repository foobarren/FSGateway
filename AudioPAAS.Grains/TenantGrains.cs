using System;
using System.Linq;
using Orleans;
using AudioPAAS.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AudioPAAS.Grains
{
    public class TenantGrain : Grain, ITenantGrain
    {
        TenantInfo myInfo;
        Dictionary<string, AgentInfo> Agents = new Dictionary<string, AgentInfo>();
        Dictionary<string, AgentRuntimeInfo> AgentRuntimes = new Dictionary<string, AgentRuntimeInfo>();
        Dictionary<string, QueueInfo> Queues = new Dictionary<string, QueueInfo>();
        Dictionary<string, InboundAudioNumberInfo> InboundAudioNumbers = new Dictionary<string, InboundAudioNumberInfo>();
        

        Task<string> ITenantGrain.Name()
        {
            return Task.FromResult(myInfo.Name);
        }
        Task ITenantGrain.SetName(string name)
        {
            this.myInfo.Name = name;
            return Task.CompletedTask;
        }
        Task ITenantGrain.SetInfo(string key, string value)
        {
            myInfo.Info[key] = value;

            return Task.CompletedTask;
        }

        // Players have names
        Task ITenantGrain.Enter(AgentRuntimeInfo member)
        {
            AgentRuntimes[member.BaseInfo.Id] = member;
            return Task.CompletedTask;
        }
        Task ITenantGrain.Exit(AgentRuntimeInfo member)
        {
            AgentRuntimes.Remove(member.BaseInfo.Id);
            return Task.CompletedTask;
        }

        Task ITenantGrain.Enter(QueueInfo member)
        {
            Queues[member.Id] = member;
            return Task.CompletedTask;
        }
        Task ITenantGrain.Exit(QueueInfo member)
        {
            Queues.Remove(member.Id);
            return Task.CompletedTask;
        }

        Task ITenantGrain.Enter(InboundAudioNumberInfo member)
        {
            InboundAudioNumbers[member.Id] = member;
            return Task.CompletedTask;
        }
        Task ITenantGrain.Exit(InboundAudioNumberInfo member)
        {
            InboundAudioNumbers.Remove(member.Id);
            return Task.CompletedTask;
        }

        Task<bool> ITenantGrain.RegisterAgent(AgentInfo Info)
        {
            bool ret = true;
            Agents[Info.Id] = Info;
            return Task.FromResult(ret);
        }
        Task ITenantGrain.UnRegisterAgent(AgentInfo Info)
        {
            Agents.Remove(Info.Id);
            return Task.CompletedTask;
        }
        Task<bool> ITenantGrain.BatchRegisterAgent(long start, long end, string prefix, string deptid)
        {
            bool ret = true;
            for(long i= start;i <=end;i++)
            {
                string agentid = prefix + i.ToString();
                Agents[agentid] = new AgentInfo(agentid, agentid,deptid);
            }
            return Task.FromResult(ret);
        }
        Task ITenantGrain.BatchUnRegisterAgent(string[] agentIds)
        {
            foreach (string agentid in agentIds)
            {
                Agents.Remove(agentid);
            }
            return Task.CompletedTask;
        }
        Task ITenantGrain.BlockAgent(string agentid)
        {
            Agents[agentid].state = AgentState.Block;
            return Task.CompletedTask;
        }
        Task ITenantGrain.ResumeAgent(string agentid)
        {
            Agents[agentid].state = AgentState.Active;
            return Task.CompletedTask;
        }

        public class InboundAudioServiceNumberGrain : Grain, IInboundAudioServiceNumberGrain
        {
            InboundAudioNumberInfo myInfo;

            Dictionary<string, AudioRoomInfo> AudioRooms = new Dictionary<string, AudioRoomInfo>();
            Dictionary<string, CustomerInfo> Customers = new Dictionary<string, CustomerInfo>();

            Task<string> IInboundAudioServiceNumberGrain.Name()
            {
                return Task.FromResult(myInfo.Name);
            }
            Task IInboundAudioServiceNumberGrain.SetName(string name)
            {
                this.myInfo.Name = name;
                return Task.CompletedTask;
            }
            Task IInboundAudioServiceNumberGrain.SetInfo(string key, string value)
            {
                myInfo.Info[key] = value;

                return Task.CompletedTask;
            }

            Task IInboundAudioServiceNumberGrain.Enter(AudioRoomInfo member)
            {
                AudioRooms[member.Id] = member;
                return Task.CompletedTask;
            }
            Task IInboundAudioServiceNumberGrain.Exit(AudioRoomInfo member)
            {
                AudioRooms.Remove(member.Id);
                return Task.CompletedTask;
            }
            // Customer can enter or exit a InboundServiceNumber
            Task IInboundAudioServiceNumberGrain.Enter(CustomerInfo member)
            {
                Customers[member.Id] = member;
                return Task.CompletedTask;
            }
            Task IInboundAudioServiceNumberGrain.Exit(CustomerInfo member)
            {
                Customers.Remove(member.Id);
                return Task.CompletedTask;
            }
        }

        public class AgentQueueInfo
        {
            public AgentRuntimeInfo runtimeinfo { get; set; }
            public int postion { get; set; }
            public DateTime EnterTime { get; set; }
            public string LastServiceCustomer { get; set; }
        }
        public class QueueGrain : Grain, IQueueGrain
        {
            QueueInfo myInfo;
            Dictionary<string, AgentQueueInfo> AgentRuntimes = new Dictionary<string, AgentQueueInfo>();
            Dictionary<string, CustomerInfo> Customers = new Dictionary<string, CustomerInfo>();
            Dictionary<string, AudioRoomInfo> AudioRooms = new Dictionary<string, AudioRoomInfo>();
            SortedList<int, string> SortedAgentsbyPostion = new SortedList<int, string>();

            Task<string> IQueueGrain.Name()
            {
                return Task.FromResult(myInfo.Name);
            }
            Task IQueueGrain.SetName(string name)
            {
                this.myInfo.Name = name;
                return Task.CompletedTask;
            }
            Task IQueueGrain.SetInfo(string key, string value)
            {
                myInfo.Info[key] = value;

                return Task.CompletedTask;
            }

            Task IQueueGrain.Enter(AgentRuntimeInfo member,int postion)
            {
                AgentQueueInfo agentinfo = new AgentQueueInfo();
                agentinfo.runtimeinfo = member;
                agentinfo.postion = postion;
                agentinfo.EnterTime = DateTime.Now;
                AgentRuntimes[member.BaseInfo.Id] = agentinfo;
                return Task.CompletedTask;
            }
            Task IQueueGrain.Exit(AgentRuntimeInfo member)
            {
                AgentRuntimes.Remove(member.BaseInfo.Id);
                return Task.CompletedTask;
            }

            // Customer can enter or exit a queue
            Task IQueueGrain.Enter(CustomerInfo member, AudioRoomInfo room)
            {
                Customers[member.Id] = member;
                AudioRooms[member.Id] = room;
                return Task.CompletedTask;
            }
            Task IQueueGrain.Exit(CustomerInfo member)
            {
                Customers.Remove(member.Id);
                AudioRooms.Remove(member.Id);
                return Task.CompletedTask;
            }

            Task IQueueGrain.Block()
            {
                this.myInfo.state = QueueState.Block;
                return Task.CompletedTask;
            }
            Task IQueueGrain.Resume()
            {
                this.myInfo.state = QueueState.Active;
                return Task.CompletedTask;
            }


        }

        public class QueueWithAgentGrain : Grain, IQueueWithAgentGrain
        {

            Dictionary<string, Dictionary<string, AgentPostion>> QueueWithAgents = new Dictionary<string, Dictionary<string, AgentPostion>>();
            Dictionary<string, Dictionary<string,int>> AgentWithQueues = new Dictionary<string, Dictionary<string, int>>();

            Task AddAgentToQueue(string agentId, string queueId, int iter, int postion)
            {
                if (agentId == null || agentId == "")
                {
                    return Task.CompletedTask;
                }
                if (queueId == null || queueId == "")
                {
                    return Task.CompletedTask;
                }

                int pos = postion;
                if (postion < 1) pos = 1;
                if (postion > 99) pos = 99;

                int it = iter;
                if (iter < 1) it = 1;
                if (iter > 99) it = 99;

                int key = it * 100 + pos;
                var agentpos = new AgentPostion();
                agentpos.Iter = iter;
                agentpos.Postion = postion;

                if (QueueWithAgents.ContainsKey(queueId))
                {
                    if (QueueWithAgents[queueId].ContainsKey(agentId))
                        QueueWithAgents[queueId][agentId] = agentpos;
                    else
                    {
                        QueueWithAgents[queueId].Add(agentId, agentpos);
                    }
                }
                else
                {
                    QueueWithAgents.Add(queueId, new Dictionary<string, AgentPostion>());
                    QueueWithAgents[queueId].Add(agentId, agentpos);
                }

                if (AgentWithQueues.ContainsKey(agentId))
                {
                    if (AgentWithQueues[agentId].ContainsKey(queueId))
                        AgentWithQueues[agentId][agentId] = key;
                    else
                    {
                        AgentWithQueues[agentId].Add(queueId, key);
                    }
                }
                else
                {
                    AgentWithQueues[agentId] = new Dictionary<string, int>();
                    AgentWithQueues[agentId].Add(queueId, key);
                }
                return Task.CompletedTask;
            }
            Task IQueueWithAgentGrain.AddAgentToQueue(string agentId, string queueId, int iter, int postion)
            {
                AddAgentToQueue(agentId, queueId, iter, postion);
                return Task.CompletedTask;
            }
            Task IQueueWithAgentGrain.RemoveAgentToQueue(string agentId, string queueId)
            {
                RemoveAgentToQueue(agentId, queueId);
                return Task.CompletedTask;
            }
            Task RemoveAgentToQueue(string agentId, string queueId)
            {
                if (agentId == null || agentId == "")
                {
                    return Task.CompletedTask;
                }
                if (queueId == null || queueId == "")
                {
                    return Task.CompletedTask;
                }

                if (AgentWithQueues.ContainsKey(agentId))
                {
                    AgentWithQueues[agentId].Remove(queueId);
                }
                if (QueueWithAgents.ContainsKey(queueId))
                {
                    QueueWithAgents[queueId].Remove(queueId);
                }
                return Task.CompletedTask;
            }

            Task IQueueWithAgentGrain.BatchAddAgentToQueue(string[] agentIds, string queueId, int iter, int postion)
            {
                if (agentIds == null || agentIds.Count() == 0)
                {
                    return Task.CompletedTask;
                }
                if (queueId == null || queueId == "")
                {
                    return Task.CompletedTask;
                }
                foreach(string agentId in agentIds)
                {
                    AddAgentToQueue(agentId, queueId, iter, postion);
                }
                return Task.CompletedTask;
            }
            Task IQueueWithAgentGrain.BatchRemoveAgentToQueue(string[] agentIds, string queueId)
            {
                if (agentIds == null || agentIds.Count() == 0)
                {
                    return Task.CompletedTask;
                }
                if (queueId == null || queueId == "")
                {
                    return Task.CompletedTask;
                }
                foreach (string agentId in agentIds)
                {
                    RemoveAgentToQueue(agentId, queueId);
                }                
                return Task.CompletedTask;
            }

            Task<Dictionary<string, int>> IQueueWithAgentGrain.QueryQueues(string agentId)
            {
                if (AgentWithQueues.ContainsKey(agentId))
                    return Task.FromResult(AgentWithQueues[agentId]);
                else
                    return Task.FromResult(new Dictionary<string, int>());
            }
            Task<Dictionary<string, AgentPostion>> IQueueWithAgentGrain.QueryAgents(string queueId)
            {
                if (QueueWithAgents.ContainsKey(queueId))
                    return Task.FromResult(QueueWithAgents[queueId]);
                else
                    return Task.FromResult(new Dictionary<string, AgentPostion>());
            }

            Task IQueueWithAgentGrain.ClearAgent(string agentId)
            {
                if (AgentWithQueues.ContainsKey(agentId))
                    AgentWithQueues[agentId] = new Dictionary<string, int>();
                return Task.CompletedTask;
            }
            Task IQueueWithAgentGrain.ClearQueue(string queueId)
            {
                if (QueueWithAgents.ContainsKey(queueId))
                    QueueWithAgents[queueId] = new Dictionary<string, AgentPostion>();
                return Task.CompletedTask;
            }
        }

        public class AgentRuntimeGrain : Grain, IAgentRuntimeGrain
        {
            AgentRuntimeInfo myInfo;

            public override Task OnActivateAsync()
            {
                this.monsterInfo.Id = this.GetPrimaryKeyString();

                RegisterTimer((_) => Move(), null, TimeSpan.FromSeconds(150), TimeSpan.FromMinutes(150));
                return base.OnActivateAsync();
            }
            //Task<string> Description();
            Task SetInfo(string key, string value);
            Task SetRuntimeInfo(string key, string value);
            Task SetPhoneStatus(string status);

            Task InCall(CustomerInfo member, AudioRoomInfo room);
            Task Accept();
            Task Reject();
        }
    }
}
