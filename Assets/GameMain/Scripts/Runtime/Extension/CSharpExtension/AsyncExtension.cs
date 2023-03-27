using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 异步拓展。
/// </summary>
public static class AsyncExtension 
{
    /// <summary>
    /// 获取TaskAwaiter。
    /// </summary>
    /// <param name="asyncOp">AsyncOperation</param>
    /// <returns>TaskAwaiter</returns>
    public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOp)
    {
        var tcs = new TaskCompletionSource<object>();
        asyncOp.completed += obj => { tcs.SetResult(null); };
        return ((Task)tcs.Task).GetAwaiter();
    }
}