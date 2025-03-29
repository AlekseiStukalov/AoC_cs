namespace AdventOfCode
{
    class GCStatPrinterDisposable : IDisposable
    {
        bool PrintUsage;
        int Gen0;
        int Gen1;
        int Gen2;

        public GCStatPrinterDisposable(bool doPrint)
        {
            PrintUsage = doPrint;
            Gen0 = GC.CollectionCount(0);
            Gen1 = GC.CollectionCount(1);
            Gen2 = GC.CollectionCount(2);
        }

        public void Dispose()
        {
            if (PrintUsage)
            {
                int gen0Usage = GC.CollectionCount(0) - Gen0;
                int gen1Usage = GC.CollectionCount(1) - Gen1;
                int gen2Usage = GC.CollectionCount(2) - Gen2;

                Console.WriteLine($". (GC: {gen0Usage}, {gen1Usage}, {gen2Usage})");
            }
            else
            {
                Console.WriteLine();
            }
        }
    }
}
