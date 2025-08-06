using System.Runtime.InteropServices;

namespace InhibitSleep.Net;

public class MacSleepInhibitor(string assertionName) : ISleepInhibitor
{
    private const string IOKitLib = "/System/Library/Frameworks/IOKit.framework/IOKit";
    private const string CoreFoundationLib =
        "/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation";
    private const string AssertionType = "NoDisplaySleepAssertion";

    public uint assertionId;

    public void InhibitSleep()
    {
        PreventDisplaySleep();
    }

    public void ReleaseInhibition()
    {
        if (assertionId != 0)
        {
            IOPMAssertionRelease(assertionId);
            assertionId = 0;
        }
    }

    private void PreventDisplaySleep()
    {
        var assertionType = CFStringCreateWithCString(
            IntPtr.Zero,
            AssertionType,
            kCFStringEncodingUTF8
        );
        var name = CFStringCreateWithCString(IntPtr.Zero, assertionName, kCFStringEncodingUTF8);

        var result = IOPMAssertionCreateWithName(
            assertionType,
            IOPMAssertionLevel.On,
            name,
            out assertionId
        );

        if (result != 0)
        {
            throw new Exception($"IOPMAssertionCreateWithName failed: {result}");
        }
    }

    /// <summary>
    /// Creates an assertions using IOKit.
    /// </summary>
    /// <param name="assertionType">Pointer to a <c>CFStringRef</c> specifying what type of assertion to make.</param>
    /// <param name="level">The <see cref="IOPMAssertionLevel"/>.</param>
    /// <param name="assertionName">Pointer to a <c>CFStringRef</c> with a friendly name for the assertion..</param>
    /// <param name="assertionID">Returns an ID of the assertion that can be used to release it.</param>
    /// <returns>A code indicating a success or failure</returns>
    [DllImport(IOKitLib, CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int IOPMAssertionCreateWithName(
        IntPtr assertionType, // CFStringRef
        IOPMAssertionLevel level,
        IntPtr assertionName, // CFStringRef
        out uint assertionID
    );

    /// <summary>
    /// Releases a previously acquired lock.
    /// </summary>
    /// <param name="assertionID">The ID of the previous acquired lock.</param>
    /// <returns>A code indicating a success or failure</returns>
    [DllImport(IOKitLib, CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int IOPMAssertionRelease(uint assertionID);

    /// <summary>
    /// Creates a <c>CFString</c> which is reqired for other IOKit calls using a C String.
    /// </summary>
    /// <param name="alloc">A pointer to some memory where to allocate this string.</param>
    /// <param name="cStr">The C String to convert.</param>
    /// <param name="encoding">A flag indicating the character encoding to use.</param>
    /// <returns>A pointer to the newly allocated <c>CFString</c></returns>
    [DllImport(CoreFoundationLib, CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CFStringCreateWithCString(
        IntPtr alloc,
        string cStr,
        uint encoding
    );

    private const uint kCFStringEncodingUTF8 = 0x08000100;

    private enum IOPMAssertionLevel : uint
    {
        Off = 0x00,
        On = 0xFF,
    }
}
