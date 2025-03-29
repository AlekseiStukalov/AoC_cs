using Common.Helpers.DataStructures;

namespace Common.Helpers
{
    public class PolishNotation<T> where T : EquationElement
    {
        public double Answer;
        public List<T> MathEquation;

        public List<T> PolishNotationEquation;

        private Dictionary<char, int> ArithmeticSignPriorities;

        private PolishNotation(Dictionary<char, int>? priorities)
        {
            PolishNotationEquation = new();
            MathEquation = new();
            ArithmeticSignPriorities = priorities ?? GetDefaultArithmeticPriorities();
        }

        public PolishNotation(string arithmeticEquation, Dictionary<char, int>? priorities = null) : this(priorities)
        {
            string spacelessEquation = string.Concat(arithmeticEquation.Where(c => !char.IsWhiteSpace(c)));
            MathEquation = ParseStringEquation(spacelessEquation);
        }

        public PolishNotation(List<T> equation, Dictionary<char, int>? priorities = null) : this(priorities)
        {
            MathEquation = equation;
        }

        public double Solve()
        {
            ConstructReversePolishNotation();
            SolvePolishNotation();
            return Answer;
        }

        public List<T> ConstructReversePolishNotation()
        {
            var stack = new Stack<T>();

            foreach (var elem in MathEquation)
            {
                if (elem.IsNumber)
                {
                    PolishNotationEquation.Add(elem);
                }
                else
                {
                    if (elem.IsOpenBrace)
                    {
                        stack.Push(elem);
                    }
                    else if (elem.IsCloseBrace)
                    {
                        while (stack.Any())
                        {
                            if (!stack.Peek().IsOpenBrace)
                            {
                                PolishNotationEquation.Add(stack.Pop());
                            }
                            else
                            {
                                stack.Pop();
                                break;
                            }
                        }
                    }
                    else if (ArithmeticSignPriorities.ContainsKey(elem.Sign))
                    {
                        while (stack.Any() && (ArithmeticSignPriorities[stack.Peek().Sign] > ArithmeticSignPriorities[elem.Sign]))
                        {
        		            PolishNotationEquation.Add(stack.Pop());
                        }

                        stack.Push(elem);
                    }
                }
            }

            while (stack.Any())
            {
                PolishNotationEquation.Add(stack.Pop());
            }

            return PolishNotationEquation;
        }

        public double SolvePolishNotation()
        {
            if (!PolishNotationEquation.Any())
            {
                return 0;
            }

            var stack = new Stack<EquationElement>();

            foreach(EquationElement element in PolishNotationEquation)
            {
                if (element.IsNumber)
                {
                    stack.Push(element);
                }
                else
                {
                    double last = stack.Pop().Value;
                    double prev = stack.Pop().Value;
                    double result = DoOperation(prev, last, element.Sign);
                    stack.Push(new EquationElement(result));
                }
            }

            Answer = stack.Pop().Value;

            return Answer;
        }

        private double DoOperation(double prev, double last, char sign)
        {
            try
            {
                switch (sign)
                {
                    default:
                    case '+': return checked(prev + last);
                    case '-': return checked(prev - last);
                    case '*': return checked(prev * last);
                    case '/': return checked(prev / last);

                }
            }
            catch (OverflowException)
            {
                Console.WriteLine("Overflow");
                return 0;
            }
        }

        private List<T> ParseStringEquation(string strEquation)
        {
            List<T> equation = new List<T>();
            foreach(char elemChar in strEquation)
            {
                if (char.IsDigit(elemChar))
                {
                    int value = int.Parse($"{elemChar}");
                    T? eqElem = (T?)Activator.CreateInstance(typeof(T), new object[] { value });

                    if(eqElem != null)
                    {
                        equation.Add(eqElem);
                    }
                    else
                    {
                        Console.Write("E");
                    }
                }
                else
                {
                    T? eqElem = (T?)Activator.CreateInstance(typeof(T), new object[] { $"{elemChar}"});
                    
                    if(eqElem != null)
                    {
                        equation.Add(eqElem);
                    }
                    else
                    {
                        Console.Write("E");
                    }
                }
            }

            return equation;
        }

        private Dictionary<char, int> GetDefaultArithmeticPriorities()
        {
            return new Dictionary<char, int> 
            {
                { '(', 0 },
                { ')', 0 },
                { '+', 1 },
                { '-', 1 },
                { '/', 2 },
                { '*', 2 },
                { '^', 3 },
            };
        }
    }
}