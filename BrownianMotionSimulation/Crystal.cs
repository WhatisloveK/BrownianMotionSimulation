using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrownianMotionSimulation
{
    public class Crystal
    {
        protected int N { get; }
        protected int K { get; }
        protected double P { get; }
        protected bool IsSynchronized { get; }

        private object _locker = new object();
        public List<int> Cells { get; }

        public Crystal(int n, int k, double p, bool isSynchronized)
        {
            N = n;
            K = k;
            P = p;
            IsSynchronized = isSynchronized;
            Cells = (new int[n]).ToList();
            Cells[0] = k;
        }


        private void ActualizeCrystalCells(object sender, SupplementMovementEventArgs e)
        {
            if (IsSynchronized)
            {
                lock (_locker)
                {
                    Cells[e.OldPosition] -= 1;
                    Cells[e.CurrentPosition] += 1;
                }
            }
            else
            {
                Cells[e.OldPosition] -= 1;
                Cells[e.CurrentPosition] += 1;
            }
        }

        public void RunSimulation(CancellationToken stop)
        {
            Parallel.For(0, K, (i) =>
            {
                 var supplement = new Supplement(N, P);
                 supplement.SupplementMovementEvent += ActualizeCrystalCells;
                 var thread = new Thread(() => supplement.StartMove(stop));
                 thread.Name = i.ToString();
                 thread.Start();
            });
        }
    }
}
