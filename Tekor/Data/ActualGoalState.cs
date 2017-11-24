using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tekor.Data
{
    public class ActualGoalState
    {
        public string ID { get; set; }

        public UserAccount User { get; set; }
        public Goal Goal { get; set; }
        public double ActualValue { get; set; }
    }
}
