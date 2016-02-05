namespace BOA
{
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using Processors;
    using Serializers;

    internal class Program
    {
        private static List<string> ReadUsersDataFile => File.ReadAllLines("CommitsRAW.txt").ToList();
        public static List<string> FileChangesPerCommit => new Api("tolgamengu", "Boa352", Queries.Commits()).Execute();

        private static void Main()
        {
            var users = new Commits().Process(ReadUsersDataFile);

            var xml = new Xml();
            var json = new Json();
            var csv = new Csv();

            foreach (var user in users)
            {
                xml.AddUser(user.UserName);
                json.AddUser(user.UserName);
                foreach (var project in user.Projects)
                {
                    var commits = user.Commits.FindAll(c => c.ProjectId.Equals(project));
                    csv.Add(user.UserName, commits);
                    xml.Add(project, commits);
                    json.Add(project, commits);
                }
                xml.CloseUser();
                json.CloseUser();
            }

            csv.Save();
            xml.CloseFile();
            xml.Save();
            json.CloseFile();
            json.Save();
        }
    }
}
