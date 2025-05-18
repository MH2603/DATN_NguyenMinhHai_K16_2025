using MH.EventBus;

namespace MH.Sound.Event
{
    public class ChangeSoundVolumeEvent : IEventContext
    {
        public float Volume { get; private set; }
        
        public ChangeSoundVolumeEvent(float volume)
        {
            Volume = volume;
        }
    }
}