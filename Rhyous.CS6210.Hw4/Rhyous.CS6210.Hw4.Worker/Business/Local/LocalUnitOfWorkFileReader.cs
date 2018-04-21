using Newtonsoft.Json;
using Rhyous.CS6210.Hw4.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{
    public class LocalUnitOfWorkFileReader : IUnitOfWorkFileReader
    {
        public async Task<List<string>> GetFilesAsync(string queueDir, string filter = null)
        {
            return Directory.GetFiles(queueDir, filter).ToList();
        }

        public async Task<string> GetNextFileAsync(string unitOfWorkDirectory)
        {
            if (!Directory.Exists(unitOfWorkDirectory))
                return null;
            var unitOfWorkFiles = Directory.GetFiles(unitOfWorkDirectory, "*.json", SearchOption.TopDirectoryOnly);
            return unitOfWorkFiles?.First();
        }

        public async Task<string> ReadAllTextAsync(string pathToFile)
        {
            return await File.ReadAllTextAsync(pathToFile);
        }

        public async Task<UnitOfWork> ReadAsync(string filepath)
        { 
            var json = await ReadAllTextAsync(filepath);
            return JsonConvert.DeserializeObject<UnitOfWork>(json);
        }        
    }
}
