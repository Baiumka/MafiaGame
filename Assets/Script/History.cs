
using NUnit.Framework;
using System;
using System.Collections.Generic;

public class History 
{
    private List<HistoryEvent> events = new List<HistoryEvent>();
    public List<HistoryEvent> Events { get => events; }


    public History()
    {
        events = new List<HistoryEvent>();
    }

    
    public void Add(Player player, Player target, EventType eventType)
    {
        events.Add(new HistoryEvent(player,target,eventType));
    }

    
    public class HistoryEvent
    {
        public Player player;
        public Player target;
        public EventType type;
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
