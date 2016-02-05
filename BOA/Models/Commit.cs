using System;

namespace BOA.Models
{
    public class Commit
    {
        public string CommitId { get; set; }
        public int ProjectId { get; set; }
        public DateTimeOffset Date { get; set; }
        public Changes Changes { get; set; }
    }
}
