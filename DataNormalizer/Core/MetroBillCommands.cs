using DevComponents.DotNetBar;

namespace DataNormalizer.Core
{
    /// <summary>
    /// Represents all application commands.
    /// </summary>
    public class MetroBillCommands
    {
        private StartControlCommands _startControlCommands = new StartControlCommands();

        public StartControlCommands StartControlCommands
        {
            get { return _startControlCommands; }
            set { _startControlCommands = value; }
        }
    }

    public class StartControlCommands
    {
        public Command Logon { get; set; }
        public Command Exit { get; set; }
    }

}

