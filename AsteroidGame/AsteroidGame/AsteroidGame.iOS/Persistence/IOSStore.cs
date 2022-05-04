using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AsteroidGame.IOS.Persistence;
using AsteroidGame.Persistence;
using Xamarin.Forms;

[assembly: Dependency(typeof(IOSStore))]
namespace AsteroidGame.IOS.Persistence
{
    public class IOSStore : IStore
    {
        public async Task<IEnumerable<String>> GetFiles()
        {
            return await Task.Run(() => Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).Select(file => Path.GetFileName(file)));
        }
        
        public async Task<DateTime> GetModifiedTime(String name)
        {
            FileInfo info = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), name));

            return await Task.Run(() => info.LastWriteTime);
        }
    }
}