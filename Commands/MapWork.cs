using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TFSUtility.Commands
{
    public class MapWork : ICommand
    {
        private readonly Parameters parameters;
        private TextWriter outputConsole;
        private TfsTeamProjectCollection collection;
        private WorkItemCollection wic;
        private readonly List<User> users = new List<User>();
        private readonly List<DateTime> dates = new List<DateTime>();

        public MapWork(Parameters parameters)
        {
            this.parameters = parameters;

            //Exemplo de chamada
            //TFSUtiliy command:map url:http://tfs2.cwi.com.br/tfs/Collection1 project:ADSFSW "iterationpath:ADSFSW\Release 1\Sprint 9" user:CWINET\ajdias password:bla

            //Falta validar os parametros necessários
            //Url
            //Project
            //IterationPath

            //parametros opcionais
            //user (domain\user)
            //password
        }

        public void Execute(TextWriter console)
        {
            outputConsole = console;

            PrintStartCommand();

            DefineCollection();
            DefineWorkItemCollection();
            DefineUsersAndDates();

            PrintHeaders();
            PrintUsers();
        }

        private void DefineCollection()
        {
            var uri = new Uri(parameters.Url);

            ICredentialsProvider credentials = null;
            if (!String.IsNullOrEmpty(parameters.User) && !String.IsNullOrEmpty(parameters.Password))
            {
                if (!String.IsNullOrEmpty(parameters.Domain))
                {
                    credentials =
                        new NetworkCredential(parameters.User, parameters.Password, parameters.Domain) as
                        ICredentialsProvider;
                }
                else
                {
                    credentials = new NetworkCredential(parameters.User, parameters.Password) as ICredentialsProvider;
                }
            }

            collection = credentials != null
                       ? TfsTeamProjectCollectionFactory.GetTeamProjectCollection(uri, credentials)
                       : TfsTeamProjectCollectionFactory.GetTeamProjectCollection(uri);
        }

        private void DefineUsersAndDates()
        {
            outputConsole.Write("Getting users and dates ");
            outputConsole.Write("(" + wic.Count +  ") ");

            var sizeUser = 0;
            var counter = 0;
            var puller = 0;

            foreach (var wi in wic.Cast<WorkItem>())
            {
                foreach (var revision in wi.Revisions.Cast<Revision>())
                {
                    var user = Convert.ToString(revision.Fields["Changed By"].Value);
                    var date = Convert.ToDateTime(revision.Fields["Revised Date"].Value);

                    if (date.Day == 1 && date.Month == 1 && date.Year == 9999) continue;

                    if (user.Length > sizeUser)
                        sizeUser = user.Length;

                    var userIndex = users.FindIndex(u => u.Name == user);
                    if (userIndex == -1)
                    {
                        users.Add(new User { Name = user, Work = new Dictionary<DateTime, Decimal>() });
                        userIndex = users.Count - 1;
                    }

                    if (!users[userIndex].Work.ContainsKey(date.Date))
                        users[userIndex].Work.Add(date.Date, 0);

                    if (!revision.Fields.Contains("Completed Work")) continue;

                    var originalValue = GetValue(revision.Fields["Completed Work"].OriginalValue);
                    var value = GetValue(revision.Fields["Completed Work"].Value);

                    users[userIndex].Work[date.Date] += value - originalValue;

                    if (!dates.Contains(date.Date))
                        dates.Add(date.Date);
                }

                counter++;

                if (counter != 10) continue;

                counter = 0;
                puller++;
                outputConsole.Write(Convert.ToString(puller * 10) + "... ");
            }

            outputConsole.WriteLine(Convert.ToString(wic.Count) + ". Done.");

            foreach (var user in users)
            {
                user.Name = user.Name.PadRight(sizeUser);
            }

            users.Sort((u1, u2) => String.CompareOrdinal(u1.Name, u2.Name));
            dates.Sort();
        }

        private void DefineWorkItemCollection()
        {
            outputConsole.Write("Getting data from WorkItems. ");

            var store = (WorkItemStore) collection.GetService(typeof (WorkItemStore));

            wic = store.Query("SELECT * from WorkItems where [System.TeamProject] = '" + parameters.Project +
                              "' and [System.IterationPath] = '" + parameters.IterationPath +
                              "' and [System.WorkItemType] <> 'User Story'");

            outputConsole.WriteLine("Done.");
        }

        private Decimal GetValue(Object value)
        {
            if (value == null)
                return 0;

            if (value.ToString() == "")
                return 0;

            Decimal v;
            return Decimal.TryParse(value.ToString(), out v) ? v : 0;
        }

        private void PrintHeaders()
        {
            outputConsole.WriteLine();
            outputConsole.WriteLine();
            outputConsole.WriteLine("Result");
            outputConsole.WriteLine("=========================================================");


            outputConsole.Write("".PadRight(users[0].Name.Length) + " ");
            foreach (var date in dates)
            {
                outputConsole.Write(date.ToString("dd/MM") + " ");
            }
            outputConsole.WriteLine();
        }

        private void PrintStartCommand()
        {
            outputConsole.WriteLine("TFSUtiliy: Map");
            outputConsole.WriteLine("==============");
            outputConsole.WriteLine();
        }

        private void PrintUsers()
        {
            foreach (var user in users)
            {
                outputConsole.Write(user.Name + " ");

                foreach (var date in dates)
                {
                    Decimal work = 0;
                    if (user.Work.ContainsKey(date))
                        work = user.Work[date.Date];

                    outputConsole.Write(Convert.ToString(work).PadLeft(5) + " ");
                }

                outputConsole.WriteLine();
            }
        }

        internal class User
        {
            public String Name { get; set; }
            public Dictionary<DateTime, Decimal> Work { get; set; }
        }
    }
}