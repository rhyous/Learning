using Rhyous.CS6210.Hw4.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{
    public class LocalDirectoryPreparerAsync : IDirectoryPreparerAsync
    {
        public async Task<bool> DirectoryExistsAsync(string dir)
        {
            return Directory.Exists(dir);
        }

        public async Task<bool> FileExistsAsync(string file)
        {
            return File.Exists(file);
        }

        public async Task<bool> PrepareAync(string primaryDirectory, IEnumerable<string> subdirs)
        {
            if (string.IsNullOrWhiteSpace(primaryDirectory))
                throw new ArgumentException("primaryDirectory", string.Format(Messages.ListNullOrEmpty, "primaryDirectory"));
            if (subdirs == null || !subdirs.Any())
                throw new ArgumentException("subdirs", string.Format(Messages.ListNullOrEmpty, "subdirs"));
            if (subdirs.Any(d => string.IsNullOrWhiteSpace(d)))
                throw new ArgumentException("subdirs", string.Format(Messages.StringListNullEmptyOrWhiteSpace, "sbudirs"));
            return await Task.Run(() =>
            {
                if (!CreateDirectoryIfNotExists(primaryDirectory))
                    return false;
                foreach (var subdir in subdirs)
                {
                    var dir = Path.Combine(primaryDirectory, subdir);
                    if (!CreateDirectoryIfNotExists(dir))
                        return false;
                }
                return true;
            });
        }

        internal bool CreateDirectoryIfNotExists(string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return Directory.Exists(dir);
        }
    }
}
