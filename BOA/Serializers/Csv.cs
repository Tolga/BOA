namespace BOA.Serializers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public class Csv
    {
        private List<string> Data { get; set; }

        public Csv()
        {
            Data = new List<string>();
        }

        public void Add(string userName, Commit commit)
        {
            if (!Data.Any())
                Data.Add("ProjectId, CommitId, User, TimeStamp, ChangeType, Quantity");

            if (commit.Changes.Added > 0)
                Data.Add(commit.ProjectId + ", " + commit.CommitId + ", " + userName + ", " + commit.Date + ", " + "A" + ", " + commit.Changes.Added);

            if (commit.Changes.Modified > 0)
                Data.Add(commit.ProjectId + ", " + commit.CommitId + ", " + userName + ", " + commit.Date + ", " + "M" + ", " + commit.Changes.Modified);

            if (commit.Changes.Deleted > 0)
                Data.Add(commit.ProjectId + ", " + commit.CommitId + ", " + userName + ", " + commit.Date + ", " + "D" + ", " + commit.Changes.Deleted);
        }

        public void Save(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
