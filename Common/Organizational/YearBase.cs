namespace Common.Organizational
{
	public abstract class YearBase
    {
        public bool IsCompleted;
        public YearBase(bool isCompleted)
        {
            IsCompleted = isCompleted;
            DaysTasks = new();
        }

        protected AnswersBase? AllAnswers;
        protected List<Day> DaysTasks;

        public abstract DateTime GetStartDate();
		public abstract List<Day> GetDaysTasks();
		public abstract AnswersBase GetAnswers();
    }
}

