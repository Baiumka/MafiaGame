
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

    public void Add(Player player, Player target, EventType eventType, string dop, int dop_n)
    {
        HistoryEvent kek = new HistoryEvent(player, target, eventType, dop, dop_n);
        events.Add(kek);
        onHistoryAdded?.Invoke(kek);
    }

    public void Add(Player player, Player target, EventType eventType, int dop_n)
    {
        HistoryEvent kek = new HistoryEvent(player, target, eventType, dop_n);
        events.Add(kek);
        onHistoryAdded?.Invoke(kek);
    }

    public void Add(Player player, Player target, EventType eventType, string dop)
    {
        HistoryEvent kek = new HistoryEvent(player, target, eventType, dop);
        events.Add(kek);
        onHistoryAdded?.Invoke(kek);
    }


    public class HistoryEvent
    {
        public Player player;
        public Player target;
        [JsonConverter(typeof(StringEnumConverter))] public EventType type;
        public DateTime dateTime;
        public string dop;
        public int dop_n;
        public HistoryEvent(Player player, Player target, EventType type) 
        {
            this.player = player;
            this.target = target;
            this.type = type;
            this.dateTime = DateTime.Now;
            this.dop = "";
            this.dop_n = 0;
        }

        public HistoryEvent(Player player, Player target, EventType type, string dop)
        {
            this.player = player;
            this.target = target;
            this.type = type;
            this.dateTime = DateTime.Now;
            this.dop = dop;
            this.dop_n = 0;
        }

        public HistoryEvent(Player player, Player target, EventType type, int dop_n)
        {
            this.player = player;
            this.target = target;
            this.type = type;
            this.dateTime = DateTime.Now;
            this.dop_n = dop_n;
            this.dop = "";
        }

        public HistoryEvent(Player player, Player target, EventType type, string dop, int dop_n)
        {
            this.player = player;
            this.target = target;
            this.type = type;
            this.dateTime = DateTime.Now;
            this.dop = dop;
            this.dop_n = dop_n;
        }


    }
}
