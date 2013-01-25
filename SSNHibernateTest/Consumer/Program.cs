using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Contacts;


using ServiceStack.ServiceClient.Web;
using ServiceStack.Text;

namespace Consumer
{
    class Program
    {
        private static string _command = string.Empty;
        private static JsonServiceClient _client = new JsonServiceClient("http://localhost:4644");

        static void Main(string[] args)
        {
            while(_command != "exit" || _command != "e")
            {
                switch (_command.ToLower())
                {
                    case "select":
                        SelectCommand();
                        break;

                    default:
                    {
                        UnknownCommand();
                        break;
                    }
                }
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void SelectCommand()
        {

             
            var request = new ProductFind {Id = 1};
            var response = _client.Send<ProductFindResponse>(request);
            Debug.WriteLine(response.Result.ToString());

            response.Result.PrintDump();
            Console.WriteLine(response.Result.Description);

            Console.WriteLine("Please type a command...");
            _command = Console.ReadLine();
        }

      private static void UnknownCommand()
        {
            Console.WriteLine("Command Unknown");
            Console.WriteLine("Please type one of the following commands:");
            Console.WriteLine("select");
          _command = Console.ReadLine();

        }
    }
}
