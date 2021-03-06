﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TkokDiscordBot.Entities;

namespace TkokDiscordBot.Data.Abstractions
{
    public interface IItemsStore
    {
        Task<IEnumerable<Item>> GetAllAsync();

        Task ReSyncItems();
    }
}