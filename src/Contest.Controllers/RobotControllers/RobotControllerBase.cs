using Contest.Core.Models;

namespace Contest.Controllers.RobotControllers
{
    public abstract class RobotControllerBase
    {
        public Problem Problem { get; }
        public IRobotController NextController { get; }

        public RobotControllerBase(Problem problem, IRobotController nextController)
        {
            Problem = problem;
            NextController = nextController;
        }
    }
}