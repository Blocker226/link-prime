using System;
using System.Threading;

namespace Discord_Link_Prime {
    class StatTimer {
        static Timer upTime;

        public static void TimeStats () {
#if DEBUG
            Program.LogLine("Timer Started");
#endif
            upTime = new Timer(_ =>
            {
                Program.currentUptime += 1;
                Program.loadedStats.totalUptime += 1;
                Program.WriteStats();
#if DEBUG
                Program.LogLine(string.Format("{0}hrs {1:00}mins", Program.currentUptime / 60, Program.currentUptime));
#endif
            },
            null,
        TimeSpan.FromMinutes(1),
        TimeSpan.FromMinutes(1));
        }
        
    }
}
