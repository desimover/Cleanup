using HtmlAgilityPack;
using System.IO;
using System.Text;

namespace HTMLCleaner
{
    internal class Parser
    {
        int counter = 0;
        private readonly string rootDir;
        private readonly string inputDir;
        private readonly string outputDir;
        public Parser(string rootDirectory, string inputDirectory, string outputDirectory)
        {
            rootDir = rootDirectory;
            inputDir = inputDirectory;
            outputDir = outputDirectory;
        }


        internal void Parse()
        {
            IterateDir(rootDir);
            System.Console.WriteLine($" Total :{counter}");
            System.Console.ReadKey();
        }

        private void IterateDir(string dir)
        {
            foreach (string childDir in Directory.GetDirectories(dir))
            {
                IterateDir(childDir);
            }
            IterateFiles(dir);
        }

        private void IterateFiles(string dir)
        {
            try
            {
                string[] files = Directory.GetFiles(dir, "*.htm*");

                foreach (string file in files)
                {
                    FileInfo info = new FileInfo(file);
                    if (info.Length > 1024)
                    {
                        string outputPathName = $"{info.Directory.Parent.FullName}".Replace(inputDir, outputDir);
                        string outputFileName = Path.Combine(outputPathName, $"{info.Directory.Name}.html");
                        string cleanHTML = GetCleanHTML(file);
                        if (!string.IsNullOrEmpty(cleanHTML))
                        {
                            if (!Directory.Exists(outputPathName)) { Directory.CreateDirectory(outputPathName); }
                            File.AppendAllText(outputFileName, cleanHTML);
                            System.Console.WriteLine($"Processing File : {file} : {counter++ }");
                        }
                        else
                        {
                            System.Console.WriteLine($"Empty File {file}");
                        }
                    }
                    else
                    {
                        System.Console.WriteLine($"Skipping File {file}");
                    }
                }
            }
            catch (System.Exception ex) { System.Console.WriteLine(ex); }
        }

        private string GetCleanHTML(string fileName)
        {
            string str;
            HtmlDocument doc = new HtmlWeb().Load(fileName);
            StringBuilder builder = new StringBuilder();
           /* str = GetNodeHTML("//div[@class='responsive-tabs']", true, doc);
            builder.Append($"{str}");
            str = GetNodeHTML("//div[@dir='ltr']", true, doc);
            builder.Append($"{str}");*/
            str = GetNodeHTML("//span[contains(@style,'color')]/parent::*/parent::*", true, doc);
            builder.Append($"{str}");
            return builder.ToString();
        }

        private static string GetNodeHTML(string xPath, bool removeImage, HtmlDocument doc)
        {
            HtmlNode node = doc.DocumentNode.SelectSingleNode(xPath);
            StringBuilder builder = new StringBuilder();
            if (node != null)
            {
                if (removeImage)
                {
                    CleanNode(node);
                }
                builder.Append($"{node.OuterHtml}");
            }
            return builder.ToString();
        }

        private static void CleanNode(HtmlNode node)
        {
            HtmlNodeCollection imgNodes = node.SelectNodes("//img");
            if (imgNodes != null)
            {
                foreach (HtmlNode imgNode in imgNodes)
                {
                    imgNode.Remove();
                }
            }
        }
    }
}