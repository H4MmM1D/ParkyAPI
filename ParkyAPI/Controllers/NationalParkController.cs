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
    [Route("api/v{version:apiVersion}/nationalpark")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParkController : ControllerBase
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParkController(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all of the national parks
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
        //[ProducesResponseType(400)]
        public IActionResult GetNationalParks()
        {
            var nationalParks = _npRepo.GetNationalParks();
            var nationalParkDtos = new List<NationalParkDto>();

            foreach (var item in nationalParks)
            {
                nationalParkDtos.Add(_mapper.Map<NationalParkDto>(item));
            }

            return Ok(nationalParks);
        }

        /// <summary>
        /// Get the individual national park 
        /// </summary>
        /// <param name="nationalParkId">The id of the national park</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        //[ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var nationalPark = _npRepo.GetNationalPark(nationalParkId);

            if (nationalPark == null)
                return NotFound();

            var nationalParkDto = _mapper.Map<NationalParkDto>(nationalPark);
            return Ok(nationalParkDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NationalParkDto))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto) 
        {
            if (nationalParkDto == null)
                return BadRequest(ModelState);

            if (_npRepo.NationalParkExists(nationalParkDto.Name)) 
            {
                ModelState.AddModelError("", "This national park is already exist");
                return StatusCode(404, ModelState);
            }

            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_npRepo.CreateNationalPark(nationalPark)) 
            {
                ModelState.AddModelError("", "Something went wrong when adding national park to database");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark", new { Version = HttpContext.GetRequestedApiVersion().ToString(), nationalParkId = nationalPark.Id }, nationalPark);
        }

        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto) 
        {
            if (nationalParkDto == null || nationalParkId != nationalParkDto.Id) 
            {
                return BadRequest(ModelState);
            }

            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_npRepo.UpdateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", "Something went wrong when updating national park to database");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_npRepo.NationalParkExists(nationalParkId))
                return NotFound();

            var nationalPark = _npRepo.GetNationalPark(nationalParkId);

            if (!_npRepo.DeleteNationalPark(nationalPark))
            {
                ModelState.AddModelError("", "Something went wrong when deleting national park to database");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
