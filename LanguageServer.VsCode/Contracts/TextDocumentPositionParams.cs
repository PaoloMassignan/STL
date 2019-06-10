using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageServer.VsCode.Contracts
{
    public class TextDocumentPositionParams
    {
        public TextDocumentIdentifier textDocumen { get; set; }

    	public Position position { get; set; }
    }
}
