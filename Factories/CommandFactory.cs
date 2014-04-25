using TFSUtility.Commands;

namespace TFSUtility.Factories
{
    public static class CommandFactory
    {
        public static ICommand GetMapWork(Parameters parameters)
        {
            return new MapWork(parameters);
        }

        public static ICommand GetWorkOfUser(Parameters parameters)
        {
            return new WorkOfUser(parameters);
        }
    }
}
