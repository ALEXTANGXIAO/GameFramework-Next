using System;


public static class SystemObjectExtension
{
    public static T Self<T>(this T self, Action<T> onDo)
    {
        onDo?.Invoke(self);
        return self;
    }

    public static T Self<T>(this T self, Func<T, T> onDo)
    {
        return onDo.Invoke(self);
    }

    public static bool IsNull<T>(this T selfObj) where T : class
    {
        return null == selfObj;
    }

    public static bool IsNotNull<T>(this T selfObj) where T : class
    {
        return null != selfObj;
    }

    public static T As<T>(this object selfObj) where T : class
    {
        return selfObj as T;
    }
}