using GameFramework;

namespace GameMain
{
    public class DinLogHelper: GameFrameworkLog.ILogHelper
    {
        public void Log(GameFrameworkLogLevel level, object message)
        {
            switch (level)
            {
                case GameFrameworkLogLevel.Debug:
                    TLogger.LogInfo(Utility.Text.Format("<color=#888888>{0}</color>", message));
                    break;
                case GameFrameworkLogLevel.Info:
                    TLogger.LogInfo(message.ToString());
                    break;

                case GameFrameworkLogLevel.Warning:
                    TLogger.LogWarning(message.ToString());
                    break;

                case GameFrameworkLogLevel.Error:
                    TLogger.LogError(message.ToString());
                    break;
                case GameFrameworkLogLevel.Fatal:
                    TLogger.LogException(message.ToString());
                    break;
                default:
                    throw new GameFrameworkException(message.ToString());
            }
        }
    }
}