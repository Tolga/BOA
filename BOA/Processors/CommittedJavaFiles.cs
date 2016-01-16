using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable InconsistentNaming
namespace BOA.Processors
{
    public class CommittedJavaFiles
    {
        public Dictionary<string, User> Users { get; set; }

        public CommittedJavaFiles(IEnumerable<string> data)
        {
            //File.WriteAllLines("CommittedJavaFiles.txt", data);

            foreach (var item in data)
            {
                var split = item.Split(Convert.ToChar("="));
                var commitId = int.Parse(split[1].Trim());
                var projectId = int.Parse(split[0].Split(Convert.ToChar("[")).Last().Replace("]", "").Trim());
                var change = split[4].Trim().ToLower();
                var userName = split[2].Trim();
                var date = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(split[3].Trim().Replace("000000", "")));

                var user = new User();

                if (Users.ContainsKey(userName))
                {
                    user = Users.Single(u => u.Key.Equals(userName)).Value;
                }
                else
                {
                    user.UserName = userName;
                }

                if (!user.Projects.Contains(projectId))
                {
                    user.Projects.Add(projectId);
                }

                var commit = new Commit();
                var is_committed = user.Commits.Keys.Contains(commitId);

                if (is_committed)
                {
                    commit = user.Commits.Single(c => c.Key.Equals(commitId)).Value;
                }

                switch (change)
                {
                    case "added": commit.Changes.Added++; break;
                    case "modified": commit.Changes.Modified++; break;
                    case "deleted": commit.Changes.Deleted++; break;
                }

                if (is_committed) continue;
                commit.CommitId = commitId;
                commit.ProjectId = projectId;
                commit.Date = date;

                user.Commits.Add(commitId, commit);
            }
            
            Console.Clear();
            Console.ReadLine();
        }
    }
}
