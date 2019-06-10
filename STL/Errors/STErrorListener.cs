using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Sharpen;
using System;
using System.Collections.Generic;
using System.Text;

namespace STL.Errors
{
    public class STErrorListener : BaseErrorListener, IAntlrErrorListener<int>
    {
        public STErrorListener()
        {
            Reset();
        }

        protected List<STCompilationError> Errors { get; set; }
        public int ErrorCount { get; internal set; }

        public void Reset()
        {
            Errors = new List<STCompilationError>();
        }

        public List<STCompilationError> GetErrors()
        {
            return Errors;
        }

        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            Errors.Add(new STCompilationError() { Line = line, Column = charPositionInLine, Message = msg });

            ErrorCount++;
        }

        public void SyntaxError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            string source = recognizer.InputStream.ToString();//[recognizer.InputStream.Index];

            Errors.Add(new STCompilationError() { Line = line, Column = charPositionInLine, Message = msg});
            ErrorCount++;
        }

        public override void ReportAmbiguity(Parser recognizer, DFA dfa, int startIndex, int stopIndex, bool exact, BitSet ambigAlts, ATNConfigSet configs)
        {
            ErrorCount++;
        }

        public override void ReportAttemptingFullContext(Parser recognizer, DFA dfa, int startIndex, int stopIndex, BitSet conflictingAlts, SimulatorState conflictState)
        {
            ErrorCount++;
        }

        public override void ReportContextSensitivity(Parser recognizer, DFA dfa, int startIndex, int stopIndex, int prediction, SimulatorState acceptState)
        {
            base.ReportContextSensitivity(recognizer, dfa, startIndex, stopIndex, prediction, acceptState);
        }
    }
}
