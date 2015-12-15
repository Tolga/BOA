using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// ReSharper disable InconsistentNaming
namespace BOA.Processors
{
    public class CommittedJavaFiles : IProcessor
    {
        private List<string> Data { get; set; }
        private Dictionary<string, int> Added { get; set; }

        private Dictionary<string, DateTime> datesAdded { get; set; }

        private Dictionary<string, int> Modified { get; set; }
        private Dictionary<string, DateTime> datesModified { get; set; }
        private Dictionary<string, int> Deleted { get; set; }
        private Dictionary<string, DateTime> datesDeleted { get; set; }

        public CommittedJavaFiles(List<string> data)
        {
            Data = data;
            //File.WriteAllLines("CommittedJavaFilesList.txt", data);
        }

        public void Process()
        {
            Added = new Dictionary<string, int>();
            datesAdded = new Dictionary<string, DateTime>();
            Modified = new Dictionary<string, int>();
            datesModified = new Dictionary<string, DateTime>();
            Deleted = new Dictionary<string, int>();
            datesDeleted = new Dictionary<string, DateTime>();

            foreach (var item in Data)
            {
                var parted = item.Split(Convert.ToChar("="));

                var project = parted[0].Split(Convert.ToChar("[")).Last().Replace("]", "").Trim();
                var changeType = parted[1].Trim();
                var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(parted[2].Trim().Replace("000000", "")));

                switch (changeType)
                {
                    case "ADDED":
                        if (Added.ContainsKey(project))
                            Added[project]++;
                        else
                            Added[project] = 1;
                        datesAdded[project] = dateTime.LocalDateTime;
                        break;
                    case "MODIFIED":
                        if (Modified.ContainsKey(project))
                            Modified[project]++;
                        else
                            Modified[project] = 1;
                        datesModified[project] = dateTime.LocalDateTime;
                        break;
                    case "DELETED":
                        if (Deleted.ContainsKey(project))
                            Deleted[project]++;
                        else
                            Deleted[project] = 1;
                        datesDeleted[project] = dateTime.LocalDateTime;
                        break;
                }
                //var pid = Int32.Parse(Regex.Replace(parted[0], @"[^\d+]", ""));
            }
            
            Console.Clear();
            foreach (var added in Added)
            {
                Console.WriteLine();
                Console.WriteLine("Project ID: {0}", added.Key);
                Console.WriteLine("Added: {0}", added.Value);
                Console.WriteLine("Modified: {0}", Modified.SingleOrDefault(s => s.Key == added.Key).Value);
                Console.WriteLine("Deleted: {0}", Deleted.SingleOrDefault(s => s.Key == added.Key).Value);
            }
            Console.ReadLine();

            /*
            File.WriteAllLines("ValidGitHubUsers.txt", users);
            File.AppendAllText("ValidGitHubUsers.txt", "\r\nTotal: " + users.Count);
            */
        }
    }
}
