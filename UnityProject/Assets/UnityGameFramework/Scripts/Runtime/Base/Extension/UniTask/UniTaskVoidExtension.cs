using Cysharp.Threading.Tasks;

namespace UnityGameFramework.Runtime
{
    public static class UniTaskVoidExtension
    {
        public static void Coroutine(this UniTaskVoid task)
        {
            task.Forget();
        }
    }
}