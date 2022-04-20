using System;
using System.Collections.Generic;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game.GameData;
using FollowBot.SimpleEXtensions.CachedObjects;

namespace FollowBot.SimpleEXtensions
{
    public class AreaChangedArgs : EventArgs
    {
        public uint OldHash { get; }
        public uint NewHash { get; }
        public DatWorldAreaWrapper OldArea { get; }
        public DatWorldAreaWrapper NewArea { get; }

        public AreaChangedArgs(uint oldHash, uint newHash, DatWorldAreaWrapper oldArea, DatWorldAreaWrapper newArea)
        {
            OldHash = oldHash;
            NewHash = newHash;
            OldArea = oldArea;
            NewArea = newArea;
        }

        public AreaChangedArgs(Message message)
        {
            OldHash = message.GetInput<uint>(0);
            NewHash = message.GetInput<uint>(1);
            OldArea = message.GetInput<DatWorldAreaWrapper>(2);
            NewArea = message.GetInput<DatWorldAreaWrapper>(3);
        }
    }

    public class ItemsSoldArgs : EventArgs
    {
        public List<CachedItem> SoldItems { get; }
        public List<CachedItem> GainedItems { get; }

        public ItemsSoldArgs(List<CachedItem> soldItems, List<CachedItem> gainedItems)
        {
            SoldItems = soldItems;
            GainedItems = gainedItems;
        }

        public ItemsSoldArgs(Message message)
        {
            SoldItems = message.GetInput<List<CachedItem>>(0);
            GainedItems = message.GetInput<List<CachedItem>>(1);
        }
    }
}
