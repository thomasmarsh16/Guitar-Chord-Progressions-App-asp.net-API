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
        [HttpGet("progressions")]
        public async Task<IEnumerable<ChordProgression>> GetProgressions([FromQuery] string[] genre, [FromQuery] string[] key)
        {
            List<ChordProgression> tempList = await _progressionRepos.GetProgressions(genre,key);

            return tempList.ToArray();

            // https://localhost:44377/ChordProgressions/chords?genre=blank&key=blank
        }

        [EnableCors("GuitarAngularApp")]
        [HttpPost("saveprogression")]
        public async Task<ActionResult<Boolean>> SaveProgression([FromForm] string progressionID, [FromForm] string email)
        {
            _progressionRepos.SaveProgression( Int32.Parse(progressionID), email);

            return true;

            // https://localhost:44377/ChordProgressions/saveprogression POST action
        }

        [EnableCors("GuitarAngularApp")]
        [HttpGet("getsavedprogressions")]
        public async Task<IEnumerable<ChordProgression>> GetSavedProgressions([FromQuery] string email )
        {
            List<ChordProgression> tempList = await _progressionRepos.GetSavedUserProgressions(email);

            return tempList.ToArray();

            // https://localhost:44377/ChordProgressions/getsavedprogressions?email=test@gmail.com
        }

        [EnableCors("GuitarAngularApp")]
        [HttpPost("removesavedprogressions")]
        public async Task<ActionResult<Boolean>> RemoveSavedProgressions([FromForm] string progressionID, [FromForm] string email)
        {
            _progressionRepos.DeleteSavedUserProgression(Int32.Parse(progressionID), email);

            return true;

            // https://localhost:44377/ChordProgressions/removesavedprogressions
        }

        [EnableCors("GuitarAngularApp")]
        [HttpGet("options")]
        public async Task<ActionResult<ProgessionOption>> GetOptions()
        {
            ProgessionOption options = new ProgessionOption();

            options = await this._progressionRepos.GetProgessionOptions();

            if (options == null)
            {
                return NotFound();
            }

            return options;

            // https://localhost:44377/ChordProgressions/options
        }
    }
}
