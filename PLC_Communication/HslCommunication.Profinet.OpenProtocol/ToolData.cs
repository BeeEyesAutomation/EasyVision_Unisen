using System;

namespace HslCommunication.Profinet.OpenProtocol;

public class ToolData
{
	public string ToolSerialNumber { get; set; }

	public uint ToolNumberOfTightening { get; set; }

	public DateTime LastCalibrationDate { get; set; }

	public string ControllerSerialNumber { get; set; }

	public double CalibrationValue { get; set; }

	public DateTime LastServiceDate { get; set; }

	public uint TighteningsSinceService { get; set; }

	public int ToolType { get; set; }

	public int MotorSize { get; set; }

	public bool UseOpenEnd { get; set; }

	public string TighteningDirection { get; set; }

	public int MotorRotation { get; set; }

	public string ControllerSoftwareVersion { get; set; }
}
