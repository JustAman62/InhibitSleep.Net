# InhibitSleep.Net

InhibitSleep.Net allows your desktop application to prevent the machine it's running on from entering sleep.
It does this by sending a request to the Operating System using native API calls for the currently running thread.

The main purpose of this library is to provide a cross-platform abstraction for native API calls for cross-platform
apps to use.

## Supported Operating Systems

InhibitSleep.Net currently only supports Windows and macOS. Attempting to call `InhibitSleep()` on an unsupported 
platform will result in a `NotSupportedException` being thrown. There is a `NullSleepInhibitor` provided to ease
integration with applications that may run on unsupported platforms. `SleepInhibitor` will use this null implementation
if an implementation for the current platform is not found, to prevent `NotSupportedException`s being thrown at runtime
from the underlying implementations.

## Usage

The libraries interface is very simple: you simply construct a `SleepInhibitor`; then use its `InhibitSleep()` and 
`ReleaseInhibition()` methods to request and release inhibitions respectively.

```cs
using var inhibitor = new SleepInhibitor();
inhibitor.InhibitSleep();

// ... device will not sleep for operations between these two method calls

// Calling ReleaseInhibition is optional if you use a `using` directive, 
// as calling `Dispose()` will also release the inhibition
inhibitor.ReleaseInhibition();
```

`SleepInhibitor` does implement the `IDisposable` interface for ergonomic purposes, but it is not required to call `.Dispose()`
if you have already called `ReleaseInhibition()`. You can also request and release inhibitions as many times as you want with
the same instance of the class.

Ideally, you should call `SleepInhibitor` methods on the applications main thread, as on some operating systems the inhibition
will be applied to the thread that made the call, so if that thread dies then the inhibition may unexpectedly disappear.
If this is not possible, ensure you call `InhibitSleep()` on a thread with the same lifetime as any functionality which requires
the inhibition.

A sample console app is provided in the [`sample/`](sample/) directory if you'd like to see a working example.

## Platform specific implementations

The `SleepInhibitor` automatically picks the correct underlying implementation for the current operating system and redirects calls
to it. The platform specific implementations of `ISleepInhibitor` are available for use if you would like though.

Any usages of a platform specific implementation _MUST_ only be done on supported platforms. A `IsSupported` static property is available
on each implementation which will return `true` if that class can be used. Calling, for example, `new MacSleepInhibitor().InhibitSleep()`
on a non-mac device will result in a `NotSupportedException` being thrown.

### Mac

The macOS implementation `MacSleepInhibitor` uses IOKit to create an assertion for the currently running process, requesting
that the device not go to sleep.

```cs
if (MacSleepInhibitor.IsSupported)
{
    using var inhibitor = new MacSleepInhibitor();
    inhibitor.InhibitSleep();

    // current process will have a NoDisplaySleepAssertion assertion applied
}
```

### Windows

The Windows implementation `WindowsSleepInhibitor` uses Win32 `ThreadExecutionState`s to mark the currently executing thread as
requiring an active display, thereby preventing the device from sleeping.

```cs
if (WindowsSleepInhibitor.IsSupported)
{
    using var inhibitor = new WindowsSleepInhibitor();
    inhibitor.InhibitSleep();

    // current thread state will have a ES_DISPLAY_REQUIRED applied
}
```

### Other Operating Systems

I have plans for supporting Linux, which requires some more thought (as using DBus isn't always possible, 
and somewhat complicated to implement). 
If supporting Linux is a requirement for you, please raise an issue so I know it's a wanted feature!
