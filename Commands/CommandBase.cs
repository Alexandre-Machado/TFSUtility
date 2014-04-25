using System;
using System.IO;
using System.Net;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TFSUtility.Commands
{
    public class CommandBase
    {
        protected Parameters parameters;
        protected TfsTeamProjectCollection collection;

        protected TextWriter outputConsole;
        protected TextWriter outputFile;
        protected WorkItemCollection wic;

        protected void DefineCollection()
        {
            var uri = new Uri(parameters.CollectionUrl);

            ICredentialsProvider credentials = null;
            if (!String.IsNullOrEmpty(parameters.User) && !String.IsNullOrEmpty(parameters.Password))
            {
                credentials = new MapWork.Credentials(parameters.LoginName, parameters.Password, parameters.Domain);
            }

            collection = credentials != null
                                ? TfsTeamProjectCollectionFactory.GetTeamProjectCollection(uri, credentials)
                                : TfsTeamProjectCollectionFactory.GetTeamProjectCollection(uri);
        }

        protected void DefineWorkItemCollection()
        {
            outputConsole.Write("Getting data from WorkItems. ");

            var store = (WorkItemStore)collection.GetService(typeof(WorkItemStore));

            wic = store.Query("SELECT * from WorkItems where [System.TeamProject] = '" + parameters.Project +
                              "' and [System.IterationPath] = '" + parameters.IterationPath +
                              "' and [System.WorkItemType] <> 'User Story'");

            outputConsole.WriteLine("Done.");
        }

        protected Decimal GetValue(Object value)
        {
            if (value == null)
                return 0;

            if (value.ToString() == "")
                return 0;

            Decimal v;
            return Decimal.TryParse(value.ToString(), out v) ? v : 0;
        }

        internal class Credentials : ICredentialsProvider
        {
            private readonly NetworkCredential credentials;

            public Credentials(String user, String password, String domain = null)
            {
                credentials = String.IsNullOrEmpty(domain) ? new NetworkCredential(user, password) : new NetworkCredential(user, password, domain);
            }

            public ICredentials GetCredentials(Uri uri, ICredentials failedCredentials)
            {
                return credentials;
            }

            public void NotifyCredentialsAuthenticated(Uri uri)
            {
            }
        }
    }
}
