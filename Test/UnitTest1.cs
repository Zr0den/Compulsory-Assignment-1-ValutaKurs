using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MonitoringSystem.Tracing;
using MonitoringSystem.Logging;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        TraceService _traceService;
        public class ExampleClass
        {

            [TestMethod]
            public void DoStuff()
            {
                _traceService = TraceService.GetInstance();

                using (var tracer = new Tracer(logger, nameof(DoStuff)))
                {
                    logger.Log(LogLevel.Information, "Start");
                    DoMoreStuff();
                }
                _traceService.GetTraceId();
                var a = 1;
            }

            public void DoMoreStuff()
            {
                using (var tracer = new Tracer(logger, nameof(DoMoreStuff)))
                {
                    logger.Log(LogLevel.Debug, "Mid");
                }
            }

            public void DoEvenMoreStuff()
            {
                using (var tracer = new Tracer(logger, nameof(DoEvenMoreStuff)))
                {
                    logger.Log(LogLevel.Debug, "Stop");
                }
                _traceService.GetTraceId();
                var a = 1;
            }
        }
    }
