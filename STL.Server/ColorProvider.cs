using LanguageServer.VsCode.Contracts;
using LanguageServer.VsCode.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace STL.Server
{
    public class ColorProvider
    {
        public ICollection<ColorInformation> ColorDocument(TextDocument document)
        {
            Program.logWriter.WriteLine(String.Format("Colorize"));
            return new List<ColorInformation>() { new ColorInformation() { color = new Color() { red = 255, green = 0, blue = 0, alpha = 125 }, range = new Range(1, 1, 1, 4) } };
        }
    }
}
