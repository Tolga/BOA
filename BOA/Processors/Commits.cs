using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Models;

namespace BOA.Processors
{
    public class Commits
    {
        private HashSet<User> ValidCommitsByUsers { get; }
        private HashSet<User> InvalidCommitsByUsers { get; }

        public Commits()
        {
            ValidCommitsByUsers = new HashSet<User>();
            InvalidCommitsByUsers = new HashSet<User>();
        }

        public HashSet<User> ValidCommits()
        {
            return ValidCommitsByUsers;
        }

        public HashSet<User> InvalidCommits()
        {
            return InvalidCommitsByUsers;
        }

        public void Process(IReadOnlyCollection<string> data)
        {
            File.WriteAllLines("CommitsMediumRAW.txt", data);
            var validAuthors = File.ReadAllLines("ValidAuthors.txt").ToList();

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
                    case "A":
                        added = int.Parse(change[1].Trim().Replace(".0", ""));
                        break;
                    case "M":
                        modified = int.Parse(change[1].Trim().Replace(".0", ""));
                        break;
                    case "D":
                        deleted = int.Parse(change[1].Trim().Replace(".0", ""));
                        break;
                }

                var user = new User();

                if (ValidCommitsByUsers.Any(u => u.UserName == userName))
                {
                    user = ValidCommitsByUsers.Single(u => u.UserName == userName);
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

                if (!committed)
                {
                    commit.CommitId = commitId;
                    commit.ProjectId = projectId;
                    commit.Date = date;
                    user.Commits.Add(commit);
                }

                if (validAuthors.Contains(userName))
                {
                    ValidCommitsByUsers.Add(user);
                }
                else
                {
                    InvalidCommitsByUsers.Add(user);
                }
            }
        }
    }
}
