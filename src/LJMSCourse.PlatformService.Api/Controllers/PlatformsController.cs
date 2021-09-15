using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LJMSCourse.PlatformService.Api.Data.Repositories;
using LJMSCourse.PlatformService.Api.Models;
using LJMSCourse.PlatformService.Api.Models.Dtos;
using LJMSCourse.PlatformService.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LJMSCourse.PlatformService.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IMessageBusClient _messageBusClient;
        private readonly IMapper _mapper;
        private readonly IPlatformRepository _repository;

        public PlatformsController(
            IPlatformRepository repository,
            IMapper mapper, 
            IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetAllPlatforms()
        {
            var platforms = await _repository.GetAllAsync();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpGet("{id:int}", Name = "GetPlatformById")]
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

            try
            {
                var platformPublishDto = _mapper.Map<PlatformPublishDto>(platformReadDto);
                platformPublishDto.Event = "Platform_Published";

                await _messageBusClient.PublishPlatform(platformPublishDto);
            }
            catch (Exception ex)
            {
                Console.Write($"--> PlatformsController.CreatePlatform: Could not send to message bus: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { platformReadDto.Id }, platformReadDto);
        }
    }
}