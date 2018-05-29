using Orleans;
using System;
using System.Threading.Tasks;

namespace AudioPAAS.Interfaces
{
    public interface IMediaServerGrain : IGrainWithGuidKey
    {
        // Rooms have a textual description
        Task<string> Description();
        Task SetInfo(MediaServerInfo info);

        Task<IAudioRoomGrain> CreateAudioRoom(Guid guid);
        Task<IAudioRoomGrain> ReleaseAudioRoom(Guid guid);

    }

    public interface IAudioRoomGrain : IGrainWithGuidKey
    {
        Task SetInboundServiceNumberInfo(InboundAudioNumberInfo info);

        // Customer can enter or exit a room
        Task Enter(CustomerInfo member);
        Task Exit(CustomerInfo member);

        // Ivr can enter or exit a room
        Task Enter(IvrInfo member);
        Task Exit(IvrInfo member);

        // Agent can enter or exit a room
        Task Enter(AgentRuntimeInfo member);
        Task Exit(AgentRuntimeInfo member);

        Task<bool> Transfer(AgentInfo member);
        Task<string> Execute(string command);

    }

    public interface ICustomerGrain : IGrainWithStringKey
    {
        // Players have names
        Task<string> Name();
        Task SetName(string name);

        // Each player is located in exactly one room
        Task SetAudioRoomGrain(IAudioRoomGrain room);
        Task<IAudioRoomGrain> AudioRoomGrain();

        // Until Death comes knocking
        Task Die();

        // A Player takes his turn by calling Play with a command
        Task<string> Play(string command);

    }
}

