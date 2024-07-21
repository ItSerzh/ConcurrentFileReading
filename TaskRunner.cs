using ConcurrentFileReading.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentFileReading
{
    internal class TaskRunner(IConfiguration cfg, IDiskProcessor fileProcessor, IOutput output)
    {
        private readonly string _path = cfg.GetValue<string>("FilesSource");
        private readonly IDiskProcessor _fileProcessor = fileProcessor;
        private readonly IOutput _output = output;

        private async Task Run(int parallelDegree)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var fileInfos = _fileProcessor.GetDirecotryFiles(_path);

            var bunchNumber = 0;
            var processedCount = parallelDegree * bunchNumber;
            var filesBunch = fileInfos.Skip(processedCount).Take(parallelDegree);
            var tasks = new Task[parallelDegree];
            while (filesBunch.Any())
            {
                var itCount = 0;
                foreach (var file in filesBunch)
                {
                    var bunchNum = bunchNumber;
                    var task = Task.Run(() =>
                    {
                        var count = _fileProcessor.GetFileCharCount(file.FullName, ' ');
                        _output.WriteLine($"[{stopWatch.Elapsed.TotalSeconds}][{Task.CurrentId}] butchNumber:{bunchNum} {file.Name}: {count} spaces.");
                    });
                    tasks[itCount++] = task;
                }

                _output.WriteLine($"wait {itCount} tasks");
                Task.WaitAll(tasks);

                bunchNumber++;
                processedCount = parallelDegree * bunchNumber;
                filesBunch = fileInfos.Skip(processedCount).Take(parallelDegree);
            }

            _output.WriteLine($"total: {stopWatch.Elapsed.TotalSeconds}");
        }

        public async Task StartInvestigation()
        {
            var cores = (byte)Environment.ProcessorCount;
            output.WriteLine($"Processor cores count: {cores}");

            output.WriteLine($"*** Start operation in: {cores/3} tasks");
            await Run(cores / 3);

            output.WriteLine($"*** Start operation in: {cores / 2} tasks");
            await Run(cores / 2);

            output.WriteLine($"*** Start operation in: {cores} tasks");
            await Run(cores);

            output.WriteLine($"*** Start operation in: {cores + 4} tasks");
            await Run(cores + 4);
        }
    }
}
