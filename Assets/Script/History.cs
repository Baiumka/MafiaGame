
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static History;


public delegate void HistoryHandler(HistoryEvent historyEvent);

public class History 
{
    private List<HistoryEvent> events = new List<HistoryEvent>();
    public List<HistoryEvent> Events { get => events; }

    public HistoryHandler onHistoryAdded;

    public History()
    {
        events = new List<HistoryEvent>();
    }

    
    public void Add(Player player, Player target, EventType eventType)
    {
        HistoryEvent kek = new HistoryEvent(player, target, eventType);
        events.Add(kek);
        onHistoryAdded?.Invoke(kek);
    }

    
    public class HistoryEvent
    {
        public Player player;
        public Player target;
        [JsonConverter(typeof(StringEnumConverter))] public EventType type;
        public DateTime dateTime;
        public HistoryEvent(Player player, Player target, EventType type) 
        {
            this.player = player;
            this.target = target;
            this.type = type;
            dateTime = DateTime.Now;
        }
    }
}
