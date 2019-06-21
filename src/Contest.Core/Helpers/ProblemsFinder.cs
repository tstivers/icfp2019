using System.IO;

namespace Contest.Core.Helpers
{
    public static class ProblemsFinder
    {
        public static string FindProblemsFolderPath()
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            while (!Directory.Exists(Path.Combine(currentDirectory, "problems")))
            {
                currentDirectory = Path.GetFullPath(Path.Combine(currentDirectory, ".."));
            }

            return Path.Combine(currentDirectory, "problems");
        }
    }
}