using System.Numerics;

namespace Common.Helpers.DataStructures
{
    public class IntervalRange<T> where T : INumber<T>
    {
        public T Min;

        public T Max;

        public IntervalRange(T min, T max)
        {
            Min = min;
            Max = max;
        }

        public bool IsInRange(T val)
        {
            return val >= Min && val <= Max;
        }

        public override string ToString()
        {
            return $"{Min}-{Max}";
        }
    }

    public class GroupRange<T> where T : INumber<T>
    {
        public List<T> Items;

        public List<IntervalRange<T>> RangeItems;

        public GroupRange()
        {
            Items = new();
            RangeItems = new();
        }

        public bool Any()
        {
            return Items.Any() || RangeItems.Any();
        }

        public void AddValue(T val)
        {
            if (!IsInGroup(val))
            {
                Items.Add(val);
            }
        }

        public void AddRange(T min, T max) => AddRange(new IntervalRange<T>(min, max));
        public void AddRange(IntervalRange<T> newRange)
        {
            bool shouldAdd = true;
            List<IntervalRange<T>> intervalsToRemove = new();
            foreach (var existed in RangeItems)
            {
                if (existed.Max < newRange.Min || existed.Min > newRange.Max)
                {
                    continue;
                }
                else if (existed.Min >= newRange.Min && existed.Max <= newRange.Max)
                {
                    intervalsToRemove.Add(existed);
                }
                else if (existed.Min <= newRange.Min && existed.Max >= newRange.Max)
                {
                    shouldAdd = false;
                    break;
                }
                else if (existed.Min < newRange.Min && existed.Max < newRange.Max)
                {
                    existed.Max = GetDecrementedValue(newRange.Min);
                }
                else if (existed.Min > newRange.Min && existed.Max > newRange.Max)
                {
                    existed.Min = GetIncrementedValue(newRange.Max);
                }
                else
                {
                    Console.Write("E");
                }
            }

            if (!shouldAdd)
            {
                return;
            }

            DoRemoveIntervals(intervalsToRemove);

            List<T> itemsToRemove = GetSingleItemsInInterval(newRange);
            DoRemoveItems(itemsToRemove);

            RangeItems.Add(newRange);
        }

        public bool RemoveValue(T val)
        {
            if (Items.Remove(val))
            {
                return true;
            }
            else
            {
                IntervalRange<T>? interval = IsIntervalsContain(val);

                if (interval != null)
                {
                    if (val == interval.Min)
                    {
                        interval.Min = GetIncrementedValue(val);
                    }
                    else if (val == interval.Max)
                    {
                        interval.Max = GetDecrementedValue(val);
                    }
                    else
                    {
                        var rightRange = new IntervalRange<T>(GetIncrementedValue(val), interval.Max);
                        interval.Max = GetDecrementedValue(val);
                        RangeItems.Add(rightRange);
                    }
                    return true;
                }
            }

            return false;
        }

        public void RemoveRange(IntervalRange<T> toRemove)
        {
            List<T> itemsToRemove = GetSingleItemsInInterval(toRemove);
            DoRemoveItems(itemsToRemove);


            //todo

        }

        public void Clear()
        {
            Items.Clear();
            RangeItems.Clear();
        }

        public bool IsInGroup(T val)
        {
            if (Items.Contains(val))
            {
                return true;
            }

            return IsIntervalsContain(val) != null;
        }

        public void ConcatAndOptimise()
        {
            ReplaceUnitIntervals();

            ConcatSingleItems();

            ConcatItemsAndIntervals();

            ConcatIntervals();
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in Items)
            {
                yield return item;
            }

            foreach (IntervalRange<T> range in RangeItems)
            {
                for (T i = range.Min; i <= range.Max; i++)
                {
                    yield return i;
                }
            }
        }

        public override string ToString()
        {
            string itemsStr = string.Empty;
            for (int i = 0; i < Items.Count; i++)
            {
                itemsStr += $"{Items[i]}";
                if (i != Items.Count - 1)
                {
                    itemsStr += "|";
                }
            }

            string rangesStr = string.Empty;
            for (int i = 0; i < RangeItems.Count; i++)
            {
                rangesStr += $"{RangeItems[i]}";
                if (i != RangeItems.Count - 1)
                {
                    rangesStr += "|";
                }
            }

            bool needSeparator = itemsStr.Length > 0 && rangesStr.Length > 0;

            return $"{itemsStr}{(needSeparator ? ";" : "")}{rangesStr}";
        }


        //=========== Private Section ============

        private void ConcatIntervals()
        {
            List<IntervalRange<T>> intervalsToRemove = new();
            foreach (var interval in RangeItems)
            {
                T nextItem = GetIncrementedValue(interval.Max);

                var nextRange = IsIntervalsContain(nextItem);
                if (nextRange != null)
                {
                    intervalsToRemove.Add(interval);
                    nextRange.Min = interval.Min;
                }

                T prevItem = GetDecrementedValue(interval.Min);

                var prevRange = IsIntervalsContain(prevItem);
                if (prevRange != null)
                {
                    intervalsToRemove.Add(interval);
                    prevRange.Max = interval.Max;
                }
            }

            DoRemoveIntervals(intervalsToRemove);
        }

        private void ConcatItemsAndIntervals()
        {
            List<T> itemsToRemove = new();

            bool bWasChange;
            do
            {
                foreach (T item in Items)
                {
                    T greater = GetIncrementedValue(item);
                    T lower = GetDecrementedValue(item);

                    foreach (var interval in RangeItems)
                    {
                        if (interval.IsInRange(greater))
                        {
                            interval.Min = item;
                            itemsToRemove.Add(item);
                            break;
                        }
                        else if (interval.IsInRange(lower))
                        {
                            interval.Max = item;
                            itemsToRemove.Add(item);
                            break;
                        }
                    }
                }

                bWasChange = DoRemoveItems(itemsToRemove);
            }
            while (bWasChange);
        }

        private void ConcatSingleItems()
        {
            List<T> itemsToRemove = new();

            bool bWasChange;
            do
            {
                foreach (T item in Items)
                {
                    T greater = GetIncrementedValue(item);
                    T lower = GetDecrementedValue(item);

                    bool hasGreater = Items.Contains(greater);
                    bool hasLower = Items.Contains(lower);

                    if (hasGreater || hasLower)
                    {
                        IntervalRange<T> newInterval = new IntervalRange<T>(item, item);
                        itemsToRemove.Add(item);

                        if (hasGreater)
                        {
                            newInterval.Max = greater;
                            itemsToRemove.Add(greater);
                        }

                        if (hasLower)
                        {
                            newInterval.Min = lower;
                            itemsToRemove.Add(lower);
                        }

                        RangeItems.Add(newInterval);
                        break;
                    }
                }

                bWasChange = DoRemoveItems(itemsToRemove);
            }
            while (bWasChange);
        }

        private bool ReplaceUnitIntervals()
        {
            List<IntervalRange<T>> intervalsToRemove = new();
            foreach(IntervalRange<T> range in RangeItems)
            {
                if (range.Min == range.Max)
                {
                    intervalsToRemove.Add(range);
                    Items.Add(range.Min);
                }
            }

            return DoRemoveIntervals(intervalsToRemove);
        }

        private List<T> GetSingleItemsInInterval(IntervalRange<T> interval)
        {
            List<T> singleItems = new();

            foreach (T item in Items)
            {
                if (interval.IsInRange(item))
                {
                    singleItems.Add(item);
                }
            }

            return singleItems;
        }

        private bool DoRemoveItems(List<T> items)
        {
            bool wasChange = false;
            foreach(T item in items)
            {
                if (Items.Remove(item))
                {
                    wasChange = true;
                }
            }

            return wasChange;
        }

        private bool DoRemoveIntervals(List<IntervalRange<T>> intervalsToRemove)
        {
            bool wasChange = false;
            foreach (IntervalRange<T> interval in intervalsToRemove)
            {
                if (RangeItems.Remove(interval))
                {
                    wasChange = true;
                }
            }

            return wasChange;
        }

        private IntervalRange<T>? IsIntervalsContain(T item)
        {
            foreach(IntervalRange<T> range in RangeItems)
            {
                if (range.IsInRange(item))
                {
                    return range;
                }
            }

            return null;
        }

        private T GetIncrementedValue(T item)
        {
            T tmp = item;
            return ++tmp;
        }

        private T GetDecrementedValue(T item)
        {
            T tmp = item;
            return --tmp;
        }
    }
}