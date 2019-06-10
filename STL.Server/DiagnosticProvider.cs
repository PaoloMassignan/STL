using System;
using System.Collections.Generic;
using System.Text;
using LanguageServer.VsCode.Contracts;
using LanguageServer.VsCode.Server;
using STL;

namespace STL.Server
{
    public class DiagnosticProvider
    {
        protected static STCompiler _compiler = null;
        public static STCompiler Compiler
        {
            get
            {
                if (_compiler == null)
                    _compiler = new STCompiler();
                return _compiler;
            }
        }   



        public DiagnosticProvider()
        {

        }

        public ICollection<Diagnostic> LintDocument(TextDocument document, int maxNumberOfProblems)
        {
            var diag = new List<Diagnostic>();
            var content = document.Content;

            Program.logWriter.WriteLine("Start compilation");

            if (!Compiler.Compile(content))
            {
                Program.logWriter.WriteLine("Errors encountered");
                foreach (var error in Compiler.Errors)
                {
                    diag.Add(new Diagnostic(DiagnosticSeverity.Error,
                    new Range(new Position(error.Line, error.Column), document.PositionAt(content?.Length ?? 0)),
                    document.LanguageId, error.Message));
                }
            }
            Program.logWriter.WriteLine(String.Format("Found {0} errors",Compiler.Errors.Count));
            return diag;
        }
    }
}
