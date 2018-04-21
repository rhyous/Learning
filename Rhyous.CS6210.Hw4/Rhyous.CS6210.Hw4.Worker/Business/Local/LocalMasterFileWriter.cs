using Newtonsoft.Json;
using Rhyous.CS6210.Hw4.Models;
using System.IO;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{
    class LocalMasterFileWriter : IMasterFileWriter
    {
        public async Task WriteAsync(string masterDirectory, Master master)
        {
            if (!Directory.Exists(masterDirectory))
                return;
            var fileFullPath = Path.Combine(masterDirectory, $"{master.ToString()}.json");
            var json = JsonConvert.SerializeObject(master);
            using (StreamWriter writer = new StreamWriter(fileFullPath))
            {
                await writer.WriteAsync(json);
                await writer.FlushAsync();
                writer.Close();
            }
        }
    }
}