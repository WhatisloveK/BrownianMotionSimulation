using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BrownianMotionSimulation
{
    public class Supplement
    {
        protected int Position { get; set; }
        protected int N { get; set; }
        protected double P { get; set; }

        Random _random;
        protected Random Random => _random ??= new Random();

        public delegate void SupplementMovementEventHandler(object sender, SupplementMovementEventArgs e);
        public event SupplementMovementEventHandler SupplementMovementEvent;

        public Supplement( int n, double p)
        {
            Position = 0;
            N = n;
            P = p;
        }

        public void StartMove(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var mean = Random.NextDouble();

                if (mean > P)
                {
                    if (Position + 1 < N)
                    {
                        Position += 1;
                        SupplementMovementEvent(this, new SupplementMovementEventArgs { OldPosition = Position - 1, CurrentPosition = Position });
                    }
                }
                else
                {
                    if (Position > 0)
                    {
                        Position -= 1;
                        SupplementMovementEvent(this, new SupplementMovementEventArgs { OldPosition = Position + 1, CurrentPosition = Position });
                    }
                }
            }
        }
    }
}
