using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Processors;

namespace BOA
{
    class Program
    {
        static void Main()
        {
            var data = new CommittedJavaFiles(PullFromBoa);
        }

        private static List<string> PullFromBoa => new Api("tolgamengu", "Boa352", Queries.GetCommitedJavaFiles()).Execute();

        //private static List<string> ReadUsersDataFile => File.ReadAllLines("CommittedJavaFilesList.txt").ToList();
    }
}
