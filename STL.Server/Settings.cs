using System;
using System.Collections.Generic;
using System.Text;

namespace STL.Server
{

    public class SettingsRoot
    {
        public LanguageServerSettings Setting { get; set; }
    }

    public class LanguageServerSettings
    {
        public int MaxNumberOfProblems { get; set; } = 10;

        public LanguageServerTraceSettings Trace { get; } = new LanguageServerTraceSettings();
    }

    public class LanguageServerTraceSettings
    {
        public string Server { get; set; }
    }
}
