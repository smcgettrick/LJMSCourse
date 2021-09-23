using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LJMSCourse.CommandService.Api.Data.Repositories;
using LJMSCourse.CommandService.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LJMSCourse.CommandService.Api.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommandRepository _repository;

        public PlatformsController(IMapper mapper, ICommandRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetAllPlatforms()
        {
            Console.WriteLine("--> CommandsController.GetAllPlatforms: Received GET request");

            var platforms = await _repository.GetAllPlatformsAsync();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }
    }
}