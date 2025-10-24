namespace InhibitSleep.Net;

/// <summary>
/// A null implementation of the <see cref="ISleepInhibitor"/> which does nothing when called.
/// </summary>
public class NullSleepInhibitor : ISleepInhibitor
{
    public bool IsSupported => true;

    public void InhibitSleep() { }

    public void ReleaseInhibition() { }
}
