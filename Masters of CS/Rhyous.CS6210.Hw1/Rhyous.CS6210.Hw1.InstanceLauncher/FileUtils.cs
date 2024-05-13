using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.AmazonS3BucketManager
{
    public static class FileUtils
    {
        public static async Task<List<string>> GetFiles(string directory, bool recursive)
        {
            var files = Directory.GetFiles(directory).ToList();
            if (!recursive)
                return files;
            var dirs = Directory.GetDirectories(directory);
            var tasks = dirs.Select(d => GetFiles(d, recursive)).ToList();
            while (tasks.Any())
            {
                var task = await Task.WhenAny(tasks);
                files.AddRange(task.Result);
                tasks.Remove(task);
            }
            return files;
        }
    }
}