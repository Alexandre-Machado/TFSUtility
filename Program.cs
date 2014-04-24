using System;
using TFSUtility.Factories;

namespace TFSUtility
{
    class Program
    {
        static void Main(String[] args)
        {
            var parameters = ParameterFactory.GetParameters(args);

            switch (parameters.Command)
            {
                case Parameters.MAP_WORK:
                    var command = CommandFactory.GetMapWork(parameters);
                    command.Execute(Console.Out);

                    break;
            }

            Console.ReadKey();
        }
    }
}
