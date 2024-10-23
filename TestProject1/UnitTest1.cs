using MonitoringSystem.Logging;
using MonitoringSystem.Tracing;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        public TraceService _traceService;
        private static Logger logger;

        public UnitTest1()
        {
            _traceService = TraceService.GetInstance();
            logger = new Logger("app.log");
        }

        [TestMethod]
        public void DoStuff()
        {

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
                DoEvenMoreStuff();
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