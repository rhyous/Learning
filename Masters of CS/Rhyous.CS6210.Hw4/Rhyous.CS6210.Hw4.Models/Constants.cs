namespace Rhyous.CS6210.Hw4.Models
{
    public class Messages
    {
        // Exception strings
        public const string ObjectNull = "The object '{0}' cannot be null.";
        public const string StringNullEmptyOrWhiteSpace = "The string '{0}' cannot be null, empty, or whitespace.";
        public const string ListNullOrEmpty = "The list '{0}' cannot be null or empty.";
        public const string StringListNullEmptyOrWhiteSpace = "The list '{0}' cannot contain a null, empty, or whitspace item.";
        public const string FolderStructureNotPrepared = "The folder structure is not prepared.";
        public const string PortNoInRange = "Port must be provided and must be in this range: 1024-65535";
    }
}
