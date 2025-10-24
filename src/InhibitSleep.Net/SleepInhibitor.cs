namespace InhibitSleep.Net;

/// <summary>
/// Requests and releases sleep inhibitions from the operating system.
/// </summary>
public class SleepInhibitor : ISleepInhibitor
{
    private readonly ISleepInhibitor _inhibitor;

    /// <summary>
    /// Constructs a <see cref="SleepInhibitor"/> which abstracts away platform specific implementations.
    /// </summary>
    /// <param name="identifier">
    /// Identifier that any assertion will be associated with. Should be something like the name of the app requesting
    /// the OS not to sleep.
    /// </param>
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
            _inhibitor = new NullSleepInhibitor();
        }
    }

    /// <inheritdoc />
    public static bool IsSupported { get; } =
        MacSleepInhibitor.IsSupported || WindowsSleepInhibitor.IsSupported;

    /// <inheritdoc />
    public void InhibitSleep() => _inhibitor.InhibitSleep();

    /// <inheritdoc />
    public void ReleaseInhibition() => _inhibitor.ReleaseInhibition();
}
