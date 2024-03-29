﻿using Contest.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Controllers.RobotControllers
{
    public class ScoreNActionsController : RobotController, IRobotController
    {
        public List<List<RobotAction>> ActionList = new List<List<RobotAction>>();

        public ScoreNActionsController(Problem problem, IRobotController nextController) : base(problem, nextController)
        {
            var actions = new[]
            {
                RobotAction.Up, RobotAction.Down, RobotAction.Left, RobotAction.Right, RobotAction.TurnLeft,
                RobotAction.TurnRight
            };

            // generate list of all possible 1 move actions
            foreach (var a in actions)
            {
                ActionList.Add(new List<RobotAction> { a });
            }

            // generate all 2 move actions
            foreach (var a in actions)
                foreach (var b in actions)
                    ActionList.Add(new List<RobotAction> { a, b });

            // generate all 3 move actions
            foreach (var a in actions)
                foreach (var b in actions)
                    foreach (var c in actions)
                        ActionList.Add(new List<RobotAction> { a, b, c });

            // generate all 4 move actions
            foreach (var a in actions)
                foreach (var b in actions)
                    foreach (var c in actions)
                        foreach (var d in actions)
                            ActionList.Add(new List<RobotAction> { a, b, c, d });

            // generate all 5 move actions
            foreach (var a in actions)
                foreach (var b in actions)
                    foreach (var c in actions)
                        foreach (var d in actions)
                            foreach (var e in actions)
                                ActionList.Add(new List<RobotAction> { a, b, c, d, e });

            //foreach (var a in actions)
            //    foreach (var b in actions)
            //        foreach (var c in actions)
            //            foreach (var d in actions)
            //                foreach (var e in actions)
            //                    foreach (var f in actions)
            //                        ActionList.Add(new List<RobotAction> { a, b, c, d, e, f });

            //foreach (var a in actions)
            //    foreach (var b in actions)
            //        foreach (var c in actions)
            //            foreach (var d in actions)
            //                foreach (var e in actions)
            //                    foreach (var f in actions)
            //                        foreach (var g in actions)
            //                            ActionList.Add(new List<RobotAction> { a, b, c, d, e, f, g });

            Console.WriteLine($"Evaluating {ActionList.Count} possible move sets");
        }

        public IEnumerable<RobotAction> GetNextActions(Robot robot)
        {
            var bestScore = 0;
            var bestActions = (List<RobotAction>)null;

            var routeActions = NextController.GetNextActions(robot);

            foreach (var actions in ActionList)
            {
                var score = Problem.ScoreActions(robot, actions, robot.Targets);
                if ((score - actions.Count) > 1 && (score - actions.Count) > bestScore)
                {
                    bestScore = (score - actions.Count);
                    bestActions = actions;
                }
            }

            if (bestScore > 0)
            {
                return bestActions.Take(1);
            }

            return routeActions.Take(1);
        }
    }
}