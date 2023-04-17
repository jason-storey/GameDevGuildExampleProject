using System;
using System.Collections.Generic;
using System.Linq;
using PokemonApp;

public class InvertedIndexQuery<T>
{
    readonly InvertedIndex<T> _index;

    public InvertedIndexQuery(InvertedIndex<T> index) => _index = index;

    public IEnumerable<T> ExecuteQuery(string query,Func<string,string> beforeTokenProcess = null)
    {
        if(string.IsNullOrWhiteSpace(query)) return Enumerable.Empty<T>();
        try
        {
            var tokens = Tokenize(query);
            var postfix = ConvertToPostfix(tokens);
            var stack = new Stack<IEnumerable<T>>();
            foreach (var token in postfix)
            {
                switch (token)
                {
                    case "AND":
                    case "&":
                    {
                        var right = stack.Pop();
                        var left = stack.Pop();
                        stack.Push(left.Intersect(right));
                        break;
                    }
                    case "OR":
                    case "|":
                    {
                        var right = stack.Pop();
                        var left = stack.Pop();
                        stack.Push(left.Union(right));
                        break;
                    }
                    case "!":
                    case "NOT":
                    {
                        IEnumerable<T> items = !stack.Any() ? _index : stack.Pop();
                        stack.Push(_index.Values.Except(items));
                        break;
                    }
                    default:
                    {
                        var actualToken = beforeTokenProcess == null ?  token : beforeTokenProcess?.Invoke(token);
                        stack.Push(_index[actualToken]);
                        break;
                    }
                }
            }

            return stack.Pop();
        }
        catch
        {
            return Enumerable.Empty<T>();
        }
    }

    static IEnumerable<string> Tokenize(string query)
    {
        var tokens = new List<string>();
        var currentToken = "";
        for (var i = 0; i < query.Length; i++)
        {
            var c = query[i];
            if (c == ' ' || c == '(' || c == ')')
            {
                if (currentToken.Length > 0)
                {
                    tokens.Add(currentToken);
                    currentToken = "";
                }
                if (c != ' ')
                {
                    tokens.Add(c.ToString());
                }
            }
            else
            {
                currentToken += c;
            }
        }
        if (currentToken.Length > 0)
        {
            tokens.Add(currentToken);
        }
        return tokens;
    }

    static IEnumerable<string> ConvertToPostfix(IEnumerable<string> infix)
    {
        var precedence = new Dictionary<string, int>
        {
            { "OR", 1 },
            { "AND", 2 },
            { "NOT", 3 }
        };
        var output = new List<string>();
        var stack = new Stack<string>();
        foreach (var token in infix)
        {
            switch (token)
            {
                case "(":
                {
                    stack.Push(token);
                    break;
                }
                case ")":
                {
                    while (stack.Count > 0 && stack.Peek() != "(")
                    {
                        output.Add(stack.Pop());
                    }
                    if (stack.Count == 0)
                    {
                        throw new ArgumentException("Unmatched parentheses");
                    }
                    stack.Pop();
                    break;
                }
                default:
                {
                    if (precedence.ContainsKey(token))
                    {
                        while (stack.Count > 0 && stack.Peek() != "(" && precedence[stack.Peek()] >= precedence[token])
                        {
                            output.Add(stack.Pop());
                        }
                        stack.Push(token);
                    }
                    else
                    {
                        output.Add(token);
                    }
                    break;
                }
            }
        }
        while (stack.Count > 0 && stack.Peek() != "(")
        {
            output.Add(stack.Pop());
        }
        if (stack.Count > 0)
        {
            throw new ArgumentException("Unmatched parentheses");
        }
        return output;
    }
}