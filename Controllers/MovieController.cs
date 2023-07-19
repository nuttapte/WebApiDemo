using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiDemo.Models;



namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieContext _context;

        public MovieController(MovieContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovie()
        {
            return await _context.Movie.ToListAsync();
        }

        // GET:api/Movie/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {

            var result = await _context.Movie.FindAsync(id);
            if (result!=null)
            {
                return result;
            }
            else
            {
                return NotFound();

            }
        }

        [HttpPut("/UpdateMovie/{id}")]
        public async Task<ActionResult> UpdateMovie(int id, Movie Mv)
        {
            if(id != Mv.Id)
            {
                return BadRequest();
            }

            _context.Entry(Mv).State = EntityState.Modified;

            try
            {
                await  _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(! MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPost("/AddMovie")]
        public async Task<ActionResult<Movie>> AddMovie(Movie Mv)
        {
            _context.Movie.Add(Mv);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new {id = Mv.Id}, Mv);
        }

        [HttpDelete("/DeleteMovie/{id}")]
        public async Task<ActionResult<Movie>> DeleteMovie(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return movie;
        }



        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }

    }
}