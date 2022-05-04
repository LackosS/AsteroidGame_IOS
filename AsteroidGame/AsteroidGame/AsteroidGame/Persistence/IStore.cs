using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsteroidGame.Persistence
{
    public interface IStore
    {
        Task<IEnumerable<String>> GetFiles();
        
        Task<DateTime> GetModifiedTime(String name);
    }
}