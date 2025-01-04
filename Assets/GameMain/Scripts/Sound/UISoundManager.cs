using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class UISoundManager 
{
    public const string SoundGroupName = "UISound";
    private static Dictionary<string,string> _commonSoundFullNameCache = new Dictionary<string,string>();
    public static int PlaySound(string soundName)
    {
        if (string.IsNullOrEmpty(soundName))
        {
            GameFramework.GameFrameworkLog.Warning("soundName is null£¡");
            return -1;
        }

        return GameEntry.Sound.PlaySound(GetCommonSoundFullName(soundName), SoundGroupName);
    }
    public static void StopSound(int soundId)
    {
        GameEntry.Sound.StopSound(soundId);
    }

    public static string GetCommonSoundFullName(string soundName)
    {
        if (string.IsNullOrEmpty(soundName))
        {
            return string.Empty;
        }

        if (_commonSoundFullNameCache.ContainsKey(soundName))
        {
            return _commonSoundFullNameCache[soundName];
        }

        string fullName = GameFramework.Utility.Text.Format("Assets/GameMain/Sounds/Common/UI/{0}.mp3", soundName);
        _commonSoundFullNameCache[soundName] = fullName;
        return fullName;
    }

}
