using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tekor.Data
{
    public class Goal
    {
        public string ID { get; set; }
        public Reward Reward { get; set; }
        public string Description { get; set; }
        public double GoalValue { get; set; }
        public IList<ActualGoalState> actualGoalStates { get; set; }
    }
}
