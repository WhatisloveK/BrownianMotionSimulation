using System;
using System.Collections.Generic;
using System.Text;

namespace BrownianMotionSimulation
{
    public class SupplementMovementEventArgs : EventArgs
    {
        public int OldPosition { get; set; }
        public int CurrentPosition { get; set; }
    }
}
