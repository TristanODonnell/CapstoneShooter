using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Regular 60 second countdown
/// </summary>
[System.Serializable]
public class Countdown
{
	public Countdown(float CountTo)
	{
		maximumCount = CountTo;
		Countdown_Restart();
	}

	public float maximumCount;
	protected float countdown;

	public override string ToString()
	{
		return "" + (int)countdown + "/" + (int)maximumCount;
	}

	public virtual bool CountdownReturn()
	{
		if (countdown > 0)
		{
			countdown -= Time.deltaTime;
			return false;
		}
		countdown = 0;
		return true;
	}
	public virtual float CountdownReturnValue()
	{
		return countdown;
	}
	public virtual float CountdownUpwardsnValue()
	{
		return maximumCount- countdown;
	}

	public void CountdownResetRandomized(float normalOffset = 0f)
	{
		normalOffset = Mathf.Clamp01(normalOffset);
		var resetTime = Random.Range(normalOffset, 1f);
		countdown = maximumCount * (resetTime);
	}

	public float normalized => Normalized();
	float Normalized()
	{
		if (maximumCount <= 0f)
			return 1f;
		if (countdown <= Time.deltaTime*1.5f)
			return 1f;

		var div = countdown / maximumCount;
		var fix = 1f - div;
		return fix;
	}

	public virtual void Countdown_Restart()
	{
		countdown = maximumCount;
	}

	public void Countdown_ForceSeconds(float seconds)
	{
		countdown = seconds;
	}
	public bool mReviseCountdownIsOver => countdown <= 0;
}
[System.Serializable]

/// <summary>
/// Regular 60 second countdown which can have increased or decreased speed of count
/// </summary>
public class Countdown_Multiplier : Countdown
{
	public Countdown_Multiplier(float CountTo) : base(CountTo) { }

	public bool CountdownReturn(float multiplier)
	{
		if(countdown > 0)
		{
			countdown -= Time.deltaTime * multiplier;
			return false;
		}
		countdown = 0;
		return true;
	}

}

/// <summary>
/// An auto reset countdown that doesn't require to reset
/// </summary>
[System.Serializable]
public class Countdown_AutoReset : Countdown
{
	public Countdown_AutoReset(float CountTo) : base(CountTo) { }


	public override bool CountdownReturn()
	{
		if (base.CountdownReturn())
		{
			Countdown_Restart();
			return true;
		}
		return false;
	}
}

/// <summary>
/// A countdown that returns the ammount of intervals it has finnished
/// </summary>
[System.Serializable]
public class Countdown_Interval : Countdown_AutoReset
{
	public Countdown_Interval(float CountTo) : base(CountTo) { }


	int finishedIntervals;

	public override bool CountdownReturn()
	{
		if (base.CountdownReturn())
		{
			++finishedIntervals;
			return true;
		}
		return false;
	}

	public void Countdown_RestartEntirely()
	{
		Countdown_Restart();
		IntervalReset();
	}



	public int IntervalAmmount()
	{
		return finishedIntervals;
	}

	public void IntervalReset()
	{
		finishedIntervals = 0;
	}
}

/// <summary>
/// A countdown that can return its value as a 1 to 0 decimal
/// </summary>
[System.Serializable]
public class Countdown_Percentage : Countdown
{
	public Countdown_Percentage(float CountTo) : base(CountTo) { }


	public float CountdownReturnValuePercentage()
	{
		base.CountdownReturnValue();
		if (maximumCount == 0f)
		{
			return 1f;
		}
		return countdown / maximumCount;
	}
}



[System.Serializable]
public class Countdown_Minutes : Countdown
{
	public Countdown_Minutes(byte Minutes, float ExtraSeconds) : base(ExtraSeconds) { }
}

[System.Serializable]
public class Countdown_Hour : Countdown_Minutes
{
	public Countdown_Hour(byte Hours, byte Minutes, float Seconds) : base(Minutes, Seconds) { }
}


