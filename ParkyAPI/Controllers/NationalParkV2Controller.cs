using AutoMapper;
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
    [Route("api/v{version:apiVersion}/nationalpark")]
    [ApiVersion("2.0")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParkV2Controller : ControllerBase
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParkV2Controller(INationalParkRepository npRepo, IMapper mapper)
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
    }
}
