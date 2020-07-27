using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public IEnumerable<ChordProgression> GetChords([FromQuery] string[] genre, [FromQuery] string[] key)
        {
            List<ChordProgression> tempList = new List<ChordProgression>();

            if ( key.Contains("A"))
            {
                tempList.Add(new ChordProgression("Country", "A", "I-IV-V-I", new GuitarChord[] { new GuitarChord("A min", 1, new int[] { -1, 0, 2, 2, 1, 0 }, false, 0),
                                                                                              new GuitarChord("C maj", 1, new int[] { -1, 3, 2, 0, 1, 0 }, false, 0),
                                                                                              new GuitarChord("D maj", 1, new int[] { -1, -1, 0, 2, 3, 2 }, false, 0),
                                                                                              new GuitarChord("A min", 1, new int[] { -1, 0, 2, 2, 1, 0 }, false, 0)
                }));
            }

            if (genre.Contains("Jazz"))
            {
                tempList.Add(new ChordProgression("Country", "C", "I-IV-V-I", new GuitarChord[] { new GuitarChord("C maj", 1, new int[] { -1, 3, 2, 0, 1, 0 }, false, 0),
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
        public ActionResult<progessionOption> GetOptions( )
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
    }
}
