using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace BOA.Processors
{
    public class CheckAuthors
    {
        private readonly List<string> _users = new List<string>();
        private readonly List<string> _names = new List<string>();
        private readonly List<string> _toRemove = new List<string>();

        public CheckAuthors(List<string> data)
        {
            foreach (var item in data)
            {
                var parted = item.Split(Convert.ToChar("="));
                var user = parted[1].Trim();
                //var pid = Int32.Parse(Regex.Replace(parted[0], @"[^\d+]", ""));
                if (user.Contains(",") || user.Contains(" ") || user.Contains("@") || user.Contains("'"))
                {
                    if (!_names.Contains(user))
                        _names.Add(user);
                }
                else
                {
                    if (!_users.Contains(user))
                        _users.Add(user);
                }
            }

            Console.Clear();
            Console.WriteLine("With Username");
            /*
             * Possibly user _names! Not having an space doesn't mean it's a username!
             */
            foreach (var user in _users)
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
                    _toRemove.Add(user);
                }
            }
            
            _names.AddRange(_toRemove);
            _users.RemoveAll(x => _toRemove.Contains(x));

            Console.WriteLine();
            Console.WriteLine("With Username: " + _users.Count);
            Console.WriteLine("Without Username: " + _names.Count);
            Console.WriteLine("Total Authors: " + data.Count);
            File.WriteAllLines("ValidAuthors.txt", _users);
            Console.Read();
        }
    }
}
