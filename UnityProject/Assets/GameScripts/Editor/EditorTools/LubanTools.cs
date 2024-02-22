using UnityEditor;
using UnityEngine;

namespace GameScripts.Editor
{
    public static class LubanTools
    {
        [MenuItem("Game Framework/Tools/Luban 转表")]
        public static void BuildLubanExcel()
        {
            Application.OpenURL(Application.dataPath + @"/../../Configs/GameConfig/gen_code_bin_to_project_lazyload.bat");
        }
    }
}