using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/trail")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecTrails")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailController : ControllerBase
    {
        private readonly ITrailRepository _trailRepo;
        private readonly IMapper _mapper;

        public TrailController(ITrailRepository trailRepo, IMapper mapper)
        {
            _trailRepo = trailRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all of the trails
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        //[ProducesResponseType(400)]
        public IActionResult GetTrails()
        {
            var trails = _trailRepo.GetTrails();
            var trailDtos = new List<TrailDto>();

            foreach (var item in trails)
            {
                trailDtos.Add(_mapper.Map<TrailDto>(item));
            }

            return Ok(trails);
        }

        /// <summary>
        /// Get the individual trail
        /// </summary>
        /// <param name="trailId">The id of the trail</param>
        /// <returns></returns>

        [HttpGet("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        //[ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "Admin")]
        public IActionResult GetTrail(int trailId)
        {
            var trail = _trailRepo.GetTrail(trailId);
            var currentUser = User.Identity.Name;
            if (trail == null)
                return NotFound();

            var trailDto = _mapper.Map<TrailDto>(trail);
            return Ok(trailDto);
        }

        [HttpGet("[action]/{nationalParkId:int}")]
        //[HttpGet("GetTrailsInNationalPark/{nationalParkId:int}")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailsInNationalPark(int nationalParkId)
        {
            var trails = _trailRepo.GetTrailsInNationalPark(nationalParkId);

            if (trails == null)
                return NotFound();

            var trailDtos = new List<TrailDto>();
            foreach (var item in trails)
            {
                trailDtos.Add(_mapper.Map<TrailDto>(item));
            }

            return Ok(trailDtos);
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TrailDto))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto) 
        {
            if (trailDto == null)
                return BadRequest(ModelState);

            if (_trailRepo.TrailExists(trailDto.Name)) 
            {
                ModelState.AddModelError("", "This national park is already exist");
                return StatusCode(404, ModelState);
            }

            var trail = _mapper.Map<Trail>(trailDto);

            if (!_trailRepo.CreateTrail(trail)) 
            {
                ModelState.AddModelError("", "Something went wrong when adding national park to database");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { trailId = trail.Id }, trail);
        }

        [HttpPatch("{TrailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDto trailDto) 
        {
            if (trailDto == null || trailId != trailDto.Id) 
            {
                return BadRequest(ModelState);
            }

            var trail = _mapper.Map<Trail>(trailDto);

            if (!_trailRepo.UpdateTrail(trail))
            {
                ModelState.AddModelError("", "Something went wrong when updating national park to database");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepo.TrailExists(trailId))
                return NotFound();

            var trail = _trailRepo.GetTrail(trailId);

            if (!_trailRepo.DeleteTrail(trail))
            {
                ModelState.AddModelError("", "Something went wrong when deleting national park to database");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
