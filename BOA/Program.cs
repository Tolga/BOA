namespace BOA
{
    using System;
    using System.Collections.Generic;
    using Processors;
    using Serializers;

    internal class Program
    {
        private static List<string> FileChangesPerCommit => new Api("tolgamengu", "Boa352", Queries.Commits(100000)).Execute();

        private static void Main()
        {
            var data = new Commits(FileChangesPerCommit);

            var users = data.Users;

            Json json = new Json();

            foreach (var user in users)
            {
                var userName = user.Value.UserName;

                Console.WriteLine("User: " + userName + " Total Projects: " + user.Value.Projects.Count + " Total Commits: " + user.Value.Commits.Count);
                Console.WriteLine();

                json.AddUser(userName);

                foreach (var project in user.Value.Projects)
                {
                    json.AddProject(project, user.Value.Commits.FindAll(c => c.ProjectId.Equals(project)));
                }

                json.CloseUser();
            }

            json.CloseFile();
            json.Save();
        }

        //private static List<string> ReadUsersDataFile => File.ReadAllLines("Commits.txt").ToList();

        /*
        foreach (var commit in commits)
        {
            var commitId = commit.CommitId;
            var date = commit.Date;
            var projectId = commit.ProjectId;
            var added = commit.Changes.Added;
            var modified = commit.Changes.Modified;
            var deleted = commit.Changes.Deleted;
        }
        */
    }
}
