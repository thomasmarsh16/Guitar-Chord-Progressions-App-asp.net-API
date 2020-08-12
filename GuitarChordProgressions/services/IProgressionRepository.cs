using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuitarChordProgressions.services
{
    public interface IProgressionRepository
    {
        Task<List<ChordProgression>> GetProgressions(string[] genres, string[] keys);

        Task<ChordProgression> GetProgression(int progressionID);

        Task<ProgessionOption> GetProgessionOptions();

        void CreateProgression(ChordProgression progression);

        void SaveProgression(int progressionID, string userEmail);

        void DeleteSavedUserProgression(int progressionID, string userEmail);

        Task<List<ChordProgression>> GetSavedUserProgressions(string userEmail);

        void DeleteProgression(int progressionID);

        void EditProgression(ChordProgression progression);

        public Task<GuitarChord> GetChord(int chordID);

        public void DeleteChord(int chordID);

        public void EditChord(GuitarChord chord);

        public void CreateChord(GuitarChord chord);

        Task<List<GuitarChord>> GetAllChords();

        Task<List<GuitarChord>> GetProgressionChords(int progID);
    }
}
