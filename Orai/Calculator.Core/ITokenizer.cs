using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Core;

internal interface ITokenizer
{
    IToken[] Tokenize(string input);
}
