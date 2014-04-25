using System;

namespace TFSUtility.Factories
{
    public static class ParameterFactory
    {
        public const String COMMAND_PARAMETER = "command:";
        public const string URL_PARAMETER = "url:";
        public const string PROJECT_PARAMETER = "project:";
        public const string INTERATIONPATH_PARAMETER = "iterationpath:";
        public const string USER_PARAMETER = "user:";
        public const string PASSWORD_PARAMETER = "password:";

        public static Parameters GetParameters(String[] args)
        {
            var parameters = new Parameters();

            foreach(var arg in args)
            {
                if (arg.Length > COMMAND_PARAMETER.Length && arg.Substring(0, COMMAND_PARAMETER.Length).ToLower() == COMMAND_PARAMETER)
                {
                    parameters.Command = arg.Substring(COMMAND_PARAMETER.Length).ToLower();
                }
                else if (arg.Length > URL_PARAMETER.Length && arg.Substring(0, URL_PARAMETER.Length).ToLower() == URL_PARAMETER)
                {
                    parameters.Url = arg.Substring(URL_PARAMETER.Length).ToLower();
                }
                else if (arg.Length > PROJECT_PARAMETER.Length && arg.Substring(0, PROJECT_PARAMETER.Length).ToLower() == PROJECT_PARAMETER)
                {
                    parameters.Project = arg.Substring(PROJECT_PARAMETER.Length).ToLower();
                }
                else if (arg.Length > INTERATIONPATH_PARAMETER.Length && arg.Substring(0, INTERATIONPATH_PARAMETER.Length).ToLower() == INTERATIONPATH_PARAMETER)
                {
                    parameters.IterationPath = arg.Substring(INTERATIONPATH_PARAMETER.Length).ToLower();
                }
                else if (arg.Length > USER_PARAMETER.Length && arg.Substring(0, USER_PARAMETER.Length).ToLower() == USER_PARAMETER)
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
                else if (arg.Length > PASSWORD_PARAMETER.Length && arg.Substring(0, PASSWORD_PARAMETER.Length).ToLower() == PASSWORD_PARAMETER)
                {
                    parameters.Password = arg.Substring(PASSWORD_PARAMETER.Length).ToLower();
                }
            }

            return parameters;
        }
    }
}
