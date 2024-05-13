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
    public class LocalMasterFileReader : IMasterFileReader
    {
        public async Task<Master> ReadAsync(string masterDirectory)
        {
            if (!Directory.Exists(masterDirectory))
                return null;
            var masterFiles = Directory.GetFiles(masterDirectory, "*.json", SearchOption.TopDirectoryOnly);
            if (masterFiles == null || !masterFiles.Any())
                return null;
            var currentMasterFile = masterFiles.OrderByDescending(f=>f).First();
            var text = await File.ReadAllTextAsync(currentMasterFile);
            var master = JsonConvert.DeserializeObject<Master>(text);
            return master;
        }
    }
}
