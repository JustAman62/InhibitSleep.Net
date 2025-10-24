using InhibitSleep.Net;

Console.WriteLine("Starting InhibitSleep.Net Sample Project");

Console.WriteLine("Requesting Inhibition");

var inhibitor = new SleepInhibitor("InhibitSleep.Net Sample");

inhibitor.InhibitSleep();

Console.WriteLine("Sleep Inhibition requested");
Console.WriteLine("Press c to release the inhibition");

var key = Console.ReadKey();
while (key.Key != ConsoleKey.C)
{
    key = Console.ReadKey();
}

Console.WriteLine("");
Console.WriteLine("Releasing Inhibition");

inhibitor.ReleaseInhibition();

Console.WriteLine("Released Inhibition");
