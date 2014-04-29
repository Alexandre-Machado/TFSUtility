using System;
using System.Linq;

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
        public const String LOGINNAME_PARAMETER = "loginName:";
        public const String PASSWORD_PARAMETER = "password:";
        public const String COMPLETEDWORKFIELD_PARAMETER = "completedWorkField:";
        public const String RESULTTOFILE_PARAMETER = "resultToFile:";

        private static String GetParameter(String[] args, String parameterName)
        {
            var index = args.ToList().FindIndex(
                a =>
                a.Length > parameterName.Length &&
                a.Substring(0, parameterName.Length).ToLower() == parameterName.ToLower());

            return index > -1 ? args[index].Substring(parameterName.Length).ToLower() : null;
        }

        public static Parameters GetParameters(String[] args)
        {
            var parameters = new Parameters
                                 {
                                     CompletedWork = "Completed Work",
                                     Command = GetParameter(args, COMMAND_PARAMETER),
                                     CollectionUrl = GetParameter(args, COLLECTIONURL_PARAMETER),
                                     Project = GetParameter(args, PROJECT_PARAMETER),
                                     IterationPath = GetParameter(args, INTERATIONPATH_PARAMETER),
                                     ResultToFile = GetParameter(args, RESULTTOFILE_PARAMETER),
                                     User = GetParameter(args, USER_PARAMETER),
                                     Password = GetParameter(args, PASSWORD_PARAMETER)
                                 };

            parameters.CompletedWork = GetParameter(args, COMPLETEDWORKFIELD_PARAMETER);

            foreach(var arg in args)
            {
                if (arg.ToLower() == HELP_PARAMETER.ToLower())
                {
                    parameters.Help = true;
                }
                
                if (arg.Length > LOGINNAME_PARAMETER.Length && arg.Substring(0, LOGINNAME_PARAMETER.Length).ToLower() == LOGINNAME_PARAMETER.ToLower())
                {
                    var loginName = arg.Substring(LOGINNAME_PARAMETER.Length).Split('\\');

                    if (loginName.Length > 1)
                    {
                        parameters.Domain = loginName[0];
                        parameters.LoginName = loginName[1];
                    }
                    else
                    {
                        parameters.User = loginName[0];
                    }
                }
            }

            return parameters;
        }
    }
}
