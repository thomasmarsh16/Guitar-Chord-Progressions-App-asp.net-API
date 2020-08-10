﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuitarChordProgressions.services
{
    public interface IProgressionRepository
    {
        async Task<List<ChordProgression>> GetProgressions(string[] genres, string[] keys);

        ChordProgression GetProgression(int progressionID);

        ProgessionOption GetProgessionOptions();

        void CreateProgression(ChordProgression progression);

        void DeleteProgression(int progressionID);

        void EditProgression(ChordProgression progression);

        public GuitarChord GetChord(int chordID);

        public void DeleteChord(int chordID);

        public void EditChord(GuitarChord chord);

        public void CreateChord(GuitarChord chord);
    }
}