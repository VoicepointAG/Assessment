using System;
using System.IO;
using System.Threading.Tasks;

namespace Voicepoint.CliTools.WebApiClientGenerator
{
    class Program
    {
        private static void ParseCommandLine(string[] args, out string dllPath, out string apiClientPath)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("Needs 2 parameters: 1. dllPath, 2. apiClientPath");
            }

            dllPath = new DirectoryInfo(args[0]).FullName;
            apiClientPath = new DirectoryInfo(args[1]).FullName;
        }

        private static void Cleanup(string apiClientPath)
        {
            if (File.Exists(apiClientPath))
            {
                File.Delete(apiClientPath);
            }
        }

        private static bool Validate(string apiClientPath) => File.Exists(apiClientPath);

        static async Task<int> Main(string[] args)
        {
            ParseCommandLine(args, out var dllPath, out var apiClientPath);
            Console.WriteLine($"Source for web api client: {dllPath}");

            string newFileContent = await WebApiClientGenerator.GenerateWebApiClientFileContentAsync(dllPath);

            string oldFileContent = File.Exists(apiClientPath) ? File.ReadAllText(apiClientPath) : null;

            if (newFileContent != oldFileContent)
            {
                Cleanup(apiClientPath);
                Directory.CreateDirectory(Path.GetDirectoryName(apiClientPath));

                File.WriteAllText(apiClientPath, newFileContent);

                if (!Validate(apiClientPath))
                {
                    Console.WriteLine("Generating the web api client failed");
                    return -1;
                }
                else
                {
                    Console.WriteLine($"Generated web api client: {apiClientPath}");
                }
            }
            else
            {
                Console.WriteLine($"Files still the same, did not generate new version: {apiClientPath}");
            }

            return 0;
        }
    }
}
