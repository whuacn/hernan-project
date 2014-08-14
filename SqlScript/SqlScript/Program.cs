using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace SqlScript
{
    class Program
    {
        static void Main(string[] args)
        {
            ParserResult<ArgsOptions> result = null;
            try
            {
                result = CommandLine.Parser.Default.ParseArguments<ArgsOptions>(args);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                System.Environment.Exit(10);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                System.Environment.Exit(10);
            }


            if (!result.Errors.Any())
            {
                try
                {
                    SqlManagement sqlm = new SqlManagement();
                    sqlm.Generar(result.Value);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e.Message);
                    System.Environment.Exit(10);
                }

            }
        }
    }
}
