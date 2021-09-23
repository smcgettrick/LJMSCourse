using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using Grpc.Net.Client;
using LJMSCourse.CommandService.Api.Models;
using LJMSCourse.CommandService.Api.Protos;
using Microsoft.Extensions.Configuration;

namespace LJMSCourse.CommandService.Api.Services
{
    public class GrpcPlatformService : IGrpcPlatformService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GrpcPlatformService(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Platform>> GetAllRemotePlatformsAsync()
        {
            var endpoint = _configuration["Grpc:PlatformService"];
            Console.WriteLine($"--> GrpcPlatformService.GetAllRemotePlatformsAsync: Retrieving platforms from {endpoint}");

            var channel = GrpcChannel.ForAddress(endpoint);
            var client = new Protos.GrpcPlatformService.GrpcPlatformServiceClient(channel);

            var grpcPlatforms = client.GetAllPlatforms(new GetAllPlatformsRequest());

            var platforms = new List<Platform>();

            await foreach (var grpcPlatform in grpcPlatforms.ResponseStream.ReadAllAsync())
                platforms.Add(_mapper.Map<Platform>(grpcPlatform));

            return platforms;
        }
    }
}