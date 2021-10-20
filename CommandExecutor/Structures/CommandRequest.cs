namespace CommandExecutor.Structures
{
    public class CommandRequest
    {
        public string Command { get; set; }
        
        public string[] Arguments { get; set; }
    }
}