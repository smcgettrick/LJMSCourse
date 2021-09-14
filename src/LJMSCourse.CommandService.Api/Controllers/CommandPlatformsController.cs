using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LJMSCourse.CommandService.Api.Data.Repositories;
using LJMSCourse.CommandService.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LJMSCourse.CommandService.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class CommandPlatformsController : ControllerBase
    {
        private readonly ICommandRepository _repository;
        private readonly IMapper _mapper;

        public CommandPlatformsController(ICommandRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetAllPlatforms()
        {
            Console.WriteLine("--> CommandPlatformsController.GetAllPlatforms: Received GET request");

            var platforms = await _repository.GetAllPlatformsAsync();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }
    }
}