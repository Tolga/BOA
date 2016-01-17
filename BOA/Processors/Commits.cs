using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Models;

namespace BOA.Processors
{
    public class Commits
    {
        public Dictionary<string, User> Users { get; set; }

        public Commits(IReadOnlyCollection<string> data)
        {
            File.WriteAllLines("Commits.RAWDATA.txt", data);

            foreach (var item in data)
            {
                var commitInfo = item.Split(Convert.ToChar("|"));
                var split = commitInfo[0].Split(Convert.ToChar("#"));
                var changes = commitInfo[1].Split(Convert.ToChar("#"));

                var commitId = int.Parse(split[1].Trim());
                var projectId = int.Parse(split[0].Trim());
                var userName = split[2].Trim();
                var date = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(split[3].Trim().Replace("000000", "")));

                var added = int.Parse(changes[0].Trim());
                var modified = int.Parse(changes[1].Trim());
                var deleted = int.Parse(changes[2].Trim());

                var user = new User();

                if (Users.ContainsKey(userName))
                    user = Users.Single(u => u.Key.Equals(userName)).Value;
                else
                {
                    user.UserName = userName;
                    user.Projects = new HashSet<int>();
                    user.Commits = new List<Commit>();
                }

                if (!user.Projects.Contains(projectId))
                    user.Projects.Add(projectId);

                var commit = new Commit();

                var committed = user.Commits.Exists(c => c.CommitId.Equals(commitId));
                if (committed)
                    commit = user.Commits.Single(c => c.CommitId.Equals(commitId));
                
                commit.Changes.Added += added;
                commit.Changes.Modified += modified;
                commit.Changes.Deleted += deleted;

                // if current commitId doesn't exists in User's context fill commit
                if (!committed) continue;
                    commit.CommitId = commitId;
                    commit.ProjectId = projectId;
                    commit.Date = date;
                    user.Commits.Add(commit);
            }
            
            Console.Clear();
            Console.ReadLine();
        }
    }
}
