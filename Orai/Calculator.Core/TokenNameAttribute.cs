namespace Calculator.Core;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
internal class TokenNameAttribute : Attribute
{
    public TokenNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}