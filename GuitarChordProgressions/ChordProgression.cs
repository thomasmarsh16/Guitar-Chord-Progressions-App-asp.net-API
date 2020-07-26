using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuitarChordProgressions
{
    public class ChordProgression
    {
        public ChordProgression( string genre, string key, string progressionStructure, GuitarChord [] chords )
        {
            this.Genre = genre;
            this.Key = key;
            this.ProgressionStructure = progressionStructure;
            this.Chords = chords;
        }
        public string Genre { get; set; }
        public string Key { get; set; }
        public string ProgressionStructure { get; set; }
        public GuitarChord [] Chords { get; set; }
    }

    public class GuitarChord
    {
        public GuitarChord( string note, int baseFret, int [] fingerPlacement, bool barre, int barreStart )
        {
            this.Note = note;
            this.BaseFret = baseFret;
            this.FingerPlacements = fingerPlacement;
            this.Barre = barre;
            this.BarreStart = barreStart;
        }

        public string Note { get; set; }
        public int BaseFret { get; set; }
        public int[] FingerPlacements { get; set; }
        public bool Barre { get; set; }
        public int BarreStart { get; set; }
    }
}
