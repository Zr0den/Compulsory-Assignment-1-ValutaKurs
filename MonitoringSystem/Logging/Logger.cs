using MonitoringSystem.Tracing;
using System;
using System.IO;
using System.Reflection.Emit;

namespace MonitoringSystem.Logging
{
    public class Logger
    {
        //Instead of writing to a file, this could very easily be made to write to a database (or something else) instead
        private readonly string _logFilePath;
        public readonly TraceService _traceService;

        public Logger(string logFilePath)
        {
            _logFilePath = logFilePath;
            _traceService = TraceService.GetInstance();
        }

        public void Log(LogLevel level, string message)
        {

            string traceInfo = String.Empty;
            if (_traceService.ActiveTraces.Count > 0)
            {
                Tracer trace = _traceService.ActiveTraces.Peek();
                traceInfo = $"[TraceID: {trace.TraceId}, SpanId: {trace.SpanId}, ParentId: {trace.ParentId.ToString() ?? "None"}]";
            }

            string logMessage = $"{DateTime.Now}: {traceInfo}[{level}] {message}";
            Console.WriteLine(logMessage);
            WriteLogToFile(logMessage);
        }

        private async void WriteLogToFile(string message)
        {
            try
            {
                using (var writer = new StreamWriter(_logFilePath, true))
                {
                    await writer.WriteLineAsync(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to logfile: {_logFilePath}. Exception: {ex.Message}");
            }
        }
    }
}