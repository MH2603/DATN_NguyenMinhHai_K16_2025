using MH.EventBus;

namespace MH.Sound.Event
{
    public class ChangeSoundVolumeEvent : IEventContext
    {
        public float SoundVolume { get; private set; }
        
        public ChangeSoundVolumeEvent(float soundVolume)
        {
            SoundVolume = soundVolume;
        }
    }
    
    public class ChangeMusicVolumeEvent : IEventContext
    {
        public float MusicVolume { get; private set; }
        
        public ChangeMusicVolumeEvent(float musicVolume)
        {
            MusicVolume = musicVolume;
        }
    }
}