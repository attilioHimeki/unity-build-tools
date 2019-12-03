using System.Collections.Generic;
using UnityEditor;

public static class VRUtils
{

    public static string[] getSelectedVRSdksFromFlags(BuildTargetGroup targetGroup, int flags)
    {
        var result = new List<string>();
        var vrSdks = PlayerSettings.GetAvailableVirtualRealitySDKs(targetGroup);
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

}