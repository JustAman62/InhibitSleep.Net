namespace InhibitSleep.Net;

public interface ISleepInhibitor : IDisposable
{
    /// <summary>
    /// Returns <see langword="true"/> if the current platform is supported. Attempting to call
    /// <see cref="InhibitSleep"/> on an unsupported platform will result in a <see cref="NotSupportedException"/>
    /// being thrown.
    /// </summary>
    public static bool IsSupported { get; }

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

    /// <summary>
    /// Disposes of any currently active sleep inhibition.
    /// </summary>
    void IDisposable.Dispose()
    {
        ReleaseInhibition();
        GC.SuppressFinalize(this);
    }
}
