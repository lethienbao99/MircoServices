using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Model;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatFormRepo _platFormRepo;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;

    public PlatformsController(IPlatFormRepo platFormRepo, 
            IMapper mapper, 
            ICommandDataClient commandDataClient,
            IMessageBusClient messageBusClient) 
    => (_platFormRepo, _mapper, _commandDataClient, _messageBusClient) 
    = (platFormRepo, mapper, commandDataClient, messageBusClient);

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        var data = _platFormRepo.GetAllFlatforms();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(data));
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        var data = _platFormRepo.GetPlatformById(id);
        if(data != null)
            return Ok(_mapper.Map<PlatformReadDto>(data));

        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto plat)
    {
        var data = _mapper.Map<Platform>(plat);
        _platFormRepo.CreatePlatform(data);
        var platFormReadDto = _mapper.Map<PlatformReadDto>(data);

        //Send Sync Message
        try
        {
            await _commandDataClient.SendPlatformToCommand(platFormReadDto);
        }
        catch (System.Exception e)
        {
            Console.WriteLine($"--> Could not send synchronusly: {e.Message}");
        }

        //Send Async Message
        try
        {
            var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platFormReadDto);
            platformPublishedDto.Event = "Platform_Published";
            _messageBusClient.PublishNewPlatform(platformPublishedDto);
        }
        catch(Exception e)
        {
            Console.WriteLine($"--> Could not send asynchronusly: {e.Message}");
            
        }

        return CreatedAtRoute(nameof(GetPlatformById), new { id = platFormReadDto.Id }, platFormReadDto);

    }
        
}