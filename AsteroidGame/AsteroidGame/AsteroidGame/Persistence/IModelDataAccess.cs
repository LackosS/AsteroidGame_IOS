using System;
using System.Threading.Tasks;
using AsteroidGame.Model;

namespace AsteroidGame.Persistence
{
    public interface IModelDataAccess
    {
        Task<ModelTable> LoadAsync(String path);
        Task SaveAsync(String path, GameModel model);
    }
}