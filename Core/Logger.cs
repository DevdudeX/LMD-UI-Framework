using MelonLoader;


namespace LMDModMenu;

/// <summary>
/// Basic wrapper for the MelonLogger instance.
/// </summary>
internal class Logger
{
	private readonly MelonLogger.Instance _loggerInstance;

	public Logger(MelonLogger.Instance loggerInstance)
	{
		_loggerInstance = loggerInstance;
	}


	public void LogInfo(string msg)
	{
		_loggerInstance.Msg(msg);
	}

	/// <summary>
	/// Logs only if the condition is true.
	/// </summary>
	public void LogInfo(bool condition, string msg)
	{
		if (!condition) return;
		_loggerInstance.Msg(msg);
	}

	public void LogWarning(string msg)
	{
		_loggerInstance.Warning(msg);
	}

	/// <summary>
	/// Logs a warning only if the condition is true.
	/// </summary>
	public void LogWarning(bool condition, string msg)
	{
		if (!condition) return;
		_loggerInstance.Warning(msg);
	}

	public void LogError(string msg)
	{
		_loggerInstance.Error(msg);
	}

	/// <summary>
	/// Logs an error only if the condition is true.
	/// </summary>
	public void LogError(bool condition, string msg)
	{
		if (!condition) return;
		_loggerInstance.Error(msg);
	}
}
