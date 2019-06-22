using Contest.Core.Helpers;
using Contest.Core.Loaders;
using NUnit.Framework;
using System.IO;

namespace Contest.Tests.CoreTests
{
    [TestFixture]
    public class ProblemLoaderTests
    {
        public string ProblemsFolderPath { get; set; }

        [SetUp]
        public void SetUp()
        {
            ProblemsFolderPath = ProblemsFinder.FindProblemsFolderPath();
        }

        [Test]
        public void TestProblemLoader()
        {
            var problemFile = Path.Join(ProblemsFolderPath, "prob-051.desc");

            var p = ProblemLoader.LoadProblem(problemFile, null);
        }
    }
}