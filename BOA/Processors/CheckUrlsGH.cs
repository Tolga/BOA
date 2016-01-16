using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

// ReSharper disable InconsistentNaming
namespace BOA.Processors
{
    public class CheckUrlsGH
    {
        readonly List<string> users = new List<string>();
        readonly List<string> names = new List<string>();
        readonly List<string> toRemove = new List<string>();

        public CheckUrlsGH(List<string> data)
        {
            foreach (var item in data)
            {
                var parted = item.Split(Convert.ToChar("="));
                var user = parted[1].Trim();
                //var pid = Int32.Parse(Regex.Replace(parted[0], @"[^\d+]", ""));
                if (user.Contains(".") || user.Contains(",") || user.Contains(" ") || user.Contains("@")) names.Add(user); else users.Add(user);
            }

            Console.Clear();
            Console.WriteLine("With Username");
            /*
             * Possibly user names! Not having an space doesn't mean it's a username!
             */
            foreach (var user in users)
            {
                var request = (HttpWebRequest)WebRequest.Create("https://github.com/" + user);
                request.Method = "HEAD";
                request.Timeout = 10000;

                try
                {
                    var response = (HttpWebResponse)request.GetResponse();
                        response.Close();
                    if (response.StatusCode == HttpStatusCode.OK)
                        Console.WriteLine("Username: "+user);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    toRemove.Add(user);
                }
            }
            
            names.AddRange(toRemove);
            users.RemoveAll(x => toRemove.Contains(x));

            Console.WriteLine();
            Console.WriteLine("With Username: "+users.Count);
            Console.WriteLine("Without Username: "+names.Count);
            Console.WriteLine("Total Committers: "+data.Count);

            File.WriteAllLines("ValidGitHubUsers.txt", users);
            File.AppendAllText("ValidGitHubUsers.txt", "\r\nTotal: " + users.Count);
        }
    }
}
