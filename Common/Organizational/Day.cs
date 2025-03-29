using System.Runtime.InteropServices;

namespace Common.Organizational
{
	public abstract class Day
	{
        string task1Answer;
        string task2Answer;

        public Day()
		{
            task1Answer = "";
            task2Answer = "";
        }

        public virtual int ImplementedNum() => -1;

        public abstract string GetName();
        public abstract void Run();

        public bool HasAnyImplementation() => ImplementedNum() != -1;
        public string GetTask1Answer() => task1Answer;
        public void SetTask1Answer(string answer) => task1Answer = new string(answer);
        public void SetTask1Answer(double answer) => task1Answer = answer.ToString();
        public bool HasAnswer1() => !string.IsNullOrEmpty(task1Answer);

        public string GetTask2Answer() => task2Answer;
        public void SetTask2Answer(string answer) => task2Answer = new string(answer);
        public void SetTask2Answer(double answer) => task2Answer = answer.ToString();
        public bool HasAnswer2() => !string.IsNullOrEmpty(task2Answer);

        public bool IsAllSolved() => ImplementedNum() == 2;

        protected string GetInputPath(Day day)
        {
            string dayNamespace = day.GetType().Namespace ?? "";
            if (string.IsNullOrEmpty(dayNamespace))
            {
                Console.WriteLine("Can't parse day namespace");
                return "";
            }

            string dayPath = dayNamespace.Replace('.', '/');
            string dayNumber = day.GetType().Name.Substring(3);
            
            int dirStepBackAmountMax = 5;
            for (int upAmount = 0; upAmount <= dirStepBackAmountMax; upAmount++)
            {
                //depends on project setup settings, which are different for different OS/IDE
                //vs code = 1 or 0
                //visual studio mac = 4
                //visual studio windows = 3

                string outerDirs = "";
                for (int i = 0; i < upAmount; i++)
                {
                    outerDirs += @"../";
                }

                string inputPath = @$"{outerDirs}{dayPath}/Input{dayNumber}.txt";

                if (Path.Exists(inputPath))
                {
                    return inputPath;
                }
            }

            return "";
        }

        protected string GetInputPath_Task2(Day day) => GetInputPath(day).Replace(".txt", "_Task2.txt");

        protected string GetInputPath_Test(Day day) => GetInputPath(day).Replace(@"/Input", @"/TestInput");
    }
}

