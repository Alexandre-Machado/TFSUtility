using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TFSUtility.Commands
{
    public class WorkOfUser : CommandBase, ICommand
    {
        private readonly List<DateOfWok> dates = new List<DateOfWok>();

        public WorkOfUser(Parameters parameters)
        {
            this.parameters = parameters;
        }

        public void Execute(TextWriter console)
        {
            outputConsole = console;

            if (parameters.Help)
            {
                PrintHelp();

                return;
            }

            if (String.IsNullOrEmpty(parameters.CollectionUrl) || String.IsNullOrEmpty(parameters.Project) || String.IsNullOrEmpty(parameters.IterationPath) || String.IsNullOrEmpty(parameters.User))
            {
                outputConsole.WriteLine("**** Missing parameters ****");
                outputConsole.WriteLine();

                PrintHelp();

                return;
            }

            if (!String.IsNullOrEmpty(parameters.ResultToFile))
            {
                outputFile = new StreamWriter(parameters.ResultToFile);
            }

            PrintStartCommand();

            DefineCollection();
            DefineWorkItemCollection();
            DefineWork();

            PrintHeader();
            PrintWork();

            if (outputFile != null)
            {
                outputConsole.WriteLine();
                outputConsole.WriteLine("Result posted on file: " + parameters.ResultToFile);

                outputFile.Close();
            }
        }

        private void DefineWork()
        {
            outputConsole.Write("Getting work of user ");
            outputConsole.Write("(" + wic.Count +  ") ");

            var counter = 0;
            var puller = 0;

            foreach (var wi in wic.Cast<WorkItem>())
            {
                foreach (var revision in wi.Revisions.Cast<Revision>())
                {
                    var user = Convert.ToString(revision.Fields["Changed By"].Value);
                    var date = Convert.ToDateTime(revision.Fields["Revised Date"].Value);

                    if (user.ToLower() != parameters.User.ToLower()) continue;
                    if (date.Day == 1 && date.Month == 1 && date.Year == 9999) continue;
                    if (!revision.Fields.Contains(parameters.CompletedWork)) continue;

                    var originalValue = GetValue(revision.Fields[parameters.CompletedWork].OriginalValue);
                    var value = GetValue(revision.Fields[parameters.CompletedWork].Value);

                    if (value - originalValue != 0)
                    {
                        var dateIndex = dates.FindIndex(d => d.Date.Date == date.Date);
                        if (dateIndex == -1)
                        {
                            dates.Add(new DateOfWok {Date = date.Date, Work = new Dictionary<String, Decimal>()});
                            dateIndex = dates.Count - 1;
                        }

                        if(!dates[dateIndex].Work.ContainsKey(wi.Id + " - " + wi.Title))
                        {
                            dates[dateIndex].Work.Add(wi.Id + " - " + wi.Title, 0);
                        }

                        dates[dateIndex].Work[wi.Id + " - " + wi.Title] += value - originalValue;
                    }
                }

                counter++;

                if (counter != 10) continue;

                counter = 0;
                puller++;
                outputConsole.Write(Convert.ToString(puller * 10) + "... ");
            }

            outputConsole.WriteLine(Convert.ToString(wic.Count) + ". Done.");

            dates.Sort((d1, d2) => d1.Date.CompareTo(d2.Date));

            outputConsole.WriteLine();
            outputConsole.WriteLine();
        }

        private void PrintHeader()
        {
            var output = outputFile ?? outputConsole;

            if (outputFile != null)
            {
                output.WriteLine("Result");
                output.WriteLine("=========================================================");
            }

            output.WriteLine();
        }

        private void PrintHelp()
        {
            outputConsole.WriteLine("Work of User");
            outputConsole.WriteLine("============");
            outputConsole.WriteLine();
            outputConsole.WriteLine("Synopsis");
            outputConsole.WriteLine("--------");
            outputConsole.WriteLine("   TFSUtiliy command:workofuser ");
            outputConsole.WriteLine("             collectionUrl:<collection url>");
            outputConsole.WriteLine("             project:<project>");
            outputConsole.WriteLine("             iterationPath:<full iteration path>");
            outputConsole.WriteLine("             user: <name of user>");
            outputConsole.WriteLine("             [loginName:<login with optional domin> password:<password>]");
            outputConsole.WriteLine("             [completedWorkField:<name of field>");
            outputConsole.WriteLine("             [resultToFile:<file name>");
            outputConsole.WriteLine();
            outputConsole.WriteLine("Description");
            outputConsole.WriteLine("-----------");
            outputConsole.WriteLine("   List the work completed by date of user.");
            outputConsole.WriteLine();
        }

        private void PrintStartCommand()
        {
            outputConsole.WriteLine("TFSUtiliy: Work of User");
            outputConsole.WriteLine("=======================");
            outputConsole.WriteLine();
        }

        private void PrintWork()
        {
            var output = outputFile ?? outputConsole;

            foreach (var date in dates)
            {
                output.WriteLine(date.Date.ToString("dd/MM/yyyy"));
                output.WriteLine("----------");

                foreach (var work in date.Work)
                {
                    output.WriteLine("   " + work.Key + ": " + Convert.ToString(work.Value));
                }

                output.WriteLine();
            }
        }

        internal class DateOfWok
        {
            public DateTime Date { get; set; }
            public Dictionary<String, Decimal> Work { get; set; }
        }
    }
}