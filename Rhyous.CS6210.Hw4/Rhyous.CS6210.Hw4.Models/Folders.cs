using System.Collections.ObjectModel;

namespace Rhyous.CS6210.Hw4.Models
{
    public class Folders
    {
        // Folders
        public const string Logs = "Logs";
        public const string Master = "Master";
        public const string Sleep = "Sleep";
        public const string WorkCompleted = "WorkCompleted";
        public const string WorkInProgress = "WorkInProgress";
        public const string WorkQueue = "WorkQueue";
        public const string WorkTerminated = "WorkTerminated";
        public static readonly ReadOnlyCollection<string> SubDirs = new ReadOnlyCollection<string>(
            new[] { Logs, Master, Sleep, WorkCompleted, WorkInProgress, WorkQueue, WorkTerminated }
        );

    }
}
