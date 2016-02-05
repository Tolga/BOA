namespace BOA
{
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using Models;
    using Processors;
    using Serializers;

    internal class Program
    {
        private static List<string> ReadUsersDataFile => File.ReadAllLines("CommitsRAW.txt").ToList();
        public static List<string> FileChangesPerCommit => new Api("tolgamengu", "Boa352", Queries.Commits()).Execute();

        private static void Main()
        {
            var users = new Commits().Process(ReadUsersDataFile);

            //var json = new Json();
            var csv = new Csv();

            foreach (var user in users)
            {
                //json.AddUser(user.Value.UserName);
                foreach (var project in user.Projects)
                {
                    foreach (var commit in user.Commits)
                    {
                        var newChanges = new Changes
                        {
                            Added = commit.Changes.Added,
                            Modified = commit.Changes.Modified,
                            Deleted = commit.Changes.Deleted
                        };

                        var newCommit = new Commit
                        {
                            CommitId = commit.CommitId,
                            ProjectId = project,
                            Date = commit.Date,
                            Changes = newChanges
                        };

                        csv.Add(user.UserName, newCommit);
                    }
                    //json.AddProject(project, user.Value.Commits.FindAll(c => c.ProjectId.Equals(project)));
                }
                //json.CloseUser();
            }
            csv.Save();
            //json.CloseFile();
            //json.Save();
        }

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
