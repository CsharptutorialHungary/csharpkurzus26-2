using System.Reflection;

namespace Calculator.Core;

internal class TokenProvider : ITokenProvider
{
    public IReadOnlyDictionary<string, IToken> Tokens { get; }

    public TokenProvider()
    {
        Type[] types = typeof(IToken).Assembly.GetTypes();

        Type[] tokenTypes = types.Where(type => !type.IsInterface
            && !type.IsAbstract
            && type.IsAssignableTo(typeof(IToken)))
            .ToArray();

        var instances = new Dictionary<string, IToken>();

        foreach (var type in tokenTypes)
        {
            TokenNameAttribute? tokenName = type.GetCustomAttribute<TokenNameAttribute>();
            if (tokenName == null) continue;

            if (Activator.CreateInstance(type) is IToken token)
            {
                instances.Add(tokenName.Name, token);
            }
        }

        Tokens = instances;
    }
}
