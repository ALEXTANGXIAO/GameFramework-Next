using GameFramework;
using GameFramework.DataTable;
using GameFramework.Sound;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public static class SoundExtension
    {
        private const float FadeVolumeDuration = 1f;
        private static int? s_MusicSerialId = null;

        // public static int? PlayMusic(this SoundComponent soundComponent, int musicId, object userData = null)
        // {
        //     soundComponent.StopMusic();
        //     
        //     IDataTable<DRMusic> dtMusic = GameModule.DataTable.GetDataTable<DRMusic>();
        //     DRMusic drMusic = dtMusic.GetDataRow(musicId);
        //     if (drMusic == null)
        //     {
        //         Log.Warning("Can not load music '{0}' from data table.", musicId.ToString());
        //         return null;
        //     }
        //     
        //     PlaySoundParams playSoundParams = PlaySoundParams.Create();
        //     playSoundParams.Priority = 64;
        //     playSoundParams.Loop = true;
        //     playSoundParams.VolumeInSoundGroup = 1f;
        //     playSoundParams.FadeInSeconds = FadeVolumeDuration;
        //     playSoundParams.SpatialBlend = 0f;
        //     s_MusicSerialId = soundComponent.PlaySound(AssetUtility.GetMusicAsset(drMusic.AssetName), "Music", Constant.AssetPriority.MusicAsset, playSoundParams, null, userData);
        //     return s_MusicSerialId;
        // }

        public static void StopMusic(this SoundComponent soundComponent)
        {
            if (!s_MusicSerialId.HasValue)
            {
                return;
            }

            soundComponent.StopSound(s_MusicSerialId.Value, FadeVolumeDuration);
            s_MusicSerialId = null;
        }

        public static int? PlaySound(this SoundComponent soundComponent, string assetName, Entity bindingEntity = null, object userData = null)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                Log.Warning("Can not load sound '{0}' from data table.", assetName.ToString());
                return null;
            }
            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            playSoundParams.Priority = 0;
            playSoundParams.Loop = false;
            playSoundParams.VolumeInSoundGroup = 1;
            playSoundParams.SpatialBlend = 1;
            
            string soundAssetName = assetName;
            return soundComponent.PlaySound(soundAssetName, "Sound", Constant.AssetPriority.SoundAsset, playSoundParams, bindingEntity != null ? bindingEntity : null, userData);
        }
        
        // public static int? PlayUISound(this SoundComponent soundComponent, int uiSoundId, object userData = null)
        // {
        //     IDataTable<DRUISound> dtUISound = GameModule.DataTable.GetDataTable<DRUISound>();
        //     DRUISound drUISound = dtUISound.GetDataRow(uiSoundId);
        //     if (drUISound == null)
        //     {
        //         Log.Warning("Can not load UI sound '{0}' from data table.", uiSoundId.ToString());
        //         return null;
        //     }
        //
        //     PlaySoundParams playSoundParams = PlaySoundParams.Create();
        //     playSoundParams.Priority = drUISound.Priority;
        //     playSoundParams.Loop = false;
        //     playSoundParams.VolumeInSoundGroup = drUISound.Volume;
        //     playSoundParams.SpatialBlend = 0f;
        //     return soundComponent.PlaySound(AssetUtility.GetUISoundAsset(drUISound.AssetName), "UISound", Constant.AssetPriority.UISoundAsset, playSoundParams, userData);
        // }
        
        public static bool IsMuted(this SoundComponent soundComponent, string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return true;
            }
        
            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return true;
            }
        
            return soundGroup.Mute;
        }

        public static void Mute(this SoundComponent soundComponent, string soundGroupName, bool mute)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return;
            }

            soundGroup.Mute = mute;

            GameModule.Setting.SetBool(Utility.Text.Format(Constant.Setting.SoundGroupMuted, soundGroupName), mute);
            GameModule.Setting.Save();
        }

        public static float GetVolume(this SoundComponent soundComponent, string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return 0f;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return 0f;
            }

            return soundGroup.Volume;
        }

        public static void SetVolume(this SoundComponent soundComponent, string soundGroupName, float volume)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return;
            }

            soundGroup.Volume = volume;

            GameModule.Setting.SetFloat(Utility.Text.Format(Constant.Setting.SoundGroupVolume, soundGroupName), volume);
            GameModule.Setting.Save();
        }
    }
}
