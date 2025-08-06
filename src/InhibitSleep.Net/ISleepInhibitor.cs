namespace InhibitSleep.Net;

/// <summary>
/// Defines the interface to request and release sleep inhibitions.
/// </summary>
public interface ISleepInhibitor
{
    /// <summary>
    /// Requests the OS to prevent sleep until the inhibition is released.
    /// This request is only valid for the Thread that it is called from, 
    /// and only has any effect for the lifetime of that thread.
    /// </summary>
    public void InhibitSleep();

    /// <summary>
    /// Releases any currently held inhibition by informing the OS that
    /// sleep no longer needs to be prevented.
    /// </summary>
    public void ReleaseInhibition();
}