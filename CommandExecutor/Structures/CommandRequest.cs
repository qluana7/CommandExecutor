namespace CommandExecutor.Structures
{
    /// <summary>
    /// </summary>
    public class CommandRequest
    {
        public string Command { get; set; }
        
        public string[] Arguments { get; set; }
    }
}