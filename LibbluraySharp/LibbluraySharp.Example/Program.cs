using System;
using LibbluraySharp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("LibbluraySharp Example");
        Console.WriteLine("=====================");

        // Get libbluray version
        int major, minor, micro;
        LibbluraySharp.Native.NativeMethods.bd_get_version(out major, out minor, out micro);
        Console.WriteLine($"libbluray version: {major}.{minor}.{micro}");

        // Example usage (note: requires a real Blu-ray disc path to work)
        if (args.Length > 0)
        {
            string discPath = args[0];
            Console.WriteLine($"Opening disc: {discPath}");
            
            using var bluray = Bluray.OpenDisc(discPath);
            if (bluray != null && bluray.IsOpen)
            {
                Console.WriteLine("Disc opened successfully!");
                
                var discInfo = bluray.GetDiscInfo();
                if (discInfo != null)
                {
                    Console.WriteLine($"Blu-ray detected: {discInfo.BlurayDetected}");
                    Console.WriteLine($"Disc name: {discInfo.DiscName}");
                    Console.WriteLine($"Number of titles: {discInfo.NumTitles}");
                    
                    // Print all available titles
                    uint numTitles = bluray.GetTitles();
                    Console.WriteLine($"Found {numTitles} titles:");
                    
                    for (uint i = 0; i < numTitles; i++)
                    {
                        var titleInfo = bluray.GetTitleInfo(i);
                        if (titleInfo != null)
                        {
                            double durationSeconds = (double)titleInfo.Duration / 90000.0; // Convert from 90kHz ticks
                            Console.WriteLine($"  Title {i}: Duration = {durationSeconds:F2}s, Clips = {titleInfo.ClipCount}, Chapters = {titleInfo.ChapterCount}");
                        }
                    }
                    
                    // Try to get main title
                    int mainTitle = bluray.GetMainTitle();
                    Console.WriteLine($"Main title: {mainTitle}");
                    
                    // Try to select and play first title if available
                    if (numTitles > 0)
                    {
                        if (bluray.SelectTitle(0))
                        {
                            Console.WriteLine("Selected title 0");
                            
                            // Show current position info
                            Console.WriteLine($"Current title: {bluray.GetCurrentTitle()}");
                            Console.WriteLine($"Current chapter: {bluray.GetCurrentChapter()}");
                            Console.WriteLine($"Current angle: {bluray.GetCurrentAngle()}");
                            Console.WriteLine($"Title size: {bluray.GetTitleSize()} bytes");
                            Console.WriteLine($"Current position: {bluray.Tell()}");
                            Console.WriteLine($"Current time: {bluray.TellTime()} (90kHz ticks)");
                            
                            // Try reading some data
                            byte[] buffer = new byte[1024];
                            int bytesRead = bluray.Read(buffer, 0, buffer.Length);
                            Console.WriteLine($"Read {bytesRead} bytes from title");
                            
                            // Check for events
                            if (bluray.GetEvent(out var @event))
                            {
                                Console.WriteLine($"Event: {@event}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Failed to select title");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Could not get disc info");
                }
            }
            else
            {
                Console.WriteLine("Failed to open disc");
            }
        }
        else
        {
            Console.WriteLine("Usage: dotnet run <path_to_bluray_disc>");
            Console.WriteLine("Example: dotnet run /dev/sr0");
            Console.WriteLine("         dotnet run /path/to/blu-ray/folder");
        }
    }
}