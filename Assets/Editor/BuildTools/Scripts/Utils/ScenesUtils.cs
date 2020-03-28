using UnityEditor;

namespace Himeki.Build
{
    public static class ScenesUtils
    {
        public static string[] getDefaultScenesAsArray()
        {
            var scenes = EditorBuildSettings.scenes;

            var result = new string[scenes.Length];

            for (int i = 0; i < scenes.Length; i++)
            {
                result[i] = scenes[i].path;
            }

            return result;
        }

    }
}