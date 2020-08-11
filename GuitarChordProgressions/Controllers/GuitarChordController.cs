using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.Text;
using GuitarChordProgressions.services;

namespace GuitarChordProgressions.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChordProgressionsController : ControllerBase
    {

        private readonly ILogger<ChordProgressionsController> _logger;
        private readonly IProgressionRepository _progressionRepos;

        public ChordProgressionsController(ILogger<ChordProgressionsController> logger, IProgressionRepository progressionService )
        {
            _logger = logger;
            _progressionRepos = progressionService;
        }

        [EnableCors("GuitarAngularApp")]
        [HttpGet("chords")]
        public async Task<IEnumerable<ChordProgression>> GetChords([FromQuery] string[] genre, [FromQuery] string[] key)
        {
            List<ChordProgression> tempList = new List<ChordProgression>();

            if ( key.Contains("A"))
            {
                tempList.Add(new ChordProgression(0,"Rock", "A", "I-IV-V-I", new GuitarChord[] { new GuitarChord("A min", 1, new int[] { -1, 0, 2, 2, 1, 0 }, false, 0),
                                                                                              new GuitarChord("C maj", 1, new int[] { -1, 3, 2, 0, 1, 0 }, false, 0),
                                                                                              new GuitarChord("D maj", 1, new int[] { -1, -1, 0, 2, 3, 2 }, false, 0),
                                                                                              new GuitarChord("A min", 1, new int[] { -1, 0, 2, 2, 1, 0 }, false, 0)
                }));
            }

            if (genre.Contains("Jazz"))
            {
                tempList.Add(new ChordProgression(1,"Rock", "C", "I-IV-V-I", new GuitarChord[] { new GuitarChord("C maj", 1, new int[] { -1, 3, 2, 0, 1, 0 }, false, 0),
                                                                                              new GuitarChord("D maj", 1, new int[] { -1, -1, 0, 2, 3, 2 }, false, 0),
                                                                                              new GuitarChord("E maj", 1, new int[] {  0, 2, 2, 1, 0, 0 }, false, 0),
                                                                                              new GuitarChord("C maj", 1, new int[] { -1, 3, 2, 0, 1, 0 }, false, 0)
                }));
                tempList.Add(new ChordProgression(2,"Jazz", "C", "I-IV-V-I", new GuitarChord[] { new GuitarChord("C maj", 1, new int[] { -1, 3, 2, 0, 1, 0 }, false, 0),
                                                                                              new GuitarChord("D maj", 1, new int[] { -1, -1, 0, 2, 3, 2 }, false, 0),
                                                                                              new GuitarChord("E maj", 1, new int[] {  0, 2, 2, 1, 0, 0 }, false, 0),
                                                                                              new GuitarChord("C maj", 1, new int[] { -1, 3, 2, 0, 1, 0 }, false, 0)
                }));
            }

            return tempList.ToArray();

            // https://localhost:44377/ChordProgressions/chords?genre=blank&key=blank
        }

        [EnableCors("GuitarAngularApp")]
        [HttpGet("options")]
        public async Task<ActionResult<ProgessionOption>> GetOptions( )
        {
            ProgessionOption options = new ProgessionOption();

            options.Genres = new string[] { "Jazz", "Rock", "Latin" };
            options.Keys = new string[] { "A", "B", "C" };

            if (options == null)
            {
                return NotFound();
            }

            return options;

            // https://localhost:44377/ChordProgressions/options
        }

        [EnableCors("GuitarAngularApp")]
        [HttpGet("testSql")]
        public async Task<IEnumerable<GuitarChord>> GetSqlAsync()
        {

            List<GuitarChord> tempList = await this._progressionRepos.GetProgressionChords(1);
            return tempList.ToArray();

            // https://localhost:44377/ChordProgressions/testSql
        }

        [EnableCors("GuitarAngularApp")]
        [HttpGet("getProgs")]
        public async Task<IEnumerable<ChordProgression>> GetProgAsync([FromQuery] string[] genre, [FromQuery] string[] key)
        {

            List<ChordProgression> tempList = await this._progressionRepos.GetProgressions(genre, key);
            return tempList.ToArray();

            // https://localhost:44377/ChordProgressions/getProgs?genre=Jazz&genre=Country&key=A&key=Am&key=Em
        }

        [EnableCors("GuitarAngularApp")]
        [HttpGet("canMake")]
        public async Task<ActionResult<string>> getCanMake()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = "serverlesschord.database.windows.net";
            builder.UserID = "viewprogress";
            builder.Password = "ProgMan234";
            builder.InitialCatalog = "progressionbank";

            List<GuitarChord> tempList = new List<GuitarChord>();
            string canMake = "test";

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("DECLARE @canMake bit= 0;");
                sb.Append("EXECUTE @canMake = dbo.canMakeChords @useremail = \"thomasmarsh16@gmail.com\";");
                sb.Append("SELECT @canMake;");

                String sql = sb.ToString();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while ( reader.Read())
                        {
                            canMake = reader.GetBoolean(0).ToString();
                        }
                    }
                }
            }

            return canMake;

            // https://localhost:44377/ChordProgressions/canMake
        }
    }
}
