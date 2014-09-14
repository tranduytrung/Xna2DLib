using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Dovahkiin.Constant
{
    public static class Sounds
    {
        private static AudioEngine _audioEngine;
        private static WaveBank _waveBank;
        private static SoundBank _soundBank;

        public static Cue GetBackgroundMusic()
        {
            return _soundBank.GetCue("background");
        }

        public static void ButtonHover()
        {
            if (GlobalConfig.SoundEnabled)
                _soundBank.PlayCue("button-hover");
        }

        public static void ButtonClick()
        {
            if (GlobalConfig.SoundEnabled)
                _soundBank.PlayCue("button-click");
        }

        public static void Initialize(ContentManager content)
        {
            _audioEngine = new AudioEngine(@"Content/sound/sound-engine.xgs");
            _waveBank = new WaveBank(_audioEngine, @"Content/sound/main-wave-bank.xwb");
            _soundBank = new SoundBank(_audioEngine, @"Content/sound/main-sound-bank.xsb");
        }
    }
}
