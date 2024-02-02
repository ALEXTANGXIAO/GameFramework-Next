using UnityEditor;

public static class SettingsMenu
{
    [MenuItem("Game Framework/Settings/Game FrameworkSettings", priority = 100)]
    public static void OpenSettings() => SettingsService.OpenProjectSettings("GameFramework/GameFrameworkSettings");
}