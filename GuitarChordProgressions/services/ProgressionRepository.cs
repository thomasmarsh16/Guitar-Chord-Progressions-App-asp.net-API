using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text;

namespace GuitarChordProgressions.services
{
    public class ProgressionRepository : IProgressionRepository
    {
        private readonly SqlConnection connection;

        public ProgressionRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<List<ChordProgression>> GetProgressions( string [] genres, string [] keys )
        {
            List<ChordProgression> tempProg = new List<ChordProgression>();

            // open connection to db
            this.connection.Open();

            // build order 
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM dbo.Chords");
            String sqlCommand = sb.ToString();

            // execute order
            using (SqlDataReader requestReader = MakeDatabaseRequest(sqlCommand) )
            {
                // read output
                while (await requestReader.ReadAsync())
                {
                    // add progressions to progression array
                }
            }
            
            // return progressions

            return tempProg;
        }

        public ChordProgression GetProgression(int progressionID)
        {
            ChordProgression tempProg = new ChordProgression(0, "none", "none", "no,structure", new GuitarChord[] { });

            return tempProg;
        }

        public ProgessionOption GetProgessionOptions()
        {
            ProgessionOption options = new ProgessionOption();

            return options;
        }

        public void CreateProgression( ChordProgression progression )
        {

        }

        public void DeleteProgression( int progressionID )
        {

        }

        public void EditProgression(ChordProgression progression)
        {

        }

        public GuitarChord GetChord(int chordID)
        {
            GuitarChord tempChord = new GuitarChord(1, "none", 0, new int[] { -1, -1, -1, -1, -1, -1 }, false, 1);

            return tempChord;
        }

        public void DeleteChord(int chordID)
        {

        }

        public void EditChord(GuitarChord chord)
        {

        }

        public void CreateChord(GuitarChord chord)
        {

        }

        private SqlDataReader MakeDatabaseRequest(string sqlCommand)
        {
            using (SqlCommand command = new SqlCommand(sqlCommand, this.connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader;
                }
            }
        }
    }
}
