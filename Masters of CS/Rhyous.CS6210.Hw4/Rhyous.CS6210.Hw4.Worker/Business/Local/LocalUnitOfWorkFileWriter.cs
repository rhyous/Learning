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
    public class LocalUnitOfWorkFileWriter : IUnitOfWorkFileWriter
    {
        public async Task WriteAsync(string directory, UnitOfWork unitOfWork)
        {
            if (!Directory.Exists(directory))
                return;
            var fileFullPath = Path.Combine(directory, $"{unitOfWork.Id}.json");
            var json = JsonConvert.SerializeObject(unitOfWork);
            using (StreamWriter writer = new StreamWriter(fileFullPath))
            {
                await writer.WriteAsync(json);
                await writer.FlushAsync();
                writer.Close();
            }
        }

        public async Task MoveAsync(string source, string destination)
        {
            if (!File.Exists(source))
                return;
            var dir = Path.GetDirectoryName(destination);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            try { File.Move(source, destination); }
            catch (Exception e){ }
        }
    }
}
