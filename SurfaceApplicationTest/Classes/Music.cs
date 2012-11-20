namespace SurfaceApplicationTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Timers;
    using Microsoft.Xna.Framework.Audio;

    public class Music
    {

        private AudioEngine audioEngine;
        private WaveBank waveBank;
        private SoundBank soundBank;

        public Music()
        {
            String path = System.Environment.CurrentDirectory;
            path = path.Replace(@"\bin\Debug", @"\Resources");

            audioEngine = new AudioEngine(path + @"\sound.xgs");
            waveBank = new WaveBank(audioEngine, path + @"\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, path + @"\Sound Bank.xsb");
        }

        public void playSound(String sound)
        {
            soundBank.PlayCue(sound);
        }
    }
}

