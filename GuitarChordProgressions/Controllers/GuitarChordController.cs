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
        public IEnumerable<ChordProgression> GetChords()
        {
            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();

            List<ChordProgression> tempList = new List<ChordProgression>();

            tempList.Add(new ChordProgression("Country", "A", "I-IV-V-I", new GuitarChord[] { new GuitarChord("A min", 1, new int[] { -1, 0, 2, 2, 1, 0 }, false, 0),
                                                                                              new GuitarChord("C maj", 1, new int[] { -1, 3, 2, 0, 1, 0 }, false, 0),
                                                                                              new GuitarChord("D maj", 1, new int[] { -1, -1, 0, 2, 3, 2 }, false, 0),
                                                                                              new GuitarChord("A min", 1, new int[] { -1, 0, 2, 2, 1, 0 }, false, 0)
            }));

            tempList.Add(new ChordProgression("Country", "C", "I-IV-V-I", new GuitarChord[] { new GuitarChord("C maj", 1, new int[] { -1, 3, 2, 0, 1, 0 }, false, 0),
                                                                                              new GuitarChord("D maj", 1, new int[] { -1, -1, 0, 2, 3, 2 }, false, 0),
                                                                                              new GuitarChord("E maj", 1, new int[] {  0, 2, 2, 1, 0, 0 }, false, 0),
                                                                                              new GuitarChord("C maj", 1, new int[] { -1, 3, 2, 0, 1, 0 }, false, 0)
            }));

            return tempList.ToArray();

            // https://localhost:44377/ChordProgressions/chords
        }
    }
}
