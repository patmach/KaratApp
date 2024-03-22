// See https://aka.ms/new-console-template for more information
using KaratApp;
using System.Diagnostics;

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
string[] ipAddresses = args[1..];
Task writeTask =TestResultsXMLFileWriter.StartXMLWriteTask(ipAddresses, timeout);
IPTestOrchestrator.CreateAndRunIPTests(timeout, ipAddresses);
writeTask.Wait();
AvailabilityStatsRetriever.RetrieveAndPrint(ipAddresses, TestResultsXMLFileWriter.path);


