using CommandLineParser.Arguments;

namespace Contest.ConsoleRunner
{
    public class Arguments
    {
        [ValueArgument(typeof(string), 'p', "problems", Description = "Problems folder path")]
        public string ProblemsFolder { get; set; }

        [ValueArgument(typeof(string), 's', "solutions", Description = "Solutions folder path")]
        public string SolutionsFolder { get; set; }

        [ValueArgument(typeof(string), 'c', "cache", Description = "Cache folder path")]
        public string CacheFolder { get; set; }
    }
}