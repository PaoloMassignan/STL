using Antlr4.Runtime.Misc;

namespace STL.Grammar
{
    partial class STParser
    {
        public IntervalSet LastExpectedTokens { get; set; }

        [return: NotNull]
        public override IntervalSet GetExpectedTokens()
        {
            LastExpectedTokens =  base.GetExpectedTokens();
            return LastExpectedTokens;
        }
    }
}
