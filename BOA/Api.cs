using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using boa = edu.iastate.cs.boa;

namespace BOA
{
    public class Api
    {
        private readonly string _query;
        private readonly boa.BoaClient _client;

        public Api(string username, string password, string query)
        {
            _query = query;
            _client = new boa.BoaClient();

            try
            {
                _client.login(username, password);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.Read();
                Environment.Exit(0);
            }
        }

        public List<string> Execute(int repository = 0)
        {
            var i = 0;
            var symbol = new[] { ".  ", ".. ", "..." };
            
            var dataSets = _client.getDatasets();
            var data = _client.query(_query, dataSets[repository]);
            Console.Write("query Submitted!");


            data.refresh();
            var compileStatus = data.getCompilerStatus();
            while (compileStatus != boa.CompileStatus.FINISHED)
            {
                Thread.Sleep(1000);
                data.refresh();
                compileStatus = data.getCompilerStatus();
                Console.Write("\rCompilation Status: " + compileStatus + symbol[i]);
                i++;
                if (i == 3) i = 0;
            }
            Console.WriteLine("");


            i = 0;
            data.refresh();
            var execStatus = data.getExecutionStatus();
            while (execStatus != boa.ExecutionStatus.FINISHED)
            {
                Thread.Sleep(1000);
                data.refresh();
                execStatus = data.getExecutionStatus();
                Console.Write("\rRunning Status: " + execStatus + symbol[i]);
                i++;
                if (i == 3) i = 0;
            }

            try
            {
                var result = data.getOutput();
                return result.Trim().Split('\n').ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.Read();
                Environment.Exit(0);
            }

            return null;
        }
    }
}
