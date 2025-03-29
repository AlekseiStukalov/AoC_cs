namespace Common.Helpers.DataStructures
{
    public class EquationElement
    {
        public double Value;
        public char Sign;
        public bool IsNumber;
        public bool IsMathSign; 
        public bool IsOpenBrace;
        public bool IsCloseBrace;

        public EquationElement(EquationElement other)
        {
            Value = other.Value;
            Sign = other.Sign;
            IsNumber = other.IsNumber;
            IsMathSign = other.IsMathSign;
            IsOpenBrace = other.IsOpenBrace;
            IsCloseBrace = other.IsCloseBrace;
        }

        public EquationElement(double value)
        {
            Value = value;
            IsNumber = true;
            Sign = '$';
            IsCloseBrace = false;
            IsOpenBrace = false;
            IsMathSign = false;
        }

        public EquationElement(string signChar)
        {
            Value = 0;
            IsNumber = false;
            Sign = signChar[0];
            IsOpenBrace = signChar[0] == '(';
            IsCloseBrace = signChar[0] == ')';
            IsMathSign = !IsOpenBrace && !IsCloseBrace;
        }

        public override string ToString() => IsNumber ? Value.ToString() : $"{Sign}";
    }
}
