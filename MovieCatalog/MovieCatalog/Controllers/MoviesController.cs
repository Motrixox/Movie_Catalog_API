using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Data;
using MovieCatalog.Models;

namespace MovieCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MoviesDbContext _context;
        private readonly ILogger _logger;

        public MoviesController(MoviesDbContext context, ILogger<MoviesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET api/movies
        [HttpGet]
        public ActionResult<Movie> GetMovie()
        {
            _logger.LogInformation("{DT} - HTTP Get request", DateTime.Now.ToString());

            var movie = _context.Movies.LastOrDefault();

            if(movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        // POST api/movies
        [HttpPost]
        public ActionResult<Movie> AddMovie([FromBody] Movie movie)
        {
            _logger.LogInformation("{DT} - HTTP Post request", DateTime.Now.ToString());

            _context.Movies.Add(movie);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        // GET api/movies/year/2001
        [HttpGet("year/{year}")]
        public ActionResult<IEnumerable<Movie>> GetMoviesByYear(string year)
        {
            int intYear;
            
            _logger.LogInformation("{DT} - HTTP Get by year '{string}' request", DateTime.Now.ToString(), year);

            if (!Int32.TryParse(year, out intYear))
            {
                return BadRequest("Year must be a number between 1800-2100.");
            }

            if (intYear < 1800 || intYear > 2100)
            {
                return BadRequest("The year must be between 1800-2100.");
            }

            var movies = _context.Movies.Where(x => x.Year == intYear).ToList();

            if (movies.FirstOrDefault() == null)
            {
                return NotFound();
            }

            return Ok(movies);
        }

        // GET api/movies/genre/Drama
        [HttpGet("genre/{genre}")]
        public ActionResult<IEnumerable<Movie>> GetMoviesByGenre(string genre)
        {
            _logger.LogInformation("{DT} - HTTP Get by genre '{string}' request", DateTime.Now.ToString(), genre);

            var movies = _context.Movies.Where(x => x.Genre.Equals(genre)).ToList();

            if (movies.FirstOrDefault() == null)
            {
                return NotFound();
            }

            return Ok(movies);
        }

    }

}
