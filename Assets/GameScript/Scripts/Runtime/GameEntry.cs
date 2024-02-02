using UnityEngine;

/// <summary>
/// 游戏入口。
/// </summary>
public partial class GameEntry : MonoBehaviour
{
    private void Awake()
    {
        UnityEngine.Object.DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GameModule.Instance.Active();
    }
}