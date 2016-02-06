using BOA.Models;

namespace BOA
{
    using System.Collections.Generic;
    using Processors;
    using Serializers;

    internal class Program
    {
        //private static List<string> ReadUsersDataFile => File.ReadAllLines("CommitsRAW.txt").ToList();
        //private static List<string> GetCommitersList => new Api("tolgamengu", "Boa352", Queries.GetAuthorsList()).Execute(1);
        private static List<string> FileChangesPerCommit => new Api("tolgamengu", "Boa352", Queries.Commits()).Execute(1);

        private static void Main()
        {
            var commits = new Commits();
            commits.Process(FileChangesPerCommit);
            SaveCommits(commits.ValidCommits(), "ValidCommits");
            SaveCommits(commits.InvalidCommits(), "InvalidCommits");
        }

        private static void SaveCommits(HashSet<User> commits, string fileName = "Commits")
        {
            var xml = new Xml();
            var json = new Json();
            var csv = new Csv();

            foreach (var user in commits)
            {
                xml.AddUser(user.UserName);
                json.AddUser(user.UserName);
                foreach (var project in user.Projects)
                {
                    var commitsByUser = user.Commits.FindAll(c => c.ProjectId == project);
                    csv.Add(user.UserName, commitsByUser);
                    xml.Add(project, commitsByUser);
                    json.Add(project, commitsByUser);
                }
                xml.CloseUser();
                json.CloseUser();
            }

            csv.Save(fileName);
            xml.CloseFile();
            xml.Save(fileName);
            json.CloseFile();
            json.Save(fileName);
        }
    }
}
