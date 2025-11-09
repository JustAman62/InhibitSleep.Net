using System.Runtime.InteropServices;

namespace InhibitSleep.Net;

/// <inheritdoc />
public class WindowsSleepInhibitor : ISleepInhibitor
{
    /// <inheritdoc />
    public static bool IsSupported { get; } = OperatingSystem.IsWindows();

    /// <inheritdoc />
    public void InhibitSleep()
    {
        ThrowIfNotSupported();

        _ = SetThreadExecutionState(
            ExecutionStateEnum.ES_CONTINUOUS
                | ExecutionStateEnum.ES_SYSTEM_REQUIRED
                | ExecutionStateEnum.ES_DISPLAY_REQUIRED
        );
    }

    /// <inheritdoc />
    public void ReleaseInhibition()
    {
        ThrowIfNotSupported();

        _ = SetThreadExecutionState(ExecutionStateEnum.ES_CONTINUOUS);
    }

    private void ThrowIfNotSupported()
    {
        if (!IsSupported)
        {
            throw new NotSupportedException(
                $"{nameof(WindowsSleepInhibitor)} can only be used on Windows devices"
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
