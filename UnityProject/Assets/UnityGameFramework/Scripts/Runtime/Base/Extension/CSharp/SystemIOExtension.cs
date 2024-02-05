using System.IO;


public static class SystemIOExtension
{
    public static string CreateDirIfNotExists(this string dirFullPath)
    {
        if (!Directory.Exists(dirFullPath))
        {
            Directory.CreateDirectory(dirFullPath);
        }

        return dirFullPath;
    }

    public static void DeleteDirIfExists(this string dirFullPath)
    {
        if (Directory.Exists(dirFullPath))
        {
            Directory.Delete(dirFullPath, true);
        }
    }

    public static void EmptyDirIfExists(this string dirFullPath)
    {
        if (Directory.Exists(dirFullPath))
        {
            Directory.Delete(dirFullPath, true);
        }

        Directory.CreateDirectory(dirFullPath);
    }

    public static bool DeleteFileIfExists(this string fileFullPath)
    {
        if (File.Exists(fileFullPath))
        {
            File.Delete(fileFullPath);
            return true;
        }

        return false;
    }

    public static string CombinePath(this string selfPath, string toCombinePath)
    {
        return Path.Combine(selfPath, toCombinePath);
    }

    public static string GetFileName(this string filePath)
    {
        return Path.GetFileName(filePath);
    }

    public static string GetFileNameWithoutExtend(this string filePath)
    {
        return Path.GetFileNameWithoutExtension(filePath);
    }

    public static string GetFileExtendName(this string filePath)
    {
        return Path.GetExtension(filePath);
    }

    public static string GetFolderPath(this string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return string.Empty;
        }

        return Path.GetDirectoryName(path);
    }
}