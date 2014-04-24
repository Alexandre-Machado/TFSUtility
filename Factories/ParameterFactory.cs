using System;

namespace TFSUtility.Factories
{
    public static class ParameterFactory
    {
        public static Parameters GetParameters(String[] args)
        {
            var parameters = new Parameters();

            foreach(var arg in args)
            {
                if (arg.Length > 7 && arg.Substring(0,8).ToLower() == "command:")
                {
                    parameters.Command = arg.Substring(8).ToLower();
                }
                else if (arg.Length > 4 && arg.Substring(0, 4).ToLower() == "url:")
                {
                    parameters.Url = arg.Substring(4).ToLower();
                }
                else if (arg.Length > 8 && arg.Substring(0, 8).ToLower() == "project:")
                {
                    parameters.Project = arg.Substring(8).ToLower();
                }
                else if (arg.Length > 14 && arg.Substring(0, 14).ToLower() == "iterationpath:")
                {
                    parameters.IterationPath = arg.Substring(10).ToLower();
                }
                else if (arg.Length > 5 && arg.Substring(0, 5).ToLower() == "user:")
                {
                    var user = arg.Substring(5).Split('\\');

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
                else if (arg.Length > 9 && arg.Substring(0, 9).ToLower() == "password:")
                {
                    parameters.Password = arg.Substring(9).ToLower();
                }
            }

            return parameters;
        }
    }
}
