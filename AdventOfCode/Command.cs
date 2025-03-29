
namespace AdventOfCode
{
    enum ExecutionType
    {
        AllTasks,
        DayNumber,
        CurrentDay,
        Proceed
    }

    class Command
    {
        public ExecutionType ExecType;

        public int Value;
    }
}
