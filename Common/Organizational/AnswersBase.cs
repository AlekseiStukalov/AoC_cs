namespace Common.Organizational
{
	public abstract class AnswersBase
    {
        protected Dictionary<int, Dictionary<int, string>>? AllAnswers;

        public AnswersBase()
        {
            Initialize();
        }
        protected abstract void Initialize();

        protected Dictionary<int, string> CreateAnswer(string t1, string t2)
        {
            return new Dictionary<int, string>() { { 1, t1 }, { 2, t2 } };
        }

        public string Get(int day, int task)
        {
            if (AllAnswers != null)
            {
                return AllAnswers[day][task];
            }

            return string.Empty;
        }
	}
}

