using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

// ReSharper disable InconsistentNaming
namespace BOA.Processors
{
    public class CheckUrlsSF
    {
        public CheckUrlsSF(List<string> data)
        {
            var urlsList = data.Select(x => Regex.Match(x, "http.+", RegexOptions.IgnoreCase).ToString()).ToList();

            //WebClient client = new WebClient();

            var workingUrls = new List<string>();
            var notWorkingUrls = new List<string>();
            var exceptionUrls = new List<string>();

            foreach (var url in urlsList)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "HEAD";
                    request.Timeout = 10000;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        response.Close();
                        workingUrls.Add(url);
                    }
                    Console.WriteLine("OK\t" + url);
                   // File.AppendAllText("ExistingUsers.txt", url + "\r\n");
                }
                catch(Exception ex)
                {
                    if (ex.Message == "The remote server returned an error: (404) Not Found.")
                    {
                        notWorkingUrls.Add(url);
                        Console.WriteLine("ERROR\t" + url);
                       // File.AppendAllText("NonExistingUsers.txt", url + "\r\n");
                    }
                    else
                    {
                        exceptionUrls.Add(url);
                        //File.AppendAllText("ExceptionUsers.txt", url + "\r\n");
                        Console.WriteLine("EXCEPTION\t" + ex.Message + url);
                    }
                }
                
            }

            File.WriteAllLines("ExistingUsers2.txt", workingUrls);
            File.WriteAllLines("NonExistingUsers2.txt", notWorkingUrls);
            File.WriteAllLines("ExceptiongUsers2.txt", exceptionUrls);
        }
    }
}
