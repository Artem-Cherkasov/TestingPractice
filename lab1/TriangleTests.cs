using System;
using System.Diagnostics;
using System.IO;

namespace TriangleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            string appName = "C:\\Users\\Админ\\ТиОПО\\Lab1\\Triangle\\Triangle\\bin\\Release\\net5.0\\Triangle.exe";
            string outFile = "result.txt";
            string inpFile = "Testcase.txt";

            StreamReader r = new(inpFile);
            StreamWriter w = new(outFile);

            string testCaseArguments;
            while ((testCaseArguments = r.ReadLine()) != null)
            {
                try
                {
                    testCaseArguments = testCaseArguments.Replace('\t', ' ');
                    string[] prevArgs = testCaseArguments.Split(':');
                    string[] tArg = prevArgs[0].Split(' ');

                    if (tArg.Length != 3)
                    {
                        if (prevArgs[1] == "Undefined Error 1") w.WriteLine("success");
                            else throw new ArgumentException();
                    }
                    else
                    {
                        var procInfo = new ProcessStartInfo
                        {
                            FileName = appName,
                            Arguments = $"{tArg[0]} {tArg[1]} {tArg[2]}",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        };

                        Process proc = Process.Start(procInfo);
                        string procResult = proc.StandardOutput.ReadLine();
                        proc.WaitForExit();

                        if (procResult == prevArgs[1]) w.WriteLine("success");
                        else w.WriteLine("error");
                    }
                }
                catch (Exception)
                {
                    w.WriteLine("error");
                }
            }
            w.Close();
        }
    }
}
