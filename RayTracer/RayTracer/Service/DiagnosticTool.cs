using System.Diagnostics;

namespace RayTracer.Service
{
    public class DiagnosticTool
    {
        public bool Active = true;

        private Stopwatch timer = new Stopwatch();

        private int amount = 0;

        private long longest = 0;

        private long shortest = 0;

        private long total = 0;

        private long last = 0;

        /// <summary>
        /// Start a new diagnose.
        /// </summary>
        public void Start()
        {
            if (!Active) return;
            timer.Start();
        }

        /// <summary>
        /// End a diagnose. All values are calculated and can ben retrieved by using the getters.
        /// </summary>
        public void End()
        {
            if (!Active) return;
            timer.Stop();
            long miliseconds = timer.ElapsedMilliseconds;
            last = miliseconds;
            if (miliseconds > longest)
                longest = miliseconds;
            if (shortest > miliseconds || shortest == 0)
                shortest = miliseconds;
            total += miliseconds;
            amount++;
            timer.Reset();
        }

        /// <summary>
        /// Get the amount of Frames Per Second, with a precision of 7 digits.
        /// </summary>
        /// <returns></returns>
        public float GetFPS()
        {
            if (last == 0) return 0;
            return (1 / (float)last) * 1000;
        }

        /// <summary>
        /// Get the average time in miliseconds between each frame since the moment the diagnostic
        /// tool was started.
        /// </summary>
        /// <returns>The average time between frames in miliseconds.</returns>
        public long GetAverageTime()
        {
            if (amount == 0) return total;
            return total / amount;
        }

        /// <summary>
        /// Get the longest time between frames in miliseconds so far since the moment the
        /// diagnostic tool was started.
        /// </summary>
        /// <returns>The longest time between frames in miliseconds.</returns>
        public long GetLongestTime()
        {
            return longest;
        }

        /// <summary>
        /// Get the shortest time between frames in miliseconds so far since the moment the
        /// diagnostic tool was started.
        /// </summary>
        /// <returns>The shortest time between frames in miliseconds.</returns>
        public long GetShortestTime()
        {
            return shortest;
        }

        /// <summary>
        /// Get the latest diagnosed time.
        /// </summary>
        /// <returns>The latest diagnosed time.</returns>
        public long GetLatestTime()
        {
            return last;
        }
    }
}
