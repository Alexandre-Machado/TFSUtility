using System;

namespace TFSUtility.Factories
{
    public static class ParameterFactory
    {
        public const String HELP_PARAMETER = "help";
        public const String COMMAND_PARAMETER = "command:";
        public const String COLLECTIONURL_PARAMETER = "collectionUrl:";
        public const String PROJECT_PARAMETER = "project:";
        public const String INTERATIONPATH_PARAMETER = "iterationpath:";
        public const String USER_PARAMETER = "user:";
        public const String PASSWORD_PARAMETER = "password:";
        public const String COMPLETEDWORKFIELD_PARAMETER = "completedWordField:";
        public const String RESULTTOFILE_PARAMETER = "resultToFile:";

        public static Parameters GetParameters(String[] args)
        {
            var parameters = new Parameters { CompletedWork = "Completed Work" };

            foreach(var arg in args)
            {
                if (arg.ToLower() == HELP_PARAMETER.ToLower())
                {
                    parameters.Help = true;
                }
                else if (arg.Length > RESULTTOFILE_PARAMETER.Length && arg.Substring(0, RESULTTOFILE_PARAMETER.Length).ToLower() == RESULTTOFILE_PARAMETER.ToLower())
                {
                    parameters.ResultToFile = arg.Substring(RESULTTOFILE_PARAMETER.Length).ToLower();
                }
                else if (arg.Length > COMMAND_PARAMETER.Length && arg.Substring(0, COMMAND_PARAMETER.Length).ToLower() == COMMAND_PARAMETER.ToLower())
                {
                    parameters.Command = arg.Substring(COMMAND_PARAMETER.Length).ToLower();
                }
                else if (arg.Length > COMPLETEDWORKFIELD_PARAMETER.Length && arg.Substring(0, COMPLETEDWORKFIELD_PARAMETER.Length).ToLower() == COMPLETEDWORKFIELD_PARAMETER.ToLower())
                {
                    parameters.CompletedWork = arg.Substring(COMPLETEDWORKFIELD_PARAMETER.Length).ToLower();
                }
                else if (arg.Length > COLLECTIONURL_PARAMETER.Length && arg.Substring(0, COLLECTIONURL_PARAMETER.Length).ToLower() == COLLECTIONURL_PARAMETER.ToLower())
                {
                    parameters.CollectionUrl = arg.Substring(COLLECTIONURL_PARAMETER.Length).ToLower();
                }
                else if (arg.Length > PROJECT_PARAMETER.Length && arg.Substring(0, PROJECT_PARAMETER.Length).ToLower() == PROJECT_PARAMETER.ToLower())
                {
                    parameters.Project = arg.Substring(PROJECT_PARAMETER.Length).ToLower();
                }
                else if (arg.Length > INTERATIONPATH_PARAMETER.Length && arg.Substring(0, INTERATIONPATH_PARAMETER.Length).ToLower() == INTERATIONPATH_PARAMETER.ToLower())
                {
                    parameters.IterationPath = arg.Substring(INTERATIONPATH_PARAMETER.Length).ToLower();
                }
                else if (arg.Length > USER_PARAMETER.Length && arg.Substring(0, USER_PARAMETER.Length).ToLower() == USER_PARAMETER.ToLower())
                {
                    var user = arg.Substring(USER_PARAMETER.Length).Split('\\');

                    if (user.Length > 1)
                    {
                        parameters.Domain = user[0];
                        parameters.User = user[1];
                    }
                    else
                    {
                        parameters.User = user[0];
                    }
                }
                else if (arg.Length > PASSWORD_PARAMETER.Length && arg.Substring(0, PASSWORD_PARAMETER.Length).ToLower() == PASSWORD_PARAMETER.ToLower())
                {
                    parameters.Password = arg.Substring(PASSWORD_PARAMETER.Length).ToLower();
                }
            }

            return parameters;
        }
    }
}
