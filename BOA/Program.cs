using System;
using System.IO;
using System.Linq;
using System.Threading;
using BOA.Processors;
using boa = edu.iastate.cs.boa;

namespace BOA
{
    class Program
    {
        static void Main()
        {
            var formatedOutput = File.ReadAllLines("CommittedJavaFilesList.txt").ToList();

            IProcessor processor = new CommittedJavaFiles(formatedOutput);
            processor.Process();
            /*
            using (boa.BoaClient client = new boa.BoaClient())
            {
                try
                {
                    client.login("tolgamengu", "Boa352");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.Read();
                    Environment.Exit(0);
                }

                var dataSets = client.getDatasets();
                var startTime = DateTime.Now;
                var query = client.query(BOAQueries.GetCommitedJavaFilesList(), dataSets[0]);

                Console.WriteLine("Query Submitted");
                Console.Write("\r\nCompilation Status");

                query.refresh();

                var compileStatus = query.getCompilerStatus();
                while (compileStatus != boa.CompileStatus.FINISHED)
                {
                    Thread.Sleep(1000);
                    query.refresh();
                    compileStatus = query.getCompilerStatus();
                    Console.Write(" -> " + compileStatus);
                }

                Console.Write("\r\n\r\nRunning Status");

                query.refresh();

                var execStatus = query.getExecutionStatus();
                while (execStatus != boa.ExecutionStatus.FINISHED)
                {
                    Thread.Sleep(1000);
                    query.refresh();
                    execStatus = query.getExecutionStatus();
                    Console.Write(" -> " + execStatus);
                }

                var runDuration = DateTime.Now - startTime;

                var result = query.getOutput();

                var formatedOutput = result.Trim().Split('\n').ToList();

                IProcessor processor = new CommittedJavaFiles(formatedOutput);
                processor.Process();

                Console.WriteLine("\r\nTotal duration: " + runDuration);
                Console.ReadLine();
            }
            */
        }
    }
}
