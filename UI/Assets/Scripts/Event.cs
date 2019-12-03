
public class EventName
{
	public const string LoadingBegin = "loadingbegin";
	public const string LoadingStep = "loadingstep";
	public const string LoadingEnd = "loadingend";
}

public class Event
{
	public string name;
}

public class EventSimpleInt : Event
{
	public int val;
}

public class EventSimpleFloat : Event
{
	public float val;
}

public class EventSimpleString : Event
{
	public string val;
}

