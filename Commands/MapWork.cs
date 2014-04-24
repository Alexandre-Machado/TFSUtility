using System;
using System.IO;
using System.Net;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TFSUtility.Commands
{
    public class MapWork : ICommand
    {
        private readonly Parameters parameters;

        public MapWork(Parameters parameters)
        {
            this.parameters = parameters;

            //validar os parametros necessários
            //Url
            //Project
            //IterationPath

            //parametros opcionais
            //user (domain\user)
            //password
        }

        public TfsTeamProjectCollection GetCollection()
        {
            var uri = new Uri(parameters.Url);

            ICredentialsProvider credentials = null;
            if (!String.IsNullOrEmpty(parameters.User) && !String.IsNullOrEmpty(parameters.Password))
            {
                if (!String.IsNullOrEmpty(parameters.Domain))
                {
                    credentials = new NetworkCredential(parameters.User, parameters.Password, parameters.Domain) as ICredentialsProvider;
                }
                else
                {
                    credentials = new NetworkCredential(parameters.User, parameters.Password) as ICredentialsProvider;
                }
            }

            return credentials != null
                       ? TfsTeamProjectCollectionFactory.GetTeamProjectCollection(uri, credentials)
                       : TfsTeamProjectCollectionFactory.GetTeamProjectCollection(uri);
        }

        public void Execute(TextWriter console)
        {
            var collection = GetCollection();

            var store = (WorkItemStore) collection.GetService(typeof (WorkItemStore));

            var wic = store.Query("SELECT * from WorkItems where [System.TeamProject] = '" + parameters.Project +
                                  "' and [System.IterationPath] = '" + parameters.IterationPath +
                                  "' and [System.WorkItemType] <> 'User Story'");

            console.WriteLine(parameters.Command);
            console.WriteLine(parameters.Url);
        }
    }
}
