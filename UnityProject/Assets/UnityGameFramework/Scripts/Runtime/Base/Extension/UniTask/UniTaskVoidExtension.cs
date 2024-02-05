using Cysharp.Threading.Tasks;

public static class UniTaskVoidExtension
{
    public static void Coroutine(this UniTaskVoid task)
    {
        task.Forget();
    }
}