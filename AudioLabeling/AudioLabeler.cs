using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace AudioLabeling
{
    class AudioLabeler
    {
        static void Main(string[] args)
        {
            var audioFiles = new List<string>
            {
                "file 1", "file 2", "file 3", "file 4", "file 5",
            };
            var sortedAudioFiles = new Dictionary<string, string>();

            sortedAudioFiles = StartAudioLabeler(audioFiles, sortedAudioFiles);
            WriteToJson(sortedAudioFiles, "Labels");
            Console.WriteLine("Done.n.. press any key for exit");
            Console.ReadKey();
        }


        private static Dictionary<string, string> StartAudioLabeler(List<string> audioFiles,
            Dictionary<string, string> sortedAudioFiles)
        {
            Console.WriteLine("Reading audio files...");

            int audioFilesCount = audioFiles.Count;
            int audioFileStartIndex = 1;

            foreach (var audioFile in audioFiles)
            {
                Console.WriteLine($"Playing audio file {audioFileStartIndex++}/{audioFilesCount},");
                SortAudioFiles(audioFile, sortedAudioFiles);
            }

            Console.WriteLine("Audio files are end...");
            return sortedAudioFiles;
        }


        private static void SortAudioFiles(string audioFile, Dictionary<string, string> sortedAudioFiles)
        {
            Console.WriteLine($"File name {audioFile},input 'n'(for noisy) or 'c' (for clean)");

            string inputFromKeyboard = Console.ReadLine();


            switch (inputFromKeyboard.ToLower())
            {
                case "n":
                    string labelN = "noisy";
                    Console.WriteLine(labelN);
                    sortedAudioFiles.Add(audioFile, labelN);
                    break;
                case "c":
                    string labelC = "clean";
                    Console.WriteLine(labelC);
                    sortedAudioFiles.Add(audioFile, labelC);
                    break;
                default:
                    Console.WriteLine("Incorrect input,Possible variants 'n' or 'c',try again");
                    SortAudioFiles(audioFile, sortedAudioFiles);
                    break;
            }
        }


        private static void WriteToJson(Dictionary<string, string> sortedAudioFiles, string jsonFileName)
        {
            Console.WriteLine("Exporting to json file");

            var data = new List<LabelingFormat>();

            foreach (var file in sortedAudioFiles)
            {
                data.Add(new LabelingFormat()
                {
                    fileName = file.Key,
                    label = file.Value,
                });
            }


            string json = JsonSerializer.Serialize(data);
            string fileName = $"{jsonFileName}.json";
            string filePath = GetProjectRootPath() + fileName;

            File.WriteAllText(filePath, json);
        }


        private class LabelingFormat
        {
            public string fileName { get; set; }
            public string label { get; set; }
        }


        private static string GetProjectRootPath()
        {
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../"));
        }
    }
}