﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface ISubscriberRepository
    {
        Task<IReadOnlyCollection<Subscriber>> GetHistory(string channelId, int days);
    }
}
