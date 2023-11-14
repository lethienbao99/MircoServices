using AutoMapper;
using Grpc.Core;
using PlatformService;
using PlatformService.Data;

public class GrpcPlatformService: GrpcPlatform.GrpcPlatformBase
{
    private readonly IPlatFormRepo _platFormRepo;
    private readonly IMapper _mapper;

    public GrpcPlatformService(IPlatFormRepo platFormRepo, IMapper mapper)
    {
        _platFormRepo = platFormRepo;
        _mapper = mapper;
    }

    public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, 
        ServerCallContext context)
    {
        var response = new PlatformResponse();
        var platforms = _platFormRepo.GetAllFlatforms();

        foreach(var plat in platforms)
        {
            response.PlatformId.Add(_mapper.Map<GrpcPlatformModel>(plat));
        }

        return Task.FromResult(response);
    } 

}