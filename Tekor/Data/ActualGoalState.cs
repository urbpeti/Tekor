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
        public Goal goal { get; set; }
        public double actualValue { get; set; }
    }
}
