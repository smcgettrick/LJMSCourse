using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using LJMSCourse.PlatformService.Api.Data.Repositories;
using LJMSCourse.PlatformService.Api.Protos;

namespace LJMSCourse.PlatformService.Api.Services
{
    public class GrpcPlatformService : Protos.GrpcPlatformService.GrpcPlatformServiceBase
    {
        private readonly IMapper _mapper;
        private readonly IPlatformRepository _repository;

        public GrpcPlatformService(IPlatformRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public override async Task GetAllPlatforms(GetAllPlatformsRequest request,
            IServerStreamWriter<GrpcPlatformModel> responseStream,
            ServerCallContext context)
        {
            var platforms = await _repository.GetAllAsync();

            foreach (var platform in platforms)
                await responseStream.WriteAsync(_mapper.Map<GrpcPlatformModel>(platform));
        }
    }
}