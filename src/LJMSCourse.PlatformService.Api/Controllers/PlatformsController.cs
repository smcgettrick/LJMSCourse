using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LJMSCourse.PlatformService.Api.Data.Repositories;
using LJMSCourse.PlatformService.Api.Models;
using LJMSCourse.PlatformService.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LJMSCourse.PlatformService.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPlatformRepository _repository;

        public PlatformsController(IPlatformRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetAllPlatforms()
        {
            var platforms = await _repository.GetAllAsync();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpGet("{id:int}", Name="GetPlatformById")]
        public async Task<ActionResult<PlatformReadDto>> GetPlatformById(int id)
        {
            var platform = await _repository.GetByIdAsync(id);

            if (platform is null)
                return NotFound();

            return Ok(_mapper.Map<PlatformReadDto>(platform));
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform([FromBody] PlatformCreateDto platformCreateDto)
        {
            var platform = _mapper.Map<Platform>(platformCreateDto);
            await _repository.CreateAsync(platform);

            var platformReadDto = _mapper.Map<PlatformReadDto>(platform);

            return CreatedAtRoute(nameof(GetPlatformById), new { platformReadDto.Id }, platformReadDto);
        }
    }
}