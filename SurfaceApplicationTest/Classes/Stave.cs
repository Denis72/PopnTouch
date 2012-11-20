namespace SurfaceApplicationTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Timers;
    using Microsoft.Xna.Framework.Audio;

    public class Stave : Music
    {
        //nombre de note maximum pour faire un accord
        private int NBNOTECHORD
        {
            get;
            set;
        }

        //nombre max de note dans la portee
        private int MAXNOTE
        {
            get;
            set;
        }

        private Timer timer
        {
            get;
            set;
        }

        private int count
        {
            get;
            set;
        }

        private int iterator
        {
            get;
            set;
        }

        private bool playNote
        {
            get;
            set;
        }

        private Note[] noteArray;

        public Stave(int nbNoteChord, int maxNote)
        {
            NBNOTECHORD = nbNoteChord;
            MAXNOTE = maxNote;
            timer = new Timer();
            iterator = 0;
            noteArray = new Note[MAXNOTE*NBNOTECHORD];
        }

        public virtual void initNoteArray()
        {
            for (int i = 0; i < MAXNOTE * NBNOTECHORD; i++)
                noteArray[i] = new Note(1, "silence");
        }

        public virtual void initPlay(int bpm)
        {
            this.playNote = true;
            this.timer.Interval = 30000 / bpm;
            timer.Start();
            this.timer.Elapsed += new ElapsedEventHandler(playMusic);
        }

        public virtual void initStop()
        {
            iterator = 0;
            timer.Stop();
            timer.Elapsed -= new ElapsedEventHandler(playMusic);
        }

        private void playMusic(object source, ElapsedEventArgs e)
        {
            if (playNote)
            {
                count = noteArray[iterator].longueur + 1;
                for (int i = 0; i < NBNOTECHORD; i++)
                    playSound(noteArray[iterator + i * MAXNOTE].name);
                //on lit les 2 notes sur le meme temps
            }

            count--;
            playNote = false;

            if (count == 1)
            {
                playNote = true;
                iterator++;
            }

            if (iterator == MAXNOTE - 1)
            {
                iterator = 0;
                timer.Stop();
                timer.EndInit();
                timer.Elapsed -= new ElapsedEventHandler(playMusic);
            }
        }

        public virtual void fillArray(int centreX, int centreY, int longu, String res)
        {
            bool limiteNote = true;

            int i = (centreX - 180) / 60;
            if (i >= 0 && i < MAXNOTE)
            {
                for (int j = 0; j < NBNOTECHORD && limiteNote; j++)
                {
                    if (noteArray[i + j * MAXNOTE].name == "silence")
                    {
                        noteArray[i + j * MAXNOTE] = new Note(longu, res);
                    
                        limiteNote = false;
                    }
                    //  if (limiteNote)
                    //   MessageBox.Show("Seulement 3 notes par accord maximum");
                }
            }

            if (timer.Enabled == false)
                playSound(res);
        }

    }
}