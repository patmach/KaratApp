// See https://aka.ms/new-console-template for more information
using KaratApp;
using System.Diagnostics;

Console.WriteLine("Starting...");
int timeout = 0;
string usageMessage = "This application must be run with these arguments:" +
    " [number of seconds the test will run]" +
    " [IP address 1] [IP address 2] ...\"";
if (args.Length < 1)
{
    Console.WriteLine("You need to specify atleast 2 parameters!");
    Console.WriteLine(usageMessage);
    return;
}
if (!int.TryParse(args[0], out timeout))
{
    
    Console.WriteLine($"\"{args[0]}\" specifying timeout in seconds can not be interpreted as an integer number!");
    Console.WriteLine(usageMessage);
    return;
}
Task writeTask =TestResultsXMLFileWriter.StartXMLWriteTask(args[1..], timeout);
IPTestOrchestrator.CreateAndRunIPTests(timeout, args[1..]);
writeTask.Wait();


