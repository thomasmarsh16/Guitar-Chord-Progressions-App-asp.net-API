using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace GuitarChordProgressions.services
{
    public class ProgressionRepository : IProgressionRepository
    {
        private readonly SqlConnection connection;

        public ProgressionRepository(IConfiguration configuration)
        {
            IConfigurationSection configurationSection = configuration.GetSection("AzureSql");

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = configurationSection.GetSection("DataSource").Value,
                UserID = configurationSection.GetSection("UID").Value,
                Password = configurationSection.GetSection("Pass").Value,
                InitialCatalog = configurationSection.GetSection("Catalog").Value,
                MultipleActiveResultSets = true
            };

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            
            this.connection = connection;

            // open connection to db
            this.connection.Open();
        }

        public async Task<List<ChordProgression>> GetProgressions( string [] genres, string [] keys )
        {
            List<ChordProgression> tempProgs = new List<ChordProgression>();

            // build order 
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * ");

            string keySelect = "From(SELECT * FROM dbo.Progressions WHERE Progressions.Notekey = ''";
            int keyNumber = 0;

            Dictionary<string, string> keyParams = new Dictionary<string, string>();

            foreach( string key in keys)
            {
                string keyParam = "@keyParam" + keyNumber;
                keySelect += " OR Progressions.NoteKey = " + keyParam;

                keyParams.Add(keyParam, key);
                keyNumber++;
            }

            keySelect += ") keyQuery";

            sb.Append(keySelect);

            string genreSelect = " WHERE keyQuery.Genre = ''";
            int genreNumber = 0;

            Dictionary<string, string> genreParams = new Dictionary<string, string>();

            foreach( string genre in genres)
            {
                string tempParam = "@genreParam" + genreNumber;
                genreSelect += " OR keyQuery.Genre = " + tempParam;

                genreParams.Add(tempParam, genre);
                genreNumber++;
            }

            genreSelect += ";";

            sb.Append(genreSelect);

            String sqlCommand = sb.ToString();


            using (SqlCommand command = new SqlCommand(sqlCommand, connection))
            {
                foreach(string paramKey in keyParams.Keys)
                {
                    command.Parameters.AddWithValue(paramKey, keyParams[paramKey]);
                }

                foreach(string paramGenre in genreParams.Keys)
                {
                    command.Parameters.AddWithValue(paramGenre, genreParams[paramGenre]);
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // read output
                    while (await reader.ReadAsync())
                    {
                        List<GuitarChord> tempList = await this.GetProgressionChords(reader.GetInt32(0));
                        // add progressions to progression array
                        tempProgs.Add(new ChordProgression( reader.GetInt32(0),
                                                            reader.GetString(1),
                                                            reader.GetString(2),
                                                            reader.GetString(3),
                                                            tempList.ToArray()
                                                          ));

                    }
                }
            }

            // return progressions

            return tempProgs;
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

        public async Task<List<GuitarChord>> GetProgressionChords( int progID )
        {
            List<GuitarChord> chordList = new List<GuitarChord>();

            // build order 
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT* FROM dbo.Chords AS c1 WHERE ChordID IN");
            sb.Append(" (SELECT ChordFID FROM dbo.ProgressionChords");
            sb.Append(" WHERE dbo.ProgressionChords.ProgressionFID = @progID");
            sb.Append(" ORDER BY dbo.ProgressionChords.ChordPosition ASC OFFSET 0 ROWS)");
            sb.Append(" ORDER BY ( SELECT ChordPosition FROM dbo.ProgressionChords AS pc1 WHERE c1.ChordID = pc1.ChordFID AND pc1.ProgressionFID = @progID);");

            String sql = sb.ToString();

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@progID", progID);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        string[] tempArray = reader.GetString(3).Split(",");
                        chordList.Add(new GuitarChord(reader.GetInt32(0),
                                                      reader.GetString(1),
                                                      reader.GetInt32(2),
                                               new int[] { Int32.Parse(tempArray[0]),
                                                                Int32.Parse(tempArray[1]),
                                                                Int32.Parse(tempArray[2]),
                                                                Int32.Parse(tempArray[3]),
                                                                Int32.Parse(tempArray[4]),
                                                                Int32.Parse(tempArray[5]) },
                                                    reader.GetBoolean(4),
                                                    reader.GetInt32(5)));
                    }

                    chordList.Add(chordList.FirstOrDefault());
                }
            }

            return chordList;
        }

        public async Task<List<GuitarChord>> GetAllChords()
        {
            List<GuitarChord> chordList = new List<GuitarChord>();

            // build order 
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM dbo.Chords");
            String sql = sb.ToString();

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        string[] tempArray = reader.GetString(3).Split(",");
                        chordList.Add(new GuitarChord(reader.GetInt32(0),
                                                      reader.GetString(1),
                                                      reader.GetInt32(2),
                                               new int[] { Int32.Parse(tempArray[0]),
                                                                Int32.Parse(tempArray[1]),
                                                                Int32.Parse(tempArray[2]),
                                                                Int32.Parse(tempArray[3]),
                                                                Int32.Parse(tempArray[4]),
                                                                Int32.Parse(tempArray[5]) },
                                                    reader.GetBoolean(4),
                                                    reader.GetInt32(5))) ;
                    }
                }
            }

            // return chords
            return chordList;
        }
    }
}