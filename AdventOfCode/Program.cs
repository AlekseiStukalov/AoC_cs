using Common.Organizational;
using AdventOfCode.Y2020;
using AdventOfCode.Y2021;
using AdventOfCode.Y2022;
using AdventOfCode.Y2023;
using AdventOfCode.Y2024;
using System.Diagnostics;

namespace AdventOfCode
{
    class Program
    {
        public static Dictionary<int, YearBase> Advents = new Dictionary<int, YearBase>
        {
            { 2020, new Year2020(isCompleted: true) },
            { 2021, new Year2021(isCompleted: true) },
            { 2022, new Year2022(isCompleted: true) },
            { 2023, new Year2023(isCompleted: false) },
            { 2024, new Year2024(isCompleted: false) },
        };

        private static bool PrintGCStat = true;

        public static void Main(string[] args)
        {
            // YearTemplateGenerator.GenerateFilesForYear(2024);
            // return;

            bool runParticularYear = true;
            Console.WriteLine();

            if (runParticularYear)
            {
                YearBase currentYear = Advents[2024];
                DateTime advStart = currentYear.GetStartDate();

                Console.WriteLine($"Solving year {advStart.Year}");

                List<Day> daysTasks = currentYear.GetDaysTasks();

                int inWorkDay = GetDayInWorkNumber(daysTasks);
                int currentDayNumber = (int)Math.Ceiling((DateTime.Now - advStart).TotalHours / 24);

                if (currentDayNumber > daysTasks.Count)
                {
                    RunPastAdvent(currentYear, inWorkDay);
                }
                else
                {
                    RunRelevantAdvent(currentYear, inWorkDay, currentDayNumber);
                }
            }
            else
            {
                Console.WriteLine("Press any key to start");
                Console.ReadLine();
                Stopwatch totalTime = Stopwatch.StartNew();

                foreach(YearBase year in Advents.Values)
                {
                    Console.WriteLine($"Running year {year.GetStartDate().Year}");
                    AnswersBase answers = year.GetAnswers();
                    List<Day> daysTasks = year.GetDaysTasks();
                    bool isAllImplemented = daysTasks.Select(d => d.IsAllSolved()).All(s => s);

                    RunAllTasks(answers, daysTasks, isAllImplemented);
                }

                Console.WriteLine($"Total time: {totalTime.Elapsed}");
            }

            Console.WriteLine("Press smth to exit");
            Console.Read();
        }

        private static void RunRelevantAdvent(YearBase currentYear, int inWorkDay, int currentDayNumber)
        {
            List<Day> daysTasks = currentYear.GetDaysTasks();
            if (daysTasks[currentDayNumber - 1].ImplementedNum() == -1)
            {
                Console.WriteLine($"Today is {currentDayNumber}th day. " +
                                  "Task from this day is not implemented. " +
                                  $"Running the last existing task (from day {inWorkDay})");
                RunDayTask(daysTasks, inWorkDay - 1);
            }
            else
            {
                if (inWorkDay == currentDayNumber && daysTasks[currentDayNumber - 1].ImplementedNum() == 2)
                {
                    Console.WriteLine("Good job! All tasks for now are done! ");
                    Console.WriteLine("Press Enter to run the last one. Or put day number (or all)");
                    Command command = GetValidCommand(false);

                    RunFromCommand(command, currentYear, false, inWorkDay);
                }
                else
                {
                    Console.WriteLine("Running today's task");
                    RunDayTask(daysTasks, currentDayNumber - 1);
                }
            }
        }

        private static void RunPastAdvent(YearBase currentYear, int inWorkDay)
        {
            Console.WriteLine("The game is over!");

            List<Day> daysTasks = currentYear.GetDaysTasks();
            bool isAllImplemented = daysTasks.Select(d => d.IsAllSolved()).All(s => s);

            if (isAllImplemented)
            {
                Console.WriteLine("Good job!");
                Console.WriteLine("Enter day number to run. Put \"all\" to run all (or just press Enter)");
            }
            else
            {
                Console.WriteLine($"Shame on you =)  Now you're on day {inWorkDay}. Press Enter to run it. Or put day number (or all)");
            }

            Command command = GetValidCommand(isAllImplemented);

            RunFromCommand(command, currentYear, isAllImplemented, inWorkDay);
        }

        public static void RunFromCommand(Command command, YearBase currentYear, bool isAllImplemented, int inWorkDay)
        {
            AnswersBase answers = currentYear.GetAnswers();
            List<Day> daysTasks = currentYear.GetDaysTasks();
            if (command.ExecType == ExecutionType.AllTasks)
            {
                RunAllTasks(answers, daysTasks, isAllImplemented);
            }
            else
            {
                int day;
                if (command.ExecType == ExecutionType.DayNumber)
                {
                    day = command.Value;
                }
                else
                {
                    day = inWorkDay;
                }

                RunDayTask(daysTasks, day - 1);
                ValidateResult(answers, daysTasks[day - 1], day, isAllImplemented);
            }
        }

        public static void RunAllTasks(AnswersBase answers, List<Day> daysTasks, bool isAllImplemented)
        {
            bool isAllCorrect = true;

            var start = DateTime.Now;
            Console.WriteLine();
            for (int i = 0; i < daysTasks.Count; i++)
            {
                RunDayTask(daysTasks, i);

                if (!ValidateResult(answers, daysTasks[i], i + 1, isAllImplemented))
                {
                    isAllCorrect = false;
                }

                Console.WriteLine();
            }
            Console.WriteLine($"All tasks completed in {(DateTime.Now - start).TotalSeconds} sec");
            Console.WriteLine($"Is all correct = {isAllCorrect}");
            Console.WriteLine();
            Console.WriteLine();
        }

        private static bool ValidateResult(AnswersBase answers, Day day, int dayNumber, bool isAllImplemented)
        {
            bool isNeedValidate = isAllImplemented;
            if (!isNeedValidate)
            {
                if (day.HasAnswer1() || day.HasAnswer2())
                {
                    isNeedValidate = true;
                }
            }

            if (isNeedValidate)
            {
                bool isTask1Correct = day.GetTask1Answer().Equals(answers.Get(dayNumber, 1));
                bool isTask2Correct = day.GetTask2Answer().Equals(answers.Get(dayNumber, 2));

                Console.Write("VALIDATION: ");
                PrintValidateResult(1, isTask1Correct);
                PrintValidateResult(2, isTask2Correct);
                Console.WriteLine();

                return isTask1Correct && isTask2Correct;
            }
            else
            {
                return false;
            }
        }

        private static void PrintValidateResult(int taskNumber, bool isCorrect)
        {
            ConsoleColor correctResultColor = ConsoleColor.Green;
            ConsoleColor wrongResultColor = ConsoleColor.Red;
            string taskResult = isCorrect ? OkStr : FailStr;
            Console.Write($"Task {taskNumber} ");

            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = isCorrect ? correctResultColor : wrongResultColor;
            Console.Write(taskResult);
            Console.ForegroundColor = prevColor;
        }

        private static int GetDayInWorkNumber(List<Day> daysTasks)
        {
            int day = 1;
            for (int dayIndex = 0; dayIndex < daysTasks.Count; dayIndex++)
            {
                int implementedNum = daysTasks[dayIndex].ImplementedNum();

                if (implementedNum == -1)
                {
                    if (dayIndex == 0)
                        return 1;
                    else
                    {
                        int prevDayIndex = dayIndex - 1;
                        day = prevDayIndex + 1;
                        break;
                    }
                }
                else if (implementedNum == 2)
                {
                    continue;
                }
                else
                {
                    day = dayIndex + 1;
                    break;
                }
            }

            return day;
        }

        private static void RunDayTask(List<Day> daysTasks, int taskIdx)
        {
            Day dayTask = daysTasks[taskIdx];
            string info = $"Day {taskIdx + 1}: {dayTask.GetName()}";
            Console.WriteLine(GetTopSeparator(info));

            if (!dayTask.HasAnyImplementation() && !dayTask.HasAnswer1() && !dayTask.HasAnswer2())
            {
                Console.WriteLine("Not implemented!");
            }
            else
            {
                using (new GCStatPrinterDisposable(PrintGCStat))
                {
                    var start = DateTime.Now;
                    dayTask.Run();
                    PrintElapsedTime(DateTime.Now - start);
                }

                Console.WriteLine(GetBottomSeparator());
            }
        }

        private static void PrintElapsedTime(TimeSpan timeSpan)
        {
            ConsoleColor timeColor = ConsoleColor.Green;

            if (timeSpan.TotalSeconds > 12)
            {
                timeColor = ConsoleColor.Red;
            }
            else if (timeSpan.TotalSeconds > 2)
            {
                timeColor = ConsoleColor.DarkYellow;
            }

            Console.Write("It took ");
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = timeColor;
            Console.Write($"{timeSpan.TotalSeconds}");
            Console.ForegroundColor = prevColor;
            Console.Write(" sec");
        }

        private static Command GetValidCommand(bool isAllImplemented)
        {
            bool validated;
            Command command = new Command();

            do
            {
                validated = false;
                string input = Console.ReadLine() ?? "";

                if (string.IsNullOrEmpty(input))
                {
                    command.ExecType = ExecutionType.Proceed;
                    validated = true;
                }
                else
                {
                    input = input.ToLower().Trim();
                    if (int.TryParse(input, out int day))
                    {
                        if (day > 0 && day <= 25)
                        {
                            command.ExecType = ExecutionType.DayNumber;
                            command.Value = day;
                            validated = true;
                        }
                    }
                    else if (input.Equals("all"))
                    {
                        command.ExecType = ExecutionType.AllTasks;
                        validated = true;
                    }
                }

                if (!validated)
                {
                    Console.WriteLine("Wrong input. Try again: ");
                }

            } while (!validated);

            if (command.ExecType == ExecutionType.Proceed)
            {
                command.ExecType = isAllImplemented ? ExecutionType.AllTasks : ExecutionType.CurrentDay;
            }

            return command;
        }

        private static string GetTopSeparator(string data)
        {
            data = $"> {data} <";
            string topSeparator = new string('=', (SeparatorLength - data.Length)/2);
            topSeparator += data;

            int currentLength = topSeparator.Length;
            for (int i = currentLength; i < SeparatorLength; i++)
            {
                topSeparator += "=";
            }

            return topSeparator;
        }

        private static string GetBottomSeparator() => new string('=', SeparatorLength);


        private static readonly string OkStr = "---OK---";
        private static readonly string FailStr = "--FAIL--";
        private static readonly int SeparatorLength = 50;
    }
}
