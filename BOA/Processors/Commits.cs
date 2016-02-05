using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Models;

namespace BOA.Processors
{
    public class Commits
    {
        public HashSet<User> Users { get; set; }

        public Commits()
        {
            Users = new HashSet<User>();
        }

        public HashSet<User> Process(IReadOnlyCollection<string> data)
        {
            File.WriteAllLines("CommitsRAW.txt", data);

            foreach (var item in data)
            {
                var split = item.Replace("files[] = ", "").Split(Convert.ToChar("#"));
                var projectId = int.Parse(split.First().Trim());
                var commitId = split[1].Trim();
                var userName = split[2].Trim();
                var date = long.Parse(split[3].Trim().Replace("000000", ""));
                var change = split.Last().Trim().Split(Convert.ToChar(","));
                var added = 0;
                var modified = 0;
                var deleted = 0;

                switch (change[0])
                {
                    case "A": added = int.Parse(change[1].Trim().Replace(".0", "")); break;
                    case "M": modified = int.Parse(change[1].Trim().Replace(".0", "")); break;
                    case "D": deleted = int.Parse(change[1].Trim().Replace(".0", "")); break;
                }

                var user = new User();

                if (Users.Any(u => u.UserName == userName))
                {
                    user = Users.Single(u => u.UserName == userName);
                }
                else
                {
                    user.UserName = userName;
                    user.Projects = new HashSet<int>();
                    user.Commits = new List<Commit>();
                }

                if (!user.Projects.Contains(projectId))
                    user.Projects.Add(projectId);

                var commit = new Commit();

                if (commit.Changes == null)
                    commit.Changes = new Changes();

                var committed = user.Commits.Any(c => c.CommitId == commitId);
                if (committed)
                    commit = user.Commits.Single(c => c.CommitId.Equals(commitId));
                
                commit.Changes.Added += added;
                commit.Changes.Modified += modified;
                commit.Changes.Deleted += deleted;

                // if current commitId doesn't exists in User's context fill commit
                if (!committed)
                {
                    commit.CommitId = commitId;
                    commit.ProjectId = projectId;
                    commit.Date = date;
                    user.Commits.Add(commit);
                }

                Users.Add(user);
            }

            return Users;
        }
    }
}
