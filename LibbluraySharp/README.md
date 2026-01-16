# LibbluraySharp - C# bindings for libbluray

LibbluraySharp provides comprehensive C# bindings for the libbluray library, enabling .NET applications to interact with Blu-ray discs. This library follows the design patterns of popular VLC libraries like LibVLCSharp, providing a clean, managed interface to the powerful libbluray functionality.

## Features

- Full P/Invoke wrapper around libbluray native functions
- Managed object-oriented API
- Disc information retrieval
- Title management and selection
- Playback control (seeking, chapter navigation, etc.)
- Event handling system
- Support for both programmatic playback and menu navigation
- Player settings configuration
- Data streaming capabilities

## Project Structure

```
LibbluraySharp/
├── LibbluraySharp/           # Main library
│   ├── Native/               # P/Invoke declarations
│   │   └── NativeMethods.cs  # All native function declarations
│   ├── Bluray.cs             # Main entry point class
│   ├── DiscInfo.cs           # Disc information wrapper
│   ├── TitleInfo.cs          # Title information wrapper
│   └── Event.cs              # Event wrapper
├── LibbluraySharp.Example/   # Example application
│   └── Program.cs            # Example usage
├── USAGE.md                  # Usage guide
└── README.md                 # This file
```

## Requirements

- .NET 6.0 or higher
- Native libbluray library installed on the system
- For encrypted discs: AACS key database (KEYDB.cfg)

## Building

```bash
# Build the main library
dotnet build LibbluraySharp/

# Run the example
dotnet run --project LibbluraySharp.Example/ -- /path/to/bluray/disc
```

## Usage Example

```csharp
using LibbluraySharp;

// Open a Blu-ray disc
using var bluray = Bluray.OpenDisc("/dev/sr0");
if (bluray != null && bluray.IsOpen)
{
    // Get disc information
    var discInfo = bluray.GetDiscInfo();
    Console.WriteLine($"Blu-ray detected: {discInfo.BlurayDetected}");
    Console.WriteLine($"Number of titles: {discInfo.NumTitles}");
    
    // Get available titles
    uint numTitles = bluray.GetTitles();
    for (uint i = 0; i < numTitles; i++)
    {
        var titleInfo = bluray.GetTitleInfo(i);
        if (titleInfo != null)
        {
            double durationSeconds = (double)titleInfo.Duration / 90000.0;
            Console.WriteLine($"Title {i}: {durationSeconds:F2}s");
        }
    }
    
    // Select and play a title
    if (bluray.SelectTitle(0))
    {
        Console.WriteLine("Playing title...");
        
        // Read data from the title
        byte[] buffer = new byte[1024];
        int bytesRead = bluray.Read(buffer, 0, buffer.Length);
        Console.WriteLine($"Read {bytesRead} bytes");
    }
}
```

For more detailed usage examples, see the [USAGE.md](USAGE.md) file.

## API Overview

### Core Classes

- `Bluray`: Main class for interacting with libbluray
- `DiscInfo`: Contains information about the opened disc
- `TitleInfo`: Contains information about a specific title
- `Event`: Represents events from libbluray

### Key Operations

- **Disc Operations**: Open, close, get information
- **Title Management**: List, select, get information
- **Playback Control**: Seek, navigate chapters, manage angles
- **Data Access**: Read data from selected title
- **Event Handling**: Process events from the library
- **User Interaction**: Handle menu navigation and user input

## License

This project is licensed under the LGPL-2.1 license, consistent with the underlying libbluray library.