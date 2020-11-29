using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace ImageStore.Backend.Parser
{
    public static class ParserService
    {
        public static byte[] ParseCaff(string path)
        {
            string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            string projectName = Assembly.GetExecutingAssembly().GetName().Name;
            string fileName = $@"{solutionDirectory}\{projectName}\caff-parser\out\src\Release\caff-parser.exe";

            var startInfo = new ProcessStartInfo(fileName)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = path
            };

            var process = new Process { StartInfo = startInfo };
            var skip = 0;
            var hexData = "";
            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data is null) return;
                if (args.Data.Length == 1)
                {
                    skip = int.Parse(args.Data);
                    return;
                }

                if (skip != 0)
                {
                    skip--;
                    return;
                }

                hexData += args.Data;
            };

            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();

            var bytesCount = (hexData.Length) / 2;
            var bytes = new byte[bytesCount];
            for (var x = 0; x < bytesCount; x++)
            {
                bytes[x] = Convert.ToByte(hexData.Substring(x * 2, 2), 16);
            }

            return bytes;
        }
    }
}
