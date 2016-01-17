using System.Collections.Generic;

namespace BOA.Models
{
    public class User
    {
        public string UserName { get; set; }
        public HashSet<int> Projects { get; set; }
        public List<Commit> Commits { get; set; }
    }
}
