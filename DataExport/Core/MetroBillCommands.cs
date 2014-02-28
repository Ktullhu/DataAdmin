using DevComponents.DotNetBar;

namespace DataExport.Core
{
    /// <summary>
    /// Represents all application commands.
    /// </summary>
    public class MetroBillCommands
    {
        public StartControlCommands StartControlCommands { get; set; }
        public CustomFormulaControlCommands CustomFormulaControlCommands { get; set; }
        public ScheduleJobControlCommands ScheduleJobControlCommands { get; set; }

        public MetroBillCommands()
        {
            StartControlCommands = new StartControlCommands{Logon = new Command(),  Exit = new Command()};
            CustomFormulaControlCommands = new CustomFormulaControlCommands { Save = new Command(), Cancel = new Command() };
            ScheduleJobControlCommands = new ScheduleJobControlCommands { Save = new Command(), Cancel = new Command() };
        }
    }

    public class StartControlCommands
    {
        public Command Logon { get; set; }
        public Command Exit { get; set; }
    }
    public class CustomFormulaControlCommands
    {
        public Command Save { get; set; }
        public Command Cancel { get; set; }
    }
    public class ScheduleJobControlCommands
    {
        public Command Save { get; set; }
        public Command Cancel { get; set; }
    }
}
