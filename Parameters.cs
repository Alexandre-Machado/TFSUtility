using System;

namespace TFSUtility
{
    public class Parameters
    {
        public const String MAPWORK_COMMAND = "map";
        public const String WORKOFUSER_COMMAND = "workofuser";

        public String Command { get; set; }
        public String CollectionUrl { get; set; }
        public String Project { get; set; }
        public String IterationPath { get; set; }
        public String User { get; set; }
        public String LoginName { get; set; }
        public String Domain { get; set; }
        public String Password { get; set; }
        public String CompletedWork { get; set; }
        public String ResultToFile { get; set; }

        public Boolean Help { get; set; }
    }
}
