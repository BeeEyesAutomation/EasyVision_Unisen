namespace HslCommunication.Algorithms.PID;

public class PIDHelper
{
	private double prakp;

	private double praki;

	private double prakd;

	private double prvalue;

	private double err;

	private double err_last;

	private double err_next;

	private double setValue;

	private double deadband;

	private double MAXLIM;

	private double MINLIM;

	private int index;

	private int UMAX;

	private int UMIN;

	public double Kp
	{
		get
		{
			return prakp;
		}
		set
		{
			prakp = value;
		}
	}

	public double Ki
	{
		get
		{
			return praki;
		}
		set
		{
			praki = value;
		}
	}

	public double Kd
	{
		get
		{
			return prakd;
		}
		set
		{
			prakd = value;
		}
	}

	public double DeadBand
	{
		get
		{
			return deadband;
		}
		set
		{
			deadband = value;
		}
	}

	public double MaxLimit
	{
		get
		{
			return MAXLIM;
		}
		set
		{
			MAXLIM = value;
		}
	}

	public double MinLimit
	{
		get
		{
			return MINLIM;
		}
		set
		{
			MINLIM = value;
		}
	}

	public double SetValue
	{
		get
		{
			return setValue;
		}
		set
		{
			setValue = value;
		}
	}

	public PIDHelper()
	{
		PidInit();
	}

	private void PidInit()
	{
		prakp = 0.0;
		praki = 0.0;
		prakd = 0.0;
		prvalue = 0.0;
		err = 0.0;
		err_last = 0.0;
		err_next = 0.0;
		MAXLIM = double.MaxValue;
		MINLIM = double.MinValue;
		UMAX = 310;
		UMIN = -100;
		deadband = 2.0;
	}

	public double PidCalculate()
	{
		err_next = err_last;
		err_last = err;
		err = SetValue - prvalue;
		prvalue += prakp * (err - err_last + praki * err + prakd * (err - 2.0 * err_last + err_next));
		if (prvalue > MAXLIM)
		{
			prvalue = MAXLIM;
		}
		if (prvalue < MINLIM)
		{
			prvalue = MINLIM;
		}
		return prvalue;
	}
}
