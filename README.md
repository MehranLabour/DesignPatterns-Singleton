# Singleton Design Pattern Example

This repository provides a practical demonstration of the Singleton design pattern using C#. The Singleton pattern ensures that a class has only one instance and facilitates global access to that instance.

## Table of Contents

- [Introduction](#introduction)
- [Usage](#usage)
- [Singleton Implementation](#singleton-implementation)
- [Thread Safety](#thread-safety)
- [Advantages](#advantages)
- [Disadvantages](#disadvantages)
- [Example](#example)
- [Conclusion](#conclusion)

### Introduction

The Singleton design pattern is used to limit the instantiation of a class to a single instance. It also offers a means to access that instance from various parts of the application. In this example, we'll delve into a simple implementation of the Singleton pattern by introducing a `PrintSpooler` class.

### Usage

The `PrintSpooler` class serves the purpose of managing print jobs in a manner that ensures thread safety. It guarantees the creation of only one instance of the `PrintSpooler` class, no matter how many times the `GetPrintSpooler` method is invoked.

### Singleton Implementation

The `PrintSpooler` class is structured as follows:

```csharp
public sealed class PrintSpooler
{
    private static PrintSpooler _lazyInstance = null!;
    private static readonly object Padlock = new();
    private readonly Queue<string> _queue = new();

    private PrintSpooler()
    {
    }

    public static PrintSpooler GetPrintSpooler()
    {
        // Ensure thread safety
        lock (Padlock)
        {
            return _lazyInstance ??= new PrintSpooler();
        }
    }

    // Other methods...
}
```

### Thread Safety
The Singleton pattern guarantees thread safety through the utilization of a lock mechanism. The GetPrintSpooler method employs a lock to ensure the creation of only one instance of the PrintSpooler class, even in scenarios involving multiple threads. Furthermore, the AddPrintJob and ProcessPrintJob methods leverage locks to synchronize access to the print job queue.

### Advantages

- **Thread Safety:** The Singleton pattern ensures thread safety, making it suitable for multi-threaded environments.
- **Improved Performance:** Reusing the same instance eliminates the overhead of creating multiple instances.
- **Simplified Configuration:** Singleton provides a single point for managing configuration settings.
- **Reduced Memory Usage:** By maintaining only one instance, memory consumption is minimized.

### Disadvantages

However, the Singleton pattern also comes with certain disadvantages:

- **Tight Coupling:** Overuse of Singleton can lead to tight coupling between classes, making the codebase less flexible.
- **Testing Challenges:** Global state makes unit testing more complex due to potential side effects.
- **Violation of SRP:** Singleton classes can easily violate the Single Responsibility Principle, taking on multiple roles.
- **Concurrency Issues:** Careful synchronization is required to handle concurrent access to the Singleton instance.


### Example
The Program class provides an illustration of how to utilize the PrintSpooler Singleton:

```csharp

namespace singleton;

public class Program
{
    public static async Task Main(string[] args)
    {
        string[] firstDocumentStrings = { "PC1_doc1", "PC1_doc2", "PC1_doc3" };
        string[] secondDocumentStrings = { "PC2_doc4", "PC2_doc5", "PC2_doc6" };
        
        var printSpooler1 = PrintSpooler.GetPrintSpooler();
        var printSpooler2 = PrintSpooler.GetPrintSpooler();

        var task1 = Task.Run(() => printSpooler1.AddPrintJob(firstDocumentStrings));
        var task2 = Task.Run(() => printSpooler2.AddPrintJob(secondDocumentStrings));

        await Task.WhenAll(task1, task2);

        printSpooler1.ProcessPrintJob();
        printSpooler2.ProcessPrintJob();
    }
}

```

### Conclusion
The Singleton design pattern is a valuable tool for ensuring the existence of only one instance of a class and for providing global access to that instance. In this example, the PrintSpooler class exemplifies a thread-safe implementation of the Singleton pattern, effectively managing print jobs. By incorporating locks and deferred instantiation, the pattern guarantees the creation of a sole instance when required and enforces synchronized access in a multi-threaded environment.