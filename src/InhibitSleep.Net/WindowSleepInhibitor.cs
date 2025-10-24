using System.Runtime.InteropServices;

namespace InhibitSleep.Net;

/// <inheritdoc />
public class WindowsSleepInhibitor : ISleepInhibitor
{
    /// <summary>
    /// The default instance of <see cref="WindowsSleepInhibitor"/>
    /// </summary>
    public static WindowsSleepInhibitor Default { get; } = new();

    /// <inheritdoc />
    public void InhibitSleep()
    {
        EnsureWindowsOs();

        _ = SetThreadExecutionState(
            ExecutionStateEnum.ES_CONTINUOUS
                | ExecutionStateEnum.ES_SYSTEM_REQUIRED
                | ExecutionStateEnum.ES_DISPLAY_REQUIRED
        );
    }

    /// <inheritdoc />
    public void ReleaseInhibition()
    {
        EnsureWindowsOs();
        _ = SetThreadExecutionState(ExecutionStateEnum.ES_CONTINUOUS);
    }

    private void EnsureWindowsOs()
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new InvalidOperationException(
                $"${nameof(WindowsSleepInhibitor)} can only be used on Windows devices"
            );
        }
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern uint SetThreadExecutionState(ExecutionStateEnum esFlags);

    [Flags]
    private enum ExecutionStateEnum : uint
    {
        ES_CONTINUOUS = 0x80000000,
        ES_SYSTEM_REQUIRED = 0x00000001,
        ES_DISPLAY_REQUIRED = 0x00000002,
    }
}
