using System;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "run --project src/SmartStay.API " + string.Join(" ", args),
            UseShellExecute = false
        };

        using (var process = Process.Start(processInfo))
        {
            if (process != null)
            {
                process.WaitForExit();
                Environment.Exit(process.ExitCode);
            }
        }
    }
}
