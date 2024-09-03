namespace BlazorWasmWdcApp
{
	public class Project
	{
		public string title { get; set; }
		public TimeValue scheduled_start_time { get; set; }
		public TimeValue scheduled_end_time { get; set; }
		public TimeValue actual_start_time { get; set; }
	}

	public class TimeValue
	{
		public string value { get; set; }
	}
}
