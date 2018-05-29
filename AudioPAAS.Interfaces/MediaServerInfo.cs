using System;
using Orleans.Concurrency;
using System.Collections.Generic;

namespace AudioPAAS.Interfaces
{
    [Serializable]
    [Immutable]
    public class MediaServerInfo
    {
        public Guid Id { get; set; }
        public string ListenIP { get; set; }
        public ulong ListenPort { get; set; }
        public ulong MaxAudioRoomCount { get; set; }
        public ulong MaxVideoRoomCount { get; set; }

    }


    [Serializable]
    [Immutable]
    public class AudioRoomInfo
    {
        public string Id { get; set; }
        //inbound outbound
        public string Type { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, long> Members { get; set; }
    }
}
