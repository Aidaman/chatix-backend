using Microsoft.AspNetCore.Mvc;
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Builders;
using Study_DOT_NET.Shared.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Study_DOT_NET.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly Room _commonRoom;
    private readonly MessagesService _messagesService;
    private readonly RoomsService _roomsService;
    private readonly UsersService _usersService;

    public RoomsController(RoomsService roomsService, UsersService usersService, MessagesService messagesService)
    {
        _roomsService = roomsService;
        _usersService = usersService;
        _messagesService = messagesService;
        _commonRoom = new Room
        {
            Id = "common",
            Title = "Common",
            Participants = usersService.GetAsync().Result,
            IsPublic = true
        };
    }

    private async Task<List<Room>> BuildRooms(List<Room> rooms, string? userId)
    {
        rooms.Add(_commonRoom);

        for (var i = 0; i < rooms.Count; i++)
            if (rooms[i].Id != "common")
            {
                var builder = new RoomBuilder(rooms[i], _roomsService, _usersService,
                    _messagesService);

                await builder.ConfigureCreator();
                await builder.ConfigureParticipants();
                if (userId is not null) await builder.ConfigureUnread(userId);

                rooms[i] = builder.Build() as Room ??
                           throw new ApplicationException("Builder didn't built the room properly");
            }

        return rooms;
    }

    [HttpGet]
    public async Task<List<Room>> Get()
    {
        return await _roomsService.GetAsync();
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Room>> Get(string id)
    {
        var room = await _roomsService.GetAsync(id);

        if (room is null) return NotFound();

        return room;
    }

    [HttpGet("availableFor/{userId:length(24)}")]
    public async Task<ActionResult<List<Room>>> GetAvailable(string userId)
    {
        return await GetAvailable(userId, null);
    }

    [HttpGet("availableFor/{userId:length(24)}/{isPublic:bool}")]
    public async Task<ActionResult<List<Room>>> GetAvailable(string userId, bool? isPublic)
    {
        // isPublic??= true;

        var user = await _usersService.GetAsync(userId);
        if (user is null) return NotFound();

        var rooms = new List<Room?>();

        foreach (var userRoomId in user.RoomIds) rooms.Add(await _roomsService.GetAsync(userRoomId));

        rooms.RemoveAll(x => x == null);
        if (isPublic is not null) rooms.RemoveAll(x => x.IsPublic != isPublic);

        rooms = await BuildRooms(rooms, userId);
        return rooms;
    }

    [HttpGet("Search/{title}")]
    public async Task<ActionResult<List<Room>>> Search(string title)
    {
        if (title == "undefined") return BadRequest();

        var rooms = await _roomsService.SearchAsync(title);
        rooms = await BuildRooms(rooms, null);

        if (rooms is null) return NotFound();

        return rooms;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Room newRoom)
    {
        await _roomsService.CreateAsync(newRoom);

        //CreatedAtAction() - creates ICreatedAtAction object that basically returns 201 response with Location header
        return CreatedAtAction(nameof(Get), new { id = newRoom.Id }, newRoom);
    }

    [HttpPut("id;length(24)")]
    public async Task<IActionResult> Update(string id, Room updatedRoom)
    {
        var room = await _roomsService.GetAsync(id);

        if (room is null) return NotFound();

        updatedRoom.Id = room.Id;

        await _roomsService.UpdateAsync(id, updatedRoom);

        //NoContent() - creates NoContentResult object that basically returns response to server and signalling that there is empty response body
        return NoContent();
    }

    [HttpDelete("id;length(24)")]
    public async Task<IActionResult> Delete(string id)
    {
        var room = await _roomsService.GetAsync(id);

        if (room is null) return NotFound();

        await _roomsService.RemoveAsync(id);

        //NoContent() - creates NoContentResult object that basically returns response to server and signalling that there is empty response body
        return NoContent();
    }
}