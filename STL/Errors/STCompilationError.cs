namespace STL.Errors
{
    public class STCompilationError
    {
        public int Line { get; set; }
        public int Column { get; set; }
        public string Message { get; set; }
    }
}