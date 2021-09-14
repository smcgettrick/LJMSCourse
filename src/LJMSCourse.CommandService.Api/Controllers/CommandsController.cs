using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LJMSCourse.CommandService.Api.Data.Repositories;
using LJMSCourse.CommandService.Api.Models;
using LJMSCourse.CommandService.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LJMSCourse.CommandService.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommandRepository _repository;

        public CommandsController(ICommandRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommandReadDto>>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> CommandsController.GetCommandsForPlatform: {platformId}");

            var platformExists = await _repository.PlatformExistsAsync(platformId);
            if (!platformExists)
                return NotFound(platformId);

            var commands = await _repository.GetAllCommandsForPlatform(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public async Task<ActionResult<CommandReadDto>> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> CommandsController.GetCommandForPlatform: {platformId} / {commandId}");

            var platformExists = await _repository.PlatformExistsAsync(platformId);
            if (!platformExists)
                return NotFound(platformId);

            var command = await _repository.GetCommandAsync(platformId, commandId);
            if (command == null)
                return NotFound(commandId);

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public async Task<ActionResult<CommandReadDto>> CreateCommand(int platformId,
            [FromBody] CommandCreateDto commandCreateDto)
        {
            Console.WriteLine($"--> CommandsController.CreateCommand: {platformId}");

            var platformExists = await _repository.PlatformExistsAsync(platformId);
            if (!platformExists)
                return NotFound(platformId);

            var command = _mapper.Map<Command>(commandCreateDto);

            await _repository.CreateCommandAsync(platformId, command);

            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommandForPlatform), new { platformId, commandId = commandReadDto.Id },
                commandReadDto);
        }
    }
}