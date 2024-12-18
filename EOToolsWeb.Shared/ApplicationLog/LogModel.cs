namespace EOToolsWeb.Shared.ApplicationLog;

public abstract class LogModel
{
    public int Id { get; set; }

    public abstract string LogDetail { get; }
}