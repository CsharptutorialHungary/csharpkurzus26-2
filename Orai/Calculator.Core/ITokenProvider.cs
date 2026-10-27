using Calculator.Core.Operations;

namespace Calculator.Core;

internal interface ITokenProvider
{
    IReadOnlyDictionary<string, IToken> Tokens { get; }
}
