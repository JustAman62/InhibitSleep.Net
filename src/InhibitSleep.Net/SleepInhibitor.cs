namespace InhibitSleep.Net;

/// <summary>
/// Requests and releases sleep inhibitions from the operating system.
/// </summary>
public class SleepInhibitor : ISleepInhibitor
{
    private ISleepInhibitor _inhibitor;

    public SleepInhibitor(string identifier)
    {
        if (OperatingSystem.IsMacOS())
        {
            _inhibitor = new MacSleepInhibitor(identifier);
        }
        else if (OperatingSystem.IsWindows())
        {
            _inhibitor = new WindowsSleepInhibitor();
        }
        else
        {
            throw new NotSupportedException(
                $"InhibitSleep.Net is not supported on the current operating system."
            );
        }
    }

    public void InhibitSleep() => _inhibitor.InhibitSleep();

    public void ReleaseInhibition() => _inhibitor.ReleaseInhibition();
}
