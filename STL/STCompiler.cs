using Antlr4.Runtime;
using STL.Errors;
using STL.Grammar;
using System;
using System.Collections.Generic;
using System.Text;

namespace STL
{
    public class STCompiler
    {
        protected STLexer Lexer { get; set; }
        protected STParser Parser { get; set; }

        protected STErrorListener ErrorManager { get; private set; }

        public List<STCompilationError> Errors
        {
            get
            {
                return ErrorManager.GetErrors();
            }
        }

        public bool Compile(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);

            Lexer = new STLexer(inputStream);
            ErrorManager = new STErrorListener();
            CommonTokenStream commonTokenStream = new CommonTokenStream(Lexer);
            Parser = new STParser(commonTokenStream);
            Parser.AddErrorListener(ErrorManager);

            STParser.CompileUnitContext context = Parser.compileUnit();

            return ErrorManager.ErrorCount == 0;
        }

        public List<string> GetExpectedTokens()
        {
            if (this.Parser == null)
                return new List<string>();

            var expectedTokens = this.Parser.LastExpectedTokens;
            if (expectedTokens == null)
                return new List<string>();

            var result = new List<String>();
            for (int i = 0; i < expectedTokens.Count;i++)
            {
                var interval = expectedTokens.GetIntervals()[i];
                for (int j = 0; j < interval.Length; j++)
                {
                    string token = this.Parser.TokenNames[interval.a + j];
                    result.Add(token.Substring(1,token.Length-2));

                }
            }

            return result;
        }
    }
}
