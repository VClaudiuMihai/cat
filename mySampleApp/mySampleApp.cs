using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CatImageDownloader
{
    class mySampleApp
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: CatImageDownloader.exe -o <output_filepath> [-t <text_to_overlay>] [-d <output_directory>]");
                return;
            }

            string outputFilePath = null;
            string textToOverlay = null;
            string outputDirectory = Directory.GetCurrentDirectory(); // Default to current directory if not specified

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-o" && i < args.Length - 1)
                {
                    outputFilePath = args[i + 1];
                }
                else if (args[i] == "-t" && i < args.Length - 1)
                {
                    textToOverlay = args[i + 1];
                }
                else if (args[i] == "-d" && i < args.Length - 1)
                {
                    outputDirectory = args[i + 1];
                }
            }

            if (outputFilePath == null)
            {
                Console.WriteLine("Output file path is required.");
                return;
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var catImageUrl = "https://cataas.com/cat";

                    if (!string.IsNullOrEmpty(textToOverlay))
                    {
                        catImageUrl += $"/says/{Uri.EscapeDataString(textToOverlay)}";
                    }

                    var catImageBytes = await httpClient.GetByteArrayAsync(catImageUrl);

                    // Set the output file path to the specified directory
                    outputFilePath = Path.Combine(outputDirectory, Path.GetFileName(outputFilePath));

                    File.WriteAllBytes(outputFilePath, catImageBytes);

                    Console.WriteLine($"Cat image downloaded and saved to: {outputFilePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
