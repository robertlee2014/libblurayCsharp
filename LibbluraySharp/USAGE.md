# LibbluraySharp Usage Guide

LibbluraySharp is a C# binding for the libbluray library, allowing .NET applications to interact with Blu-ray discs.

## Installation

To use LibbluraySharp in your project, you'll need to:

1. Build the native libbluray library and ensure it's available on your system
2. Add the LibbluraySharp NuGet package to your project (when published) or reference the source project

## Basic Usage

### Opening a Blu-ray Disc

```csharp
using LibbluraySharp;

// Method 1: Static OpenDisc method
using var bluray = Bluray.OpenDisc("/path/to/bluray/disc");
if (bluray != null && bluray.IsOpen)
{
    // Work with the disc
}

// Method 2: Create instance and open
using var bluray = new Bluray();
if (bluray.Open("/path/to/bluray/disc"))
{
    // Work with the disc
}
```

### Getting Disc Information

```csharp
var discInfo = bluray.GetDiscInfo();
if (discInfo != null)
{
    Console.WriteLine($"Blu-ray detected: {discInfo.BlurayDetected}");
    Console.WriteLine($"Disc name: {discInfo.DiscName}");
    Console.WriteLine($"Number of titles: {discInfo.NumTitles}");
    Console.WriteLine($"AACS protected: {discInfo.AacsDetected}");
}
```

### Working with Titles

```csharp
// Get number of titles
uint numTitles = bluray.GetTitles();

// Get information about a specific title
var titleInfo = bluray.GetTitleInfo(0); // First title
if (titleInfo != null)
{
    Console.WriteLine($"Duration: {titleInfo.Duration} (90kHz ticks)");
    Console.WriteLine($"Clip count: {titleInfo.ClipCount}");
    Console.WriteLine($"Chapter count: {titleInfo.ChapterCount}");
}

// Select a title for playback
if (bluray.SelectTitle(0))
{
    Console.WriteLine("Title selected successfully");
}
```

### Reading Data

```csharp
byte[] buffer = new byte[1024];
int bytesRead = bluray.Read(buffer, 0, buffer.Length);
Console.WriteLine($"Read {bytesRead} bytes");
```

### Playback Control

```csharp
// Seek to a specific position
long newPosition = bluray.Seek(1000000);

// Seek to a specific time (in 90kHz ticks)
long newPosByTime = bluray.SeekTime(900000); // 10 seconds (10 * 90000)

// Seek to a specific chapter
long chapterPos = bluray.SeekChapter(2); // Third chapter (0-indexed)

// Get current position
ulong currentPosition = bluray.Tell();
ulong currentTime = bluray.TellTime();
```

### Handling Events

```csharp
if (bluray.GetEvent(out var @event))
{
    Console.WriteLine($"Event type: {@event.EventType}");
    Console.WriteLine($"Event param: {@event.Param}");
    
    switch (@event.EventType)
    {
        case EventTypes.Chapter:
            Console.WriteLine($"New chapter: {@event.Param}");
            break;
        case EventTypes.Title:
            Console.WriteLine($"New title: {@event.Param}");
            break;
        // Handle other events...
    }
}
```

### Player Settings

```csharp
// Set player settings
bluray.SetPlayerSetting(PlayerSettings.RegionCode, 1); // Region 1
bluray.SetPlayerSettingString(PlayerSettings.MenuLang, "en"); // English menu
bluray.SetPlayerSettingString(PlayerSettings.AudioLang, "en"); // English audio
```

### Navigation and Menus

```csharp
// Play with on-disc menus
if (bluray.Play())
{
    Console.WriteLine("Playing with menus started");
}

// Play a specific title
bluray.PlayTitle(1); // Play title 1 from disc index

// Call top menu
bluray.MenuCall();

// Handle user input for menus
int result = bluray.UserInput(pts: -1, key: 0x01); // Simulate key press
```

## Important Notes

- The native libbluray library must be installed and available on the system
- Some operations may require key databases for encrypted discs (AACS)
- Always dispose of the Bluray object when done to properly clean up resources
- The library supports both programmatic title selection and interactive menu navigation
- Events should be regularly checked during playback to handle state changes