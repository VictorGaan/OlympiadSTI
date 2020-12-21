using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Blockchain
{
    public class Helper
    {
        public static readonly string DefaultFilePath = $"{Environment.CurrentDirectory}\\Data\\blockchain.json";
        public static readonly string DefaultFolderPath = $"{Environment.CurrentDirectory}\\Data";
        public static void InitFileSystem()
        {
            if (!CheckFolder())
            {
                CreateFolder();
            }
            if (!CheckFile())
            {
                CreateFile();
            }
        }
        public static void CreateFolder()
        {
            Directory.CreateDirectory(DefaultFolderPath);
        }
        public static void CreateFile()
        {
            File.Create(DefaultFilePath);
        }
        public static bool CheckFolder()
        {
            return File.Exists(DefaultFolderPath) == true ? true : false;
        }
        public static bool CheckFile()
        {
            return File.Exists(DefaultFilePath) == true ? true : false;
        }
        public static void LoadBlockchain(Blockchain blockchain)
        {
           
            if (CheckFile())
            {
                blockchain.Chain = JsonConvert.DeserializeObject<List<Block>>(File.ReadAllText(DefaultFilePath));
            }
        }
        public static void SaveBlockchain(Blockchain blockchain)
        {
            File.WriteAllText(DefaultFilePath, JsonConvert.SerializeObject(blockchain.Chain, Formatting.Indented));
        }
    }
}
