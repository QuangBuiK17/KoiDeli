﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.MiddleWares
{
    public class PerformanceTimeMiddleware : IMiddleware
    {
        private readonly Stopwatch _stopwatch;

        public PerformanceTimeMiddleware(Stopwatch stopwatch)
        {
            _stopwatch = stopwatch;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwatch.Restart();
            _stopwatch.Start();
            Console.WriteLine("start performance recored");
            await next(context);
            Console.WriteLine("end performance recored");
            _stopwatch.Stop();
            TimeSpan timeTaken = _stopwatch.Elapsed;
            Console.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));
        }
    }
}
