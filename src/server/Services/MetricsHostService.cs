using System.Diagnostics;

namespace V.TouristGuide.Server.Services
{
    public class MetricsHostService : IHostedService, IDisposable
    {
        private CancellationToken token;
        private Task task;
        private volatile bool report = false;

        public MetricsHostService()
        {
            this.task = Task.Run(this.ReportMetrics);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.token = cancellationToken;
            this.report = true;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.report = false;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this.task.Dispose();
        }

        private void ReportMetrics()
        {
            while (true)
            {
                if (token != default && token.IsCancellationRequested)
                {
                    return;
                }

                if (this.report)
                {
                    var beginUsage = Process.GetCurrentProcess().TotalProcessorTime;
                    var begin = DateTime.Now;
                    Thread.Sleep(500);
                    var cpuUsage = (Process.GetCurrentProcess().TotalProcessorTime - beginUsage).TotalMilliseconds;
                    var passTime = (DateTime.Now - begin).TotalMilliseconds;
                    var cpuUsagePercent = cpuUsage / passTime / Environment.ProcessorCount * 100;
                    using var client = new HttpClient();
                    client.GetAsync($"https://vbranch.cn/talog/metric/add?index=tg&name=cpu&value={Math.Round(cpuUsagePercent, 2)}").Wait();
                    client.GetAsync($"https://vbranch.cn/talog/metric/add?index=tg&name=memory&value={Math.Round(Process.GetCurrentProcess().WorkingSet64 / 1024.0 / 1024, 2)}").Wait();
                }

                Thread.Sleep(30000);
            }
        }
    }
}
