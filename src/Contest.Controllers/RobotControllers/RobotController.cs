using Contest.Core.Models;

namespace Contest.Controllers.RobotControllers
{
    public abstract class RobotController
    {
        public Problem Problem { get; }
        public IRobotController NextController { get; }

        public RobotController(Problem problem, IRobotController nextController)
        {
            Problem = problem;
            NextController = nextController;
        }
    }
}