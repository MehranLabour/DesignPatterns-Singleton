// See https://aka.ms/new-console-template for more information
namespace SingletonDP;

/// <summary>
///     Singleton Design Pattern
/// </summary>
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
        //Make it thread safe
        lock (Padlock)
        {
            return _lazyInstance ??= new PrintSpooler();
        }
    }

    public void AddPrintJob(IEnumerable<string> documents)
    {
        if (documents == null) throw new ArgumentNullException(nameof(documents));
        //Make it thread safe
        lock (_queue)
        {
            foreach (var document in documents) _queue.Enqueue(document);
        }
    }

    public void ProcessPrintJob()
    {
        //Make it thread safe
        lock (_queue)
        {
            foreach (var document in _queue) Console.WriteLine(document);
            Console.WriteLine("Instance Call Finished");
        }
    }
}