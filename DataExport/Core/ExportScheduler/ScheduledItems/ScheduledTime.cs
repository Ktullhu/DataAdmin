using System;
using System.Collections;

namespace DataExport.Core.ExportScheduler.ScheduledItems
{
	public enum EventTimeBase
	{
		BySecond = 1,
		ByMinute = 2,
		Hourly = 3,
		Daily = 4,
		Weekly = 5,
		Monthly = 6,
	}

	/// <summary>
	/// This class represents a simple schedule.  It can represent a repeating event that occurs anywhere from every
	/// second to once a month.  It consists of an enumeration to mark the interval and an offset from that interval.
	/// For example new ScheduledTime(Hourly, new TimeSpan(0, 15, 0)) would represent an event that fired 15 minutes
	/// after the hour every hour.
	/// </summary>
	[Serializable]
	public class ScheduledTime : IScheduledItem
	{
        public EventTimeBase Base;
        public TimeSpan Offset;

		public ScheduledTime(EventTimeBase bbase, TimeSpan offset)
		{
			Base = bbase;
			Offset = offset;
		}

		/// <summary>
		/// intializes a simple scheduled time element from a pair of strings.  
		/// Here are the supported formats
		/// 
		/// BySecond - single integer representing the offset in ms
		/// ByMinute - A comma seperate list of integers representing the number of seconds and ms
		/// Hourly - A comma seperated list of integers representing the number of minutes, seconds and ms
		/// Daily - A time in hh:mm:ss AM/PM format
		/// Weekly - n, time where n represents an integer and time is a time in the Daily format
		/// Monthly - the same format as weekly.
		/// 
		/// </summary>
		/// <param name="StrBase">A string representing the base enumeration for the scheduled time</param>
		/// <param name="StrOffset">A string representing the offset for the time.</param>
		public ScheduledTime(string StrBase, string StrOffset)
		{
			//TODO:Create an IScheduled time factory method.
			Base = (EventTimeBase)Enum.Parse(typeof(EventTimeBase), StrBase,true);
			Init(StrOffset);
		}

		public int ArrayAccess(string[] Arr, int i)
		{
			if (i >= Arr.Length)
				return 0;
			return int.Parse(Arr[i]);
		}

		public void AddEventsInInterval(DateTime Begin, DateTime End, ArrayList List)
		{
			DateTime Next = NextRunTime(Begin, true);
			while (Next < End)
			{
				List.Add(Next);
				Next = IncInterval(Next);
			}
		}

		public DateTime NextRunTime(DateTime time, bool AllowExact)
		{
			DateTime NextRun = LastSyncForTime(time) + Offset;
			if (NextRun == time && AllowExact)
				return time;
			if (NextRun > time)
				return NextRun;
			return IncInterval(NextRun);
		}


		private DateTime LastSyncForTime(DateTime time)
		{
			switch (Base)
			{
				case EventTimeBase.BySecond:
					return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second);
				case EventTimeBase.ByMinute:
					return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0);
				case EventTimeBase.Hourly:
					return new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0);
				case EventTimeBase.Daily:
					return new DateTime(time.Year, time.Month, time.Day);
				case EventTimeBase.Weekly:
					return (new DateTime(time.Year, time.Month, time.Day)).AddDays(-(int)time.DayOfWeek);
				case EventTimeBase.Monthly:
					return new DateTime(time.Year, time.Month, 1);
			}
			throw new Exception("Invalid base specified for timer.");
		}
	 
		private DateTime IncInterval(DateTime Last)
		{
			switch (Base)
			{
				case EventTimeBase.BySecond:
					return Last.AddSeconds(1);
				case EventTimeBase.ByMinute:
					return Last.AddMinutes(1);
				case EventTimeBase.Hourly:
					return Last.AddHours(1);
				case EventTimeBase.Daily:
					return Last.AddDays(1);
				case EventTimeBase.Weekly:
					return Last.AddDays(7);
				case EventTimeBase.Monthly:
					return Last.AddMonths(1);
			}
			throw new Exception("Invalid base specified for timer.");
		}

		private void Init(string StrOffset)
		{
			switch (Base)
			{
				case EventTimeBase.BySecond:
					Offset = new TimeSpan(0, 0, 0, 0, int.Parse(StrOffset));
					break;
				case EventTimeBase.ByMinute:
					string[] ArrMinute = StrOffset.Split(',');
					Offset = new TimeSpan(0, 0, 0, ArrayAccess(ArrMinute, 0), ArrayAccess(ArrMinute, 1));
					break;
				case EventTimeBase.Hourly:
					string[] ArrHour = StrOffset.Split(',');
					Offset = new TimeSpan(0, 0, ArrayAccess(ArrHour, 0), ArrayAccess(ArrHour, 1), ArrayAccess(ArrHour, 2));
					break;
				case EventTimeBase.Daily:
					DateTime Daytime = DateTime.Parse(StrOffset);
					Offset = new TimeSpan(0, Daytime.Hour, Daytime.Minute, Daytime.Second, Daytime.Millisecond);
					break;
				case EventTimeBase.Weekly:
					string[] ArrWeek = StrOffset.Split(',');
					if (ArrWeek.Length != 2)
						throw new Exception("Weekly offset must be in the format n, time where n is the day of the week starting with 0 for sunday");
					DateTime WeekTime = DateTime.Parse(ArrWeek[1]);
					Offset = new TimeSpan(int.Parse(ArrWeek[0]), WeekTime.Hour, WeekTime.Minute, WeekTime.Second, WeekTime.Millisecond);
					break;
				case EventTimeBase.Monthly:
					string[] ArrMonth = StrOffset.Split(',');
					if (ArrMonth.Length != 2)
						throw new Exception("Monthly offset must be in the format n, time where n is the day of the month starting with 1 for the first day of the month.");
					DateTime MonthTime = DateTime.Parse(ArrMonth[1]);
					Offset = new TimeSpan(int.Parse(ArrMonth[0])-1, MonthTime.Hour, MonthTime.Minute, MonthTime.Second, MonthTime.Millisecond);
					break;
				default:
					throw new Exception("Invalid base specified for timer.");
			}
		}

	}
}