using Contest.Core.Models;
using System.Collections.Generic;

namespace Contest.Controllers.RobotControllers
{
    public interface IRobotController
    {
        IEnumerable<RobotAction> GetNextActions(Robot robot);
    }
}