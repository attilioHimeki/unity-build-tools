using System.Collections.Generic;
using UnityEditor;

namespace Himeki.Build
{
    public static class VRUtils
    {
        public static string[] getSelectedVRSdksFromFlags(BuildTargetGroup targetGroup, int flags)
        {
            var result = new List<string>();

            #if UNITY_2017_2_OR_NEWER
            var vrSdks = PlayerSettings.GetAvailableVirtualRealitySDKs(targetGroup);
            for (int i = 0; i < vrSdks.Length; i++)
            {
                int layer = 1 << i;
                if ((flags & layer) != 0)
                {
                    result.Add(vrSdks[i]);
                }
            }
            #endif
            
            return result.ToArray();
        }

        public static bool targetGroupSupportsVirtualReality(BuildTargetGroup targetGroup)
        {
            #if UNITY_2017_2_OR_NEWER
            var vrSdks = PlayerSettings.GetAvailableVirtualRealitySDKs(targetGroup);
            return vrSdks.Length > 0;
            #else
            return false;
            #endif
        }

    }
}