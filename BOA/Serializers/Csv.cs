using System.IO;

namespace BOA.Serializers
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public class Csv
    {
        private List<string> Data { get; }

        public Csv()
        {
            Data = new List<string>();
        }

        public void Add(string userName, Commit commit)
        {
            if (!Data.Any())
            {
                Data.Add("ProjectId, CommitId, Author, TimeStamp, Added, Modified, Deleted, Total");
            }
            var total = commit.Changes.Added + commit.Changes.Modified + commit.Changes.Deleted;
            Data.Add(commit.ProjectId + ", " + commit.CommitId + ", " + userName + ", " + commit.Date + ", " + commit.Changes.Added + ", " + commit.Changes.Modified + ", " + commit.Changes.Deleted + ", " + total);
        }

        public void Save(string fileName = "Commits")
        {
            File.WriteAllLines(fileName + ".csv", Data);
        }
    }
}
