namespace SurfaceApplicationTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Note
    {
        public int longueur
        {
            get;
            set;
        }

        public string name
        {
            get;
            set;
        }

        public Note(int longu, string nom)
        {
            longueur = longu;
            name = nom;
        }
    }
}

