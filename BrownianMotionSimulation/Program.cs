using System;
using System.Linq;
using System.Threading;

namespace BrownianMotionSimulation
{
    public class Program
    {
        private static readonly int statusTimer = 5;

        public static int N;
        public static int K;
        public static double P;
        public static bool IsSynchronized;
        private static int SimulationTime;

        static void Main(string[] args)
        {
            if (args?.Length > 0)
            {
                N = int.Parse(args[0]);
                K = int.Parse(args[1]);
                P = double.Parse(args[2]);
                IsSynchronized = bool.Parse(args[3]);
                SimulationTime = int.Parse(args[4]);
            }
            else
            {
                N = 70;
                K = 50;
                P = 0.5d;
                IsSynchronized = true;
                SimulationTime = 60;
            }
            SetupSimulation();
        }

        public static void DisplayCells(object obj)
        {
            var cells = ((Crystal)obj).Cells.ToList();

            Console.WriteLine(cells.Select(x => x.ToString()).Aggregate((x, y) => $"{x}, {y}"));
        }

        public static void SetupSimulation()
        {
            var crystal = new Crystal(N, K, P, IsSynchronized);
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            TimerCallback timerCallback = new TimerCallback(DisplayCells);
            crystal.RunSimulation(cancellationToken);
            Timer timer = new Timer(timerCallback, crystal, TimeSpan.FromSeconds(statusTimer), TimeSpan.FromSeconds(statusTimer));
            Thread.Sleep(TimeSpan.FromSeconds(SimulationTime));
            cancellationTokenSource.Cancel();
            timer.Dispose();
            Console.WriteLine(crystal.Cells.Sum());
        }
    }
}
