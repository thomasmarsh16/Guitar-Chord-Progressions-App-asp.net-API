﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.Text;

namespace GuitarChordProgressions.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChordProgressionsController : ControllerBase
    {

        private readonly ILogger<ChordProgressionsController> _logger;

        public ChordProgressionsController(ILogger<ChordProgressionsController> logger)
        {
            _logger = logger;
        }

        [EnableCors("GuitarAngularApp")]
        [HttpGet("chords")]
        public async Task<IEnumerable<ChordProgression>> GetChords([FromQuery] string[] genre, [FromQuery] string[] key)
        {
            List<ChordProgression> tempList = new List<ChordProgression>();

            if ( key.Contains("A"))
            {
                tempList.Add(new ChordProgression("Rock", "A", "I-IV-V-I", new GuitarChord[] { new GuitarChord("A min", 1, new int[] { -1, 0, 2, 2, 1, 0 }, false, 0),
                                                                                              new GuitarChord("C maj", 1, new int[] { -1, 3, 2, 0, 1, 0 }, false, 0),
                                                                                              new GuitarChord("D maj", 1, new int[] { -1, -1, 0, 2, 3, 2 }, false, 0),
                                                                                              new GuitarChord("A min", 1, new int[] { -1, 0, 2, 2, 1, 0 }, false, 0)
                }));
            }

            if (genre.Contains("Jazz"))
            {
                tempList.Add(new ChordProgression("Rock", "C", "I-IV-V-I", new GuitarChord[] { new GuitarChord("C maj", 1, new int[] { -1, 3, 2, 0, 1, 0 }, false, 0),
                                                                                              new GuitarChord("D maj", 1, new int[] { -1, -1, 0, 2, 3, 2 }, false, 0),
                                                                                              new GuitarChord("E maj", 1, new int[] {  0, 2, 2, 1, 0, 0 }, false, 0),
                                                                                              new GuitarChord("C maj", 1, new int[] { -1, 3, 2, 0, 1, 0 }, false, 0)
                }));
                tempList.Add(new ChordProgression("Jazz", "C", "I-IV-V-I", new GuitarChord[] { new GuitarChord("C maj", 1, new int[] { -1, 3, 2, 0, 1, 0 }, false, 0),
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
        public async Task<ActionResult<progessionOption>> GetOptions( )
        {
            progessionOption options = new progessionOption();

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
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = "serverlesschord.database.windows.net";
            builder.UserID = "viewprogress";
            builder.Password = "ProgMan234";
            builder.InitialCatalog = "progressionbank";

            List<GuitarChord> tempList = new List<GuitarChord>();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM dbo.Chords");
                String sql = sb.ToString();

                using( SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            string[] tempArray = reader.GetString(3).Split(",");
                            tempList.Add(new GuitarChord(reader.GetString(1), 
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
                    }
                }
            }

            return tempList.ToArray();

            // https://localhost:44377/ChordProgressions/testSql
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
