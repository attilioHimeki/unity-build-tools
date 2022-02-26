using System.Collections.Generic;
using UnityEditor;

namespace Himeki.Build
{
    public static class VRUtils
    {

        public static string[] getAvailableVRSdks(BuildTargetGroup targetGroup)
        {
#if UNITY_2017_2_OR_NEWER && !UNITY_2019_3_OR_NEWER
            return PlayerSettings.GetAvailableVirtualRealitySDKs(targetGroup);
#elif UNITY_2019_3_OR_NEWER
            return UnityEngine.XR.XRSettings.supportedDevices;
#else
            return new string[0];
#endif
        }

        public static string[] getSelectedVRSdksFromFlags(BuildTargetGroup targetGroup, int flags)
        {
            var result = new List<string>();

            var vrSdks = getAvailableVRSdks(targetGroup);
            for (int i = 0; i < vrSdks.Length; i++)
            {
                int layer = 1 << i;
                if ((flags & layer) != 0)
                {
                    result.Add(vrSdks[i]);
                }
            }

            return result.ToArray();
        }

        public static bool targetGroupSupportsVirtualReality(BuildTargetGroup targetGroup)
        {
            var vrSdks = getAvailableVRSdks(targetGroup);
            return vrSdks.Length > 0;
        }

    }
}