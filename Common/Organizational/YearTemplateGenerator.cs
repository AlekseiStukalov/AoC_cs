
namespace Common.Organizational
{

/*
1) Execute
3) Update AdventOfCode.sln (project declaration, global section)
2) Update AdventOfCode/AdventOfCode.csproj
4) End up Y2024/Year2024.cs
5) Update AdventOfCode/Program.cs
*/

	public class YearTemplateGenerator
	{
		public YearTemplateGenerator()
		{
		}

		public static void GenerateFilesForYear(int year)
		{
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }

            if (directory == null)
            {
                Console.WriteLine("Can't find solution directory");
                return;
            }

			string solutionDir = directory.FullName;

			string newYearProjectName = $"Y{year}";
			string projectDir = Path.Combine(solutionDir, newYearProjectName);
			if (Directory.Exists(projectDir))
            {
                string projectFilePath = Path.Combine(projectDir, $"Y{year}.csproj");
                if (SafeCreateFile(projectDir, projectFilePath))
                {
                    File.WriteAllLines(projectFilePath, GetProjFileContent(year));
                }

                string answersPath = Path.Combine(projectDir, $"Answers{year}.cs");
                if (SafeCreateFile(projectDir, answersPath))
                {
                    File.WriteAllLines(answersPath, GetAnswersContent(year));
                }

                string yearPath = Path.Combine(projectDir, $"Year{year}.cs");
                if (SafeCreateFile(projectDir, yearPath))
                {
                    File.WriteAllLines(yearPath, GetYearContent(year));
                }

                string decateDirTemplatePath = Path.Combine(projectDir, "Decade");
				for (int decade = 1; decade <= 3; decade++)
				{
                    string decateDirPath = decateDirTemplatePath + decade;
					Directory.CreateDirectory(decateDirPath);

                    int daysInDecade = decade == 3 ? 5 : 10;
                    for (int day = 1; day <= daysInDecade; day++)
                    {
						int dayNumber = (decade - 1) * 10 + day;
                        string dayNumberStr = $"{(dayNumber < 10 ? "0" : "")}{dayNumber}";

                        string dayDirPath = Path.Combine(decateDirPath, $"Day{dayNumberStr}");
                        Directory.CreateDirectory(dayDirPath);

                        SafeCreateFile(dayDirPath, $"Input{dayNumberStr}.txt");

                        string dayClassPath = Path.Combine(dayDirPath, $"Day{dayNumberStr}.cs");
                        if (SafeCreateFile(dayClassPath))
                        {
                            File.WriteAllLines(dayClassPath, GetDayContent(year, decade, dayNumberStr));
                        }

                    }
                }
                Console.WriteLine("Done!");
            }
            else
            {
                Console.WriteLine("Fail");
            }

            Console.WriteLine("Press smth to exit");
            Console.ReadKey(true);
        }

        private static bool SafeCreateFile(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                Console.WriteLine($"E: Can't create file {Path.GetFileName(fullPath)}. Already exist");
                return false;
            }
            else
            {
                using (File.Create(fullPath)) { }
                return true;
            }
        }

        private static bool SafeCreateFile(string directory, string filename)
        {
            return SafeCreateFile(Path.Combine(directory, filename));
        }

        private static string[] GetDayContent(int year, int decade, string dayNumber)
        {
            return new string[]
            {
                "using Common.Organizational;",
                "",
                $"namespace Y{year}.Decade{decade}.Day{dayNumber}",
                "{",
                $"    class Day{dayNumber} : Day",
                "    {",
                "        public override int ImplementedNum()",
                "        {",
                "            return base.ImplementedNum();",
                "        }",
                "",
                "        public override string GetName()",
                "        {",
                "            return \"\";",
                "        }",
                "",
                "        public override void Run()",
                "        {",
                $"            string[] input = File.ReadAllLines(GetInputPath(this));",
                "",
                "        }",
                "    }",
                "}",
                "",
                "/*",
                "",
                "",
                "",
                "*/",
                "",
            };
        }

        private static string[] GetAnswersContent(int year)
        {
            return new string[]
            {
                "using Common.Organizational;",
                "",
                $"namespace AdventOfCode.Y{year}",
                "{",
                $"    public class Answers{year} : AnswersBase",
                "    {",
                "        protected override void Initialize()",
                "        {",
                "            AllAnswers = new Dictionary<int, Dictionary<int, string>>",
                "            {",
                "                { 1,  CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 2,  CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 3,  CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 4,  CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 5,  CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 6,  CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 7,  CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 8,  CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 9,  CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 10, CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 11, CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 12, CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 13, CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 14, CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 15, CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 16, CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 17, CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 18, CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 19, CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 20, CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 21, CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 22, CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 23, CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 24, CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "                { 25, CreateAnswer(t1: \"todo\", t2: \"todo\") },",
                "            };",
                "        }",
                "    }",
                "}",
                ""
            };
        }

        private static string[] GetYearContent(int year)
        {
            return new string[]
            {
                "using Common.Organizational;",
                $"using Y{year}.Decade1.Day01;",
                $"using Y{year}.Decade1.Day02;",
                $"using Y{year}.Decade1.Day03;",
                $"using Y{year}.Decade1.Day04;",
                $"using Y{year}.Decade1.Day05;",
                $"using Y{year}.Decade1.Day06;",
                $"using Y{year}.Decade1.Day07;",
                $"using Y{year}.Decade1.Day08;",
                $"using Y{year}.Decade1.Day09;",
                $"using Y{year}.Decade1.Day10;",
                $"using Y{year}.Decade2.Day11;",
                $"using Y{year}.Decade2.Day12;",
                $"using Y{year}.Decade2.Day13;",
                $"using Y{year}.Decade2.Day14;",
                $"using Y{year}.Decade2.Day15;",
                $"using Y{year}.Decade2.Day16;",
                $"using Y{year}.Decade2.Day17;",
                $"using Y{year}.Decade2.Day18;",
                $"using Y{year}.Decade2.Day19;",
                $"using Y{year}.Decade2.Day20;",
                $"using Y{year}.Decade3.Day21;",
                $"using Y{year}.Decade3.Day22;",
                $"using Y{year}.Decade3.Day23;",
                $"using Y{year}.Decade3.Day24;",
                $"using Y{year}.Decade3.Day25;",
                $"",
                $"namespace AdventOfCode.Y{year}",
                "{",
                $"    public class Year{year} : YearBase",
                "    {",
                $"        public Year{year}(bool isCompleted) : base(isCompleted)",
                "        {",
                "            DaysTasks = new List<Day>",
                "            {",
                "                new Day01(),",
                "                new Day02(),",
                "                new Day03(),",
                "                new Day04(),",
                "                new Day05(),",
                "                new Day06(),",
                "                new Day07(),",
                "                new Day08(),",
                "                new Day09(),",
                "                new Day10(),",
                "                ",
                "                new Day11(),",
                "                new Day12(),",
                "                new Day13(),",
                "                new Day14(),",
                "                new Day15(),",
                "                new Day16(),",
                "                new Day17(),",
                "                new Day18(),",
                "                new Day19(),",
                "                new Day20(),",
                "                ",
                "                new Day21(),",
                "                new Day22(),",
                "                new Day23(),",
                "                new Day24(),",
                "                new Day25()",
                "            }",
                "        }",
                "",
                "        public override DateTime GetStartDate()",
                "        {",
                $"            return new DateTime({year}, 12, 1, 6, 0, 1);  // UTC +1",
                "        }",
                "        ",
                "        public override List<Day> GetDaysTasks() => DaysTasks;",
                "        ",
                "        public override AnswersBase GetAnswers()",
                "        {",
                "            if (AllAnswers == null)",
                "            {",
                $"                AllAnswers = new Answers{year}();",
                "            }",
                "",
                "            return AllAnswers;",
                "        }",
                "    }",
                "}",
                "",
            };
        }

        private static string[] GetProjFileContent(int year)
        {
            return new string[]
            {
                "<Project Sdk=\"Microsoft.NET.Sdk\">",
                "",
                "  <PropertyGroup>",
                "    <TargetFramework>net7.0</TargetFramework>",
                "    <ImplicitUsings>enable</ImplicitUsings>",
                "    <Nullable>enable</Nullable>",
                "  </PropertyGroup>",
                "",
                "  <ItemGroup>",
                "    <ProjectReference Include=\\\"..\\Common\\Common.csproj\" />",
                "  </ItemGroup>",
                "</Project>"
            };
        }
    }
}

