using System;
using System.IO;
using System.Threading.Tasks;
using AsteroidGame.iOS.Persistence;
using AsteroidGame.IOS.Persistence;
using AsteroidGame.Model;
using AsteroidGame.Persistence;
using Xamarin.Forms;

[assembly: Dependency(typeof(IOSDataAccess))]
namespace AsteroidGame.iOS.Persistence
{
    public class IOSDataAccess : IModelDataAccess
    {
        public async Task<ModelTable> LoadAsync(String path)
        {
            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);
            
            String[] values = (await Task.Run(() => File.ReadAllText(filePath))).Split(' ');
            
            Int32 tableSize = Int32.Parse(values[0]);
            Int32 gameTime = Int32.Parse(values[1]);
            ModelTable table = new ModelTable(tableSize);
            table.GameTime = gameTime;
            
            Int32 valueIndex = 2;
            for (Int32 rowIndex = 0; rowIndex < tableSize; rowIndex++)
            {              
                for (Int32 columnIndex = 0; columnIndex < tableSize; columnIndex++)
                {
                    table.SetValue(rowIndex, columnIndex, Int32.Parse(values[valueIndex]));
                    if (Int32.Parse(values[valueIndex]) == 1) table.SpaceshipPos = columnIndex;
                    valueIndex++;
                }
            }

            return table;
        }
        
        public async Task SaveAsync(String path, GameModel model)
        {
            String text = model.Table.Size.ToString() + " " + model.TimerCount.ToString()+ " ";

            for (Int32 i = 0; i < model.Table.Size; i++)
            {
                for (Int32 j = 0; j < model.Table.Size; j++)
                {
                    text += model.Table[i, j] + " ";
                }
            }
            
            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);
            
            await Task.Run(() => File.WriteAllText(filePath, text));
        }
    }
}