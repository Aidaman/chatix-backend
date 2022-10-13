using Microsoft.AspNetCore.Mvc;
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Builders;
using Study_DOT_NET.Shared.Models;
using Study_DOT_NET.Shared.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Study_DOT_NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly RoomsService _roomsService;
        private readonly UsersService _usersService;
        private readonly MessagesService _messagesService;

        private readonly Room _commonRoom;
        public RoomsController(RoomsService roomsService, UsersService usersService, MessagesService messagesService)
        {
            this._roomsService = roomsService;
            this._usersService = usersService;
            this._messagesService = messagesService;
            _commonRoom = new Room()
            {
                Id = "common",
                Title = "Common",
                Participants = usersService.GetAsync().Result,
                IsPublic = true,
            };
        }

        private async Task<List<Room>> BuildRooms(List<Room> rooms, string? userId)
        {
            rooms.Add(_commonRoom);

            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].Id != "common")
                {
                    RoomBuilder builder = new RoomBuilder(rooms[i], this._roomsService, this._usersService,
                        this._messagesService);

                    await builder.ConfigureCreator();
                    await builder.ConfigureParticipants();
                    if (userId is not null)
                    {
                        await builder.ConfigureUnread(userId);   
                    }
                    rooms[i] = builder.Build() as Room ?? throw new ApplicationException("Builder didn't built the room properly");
                }
            }

            return rooms;
        }
        
        [HttpGet]
        public async Task<List<Room>> Get()
        {
            return await this._roomsService.GetAsync();
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Room>> Get(string id)
        {
            Room room = await this._roomsService.GetAsync(id);

            if (room is null)
            {
                return NotFound();
            }

            return room;
        }

        [HttpGet("availableFor/{userId:length(24)}")]
        public async Task<ActionResult<List<Room>>> GetAvailable(string userId)
        {
            return await this.GetAvailable(userId, null);
        }

        [HttpGet("availableFor/{userId:length(24)}/{isPublic:bool}")]
        public async Task<ActionResult<List<Room>>> GetAvailable(string userId, bool? isPublic)
        {
            // isPublic??= true;
            
            User? user = await this._usersService.GetAsync(userId);
            if (user is null)
            {
                return NotFound();
            }
            
            List<Room?> rooms = new List<Room?>();
            
            foreach (string userRoomId in user.RoomIds)
            {
                rooms.Add(await this._roomsService.GetAsync(userRoomId));
            }

            rooms.RemoveAll(x => x == null);
            if (isPublic is not null)
            {
                rooms.RemoveAll(x => x.IsPublic != isPublic);   
            }

            rooms = await this.BuildRooms(rooms, userId);
            return rooms;
        }

        [HttpGet("Search/{title}")]
        public async Task<ActionResult<List<Room>>> Search(string title)
        {
            if (title.ToString() == "undefined")
            {
                return BadRequest();
            }
            
            List<Room> rooms = await this._roomsService.SearchAsync(title);
            rooms = await this.BuildRooms(rooms, null);
            
            if (rooms is null)
            {
                return NotFound();
            }

            return rooms;
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(Room newRoom)
        {
            await this._roomsService.CreateAsync(newRoom);

            //CreatedAtAction() - creates ICreatedAtAction object that basically returns 201 response with Location header
            return CreatedAtAction(nameof(Get), new { id = newRoom.Id }, newRoom);
        }

        [HttpPut("id;length(24)")]
        public async Task<IActionResult> Update(string id, Room updatedRoom)
        {
            Room room = await this._roomsService.GetAsync(id);

            if (room is null)
            {
                return NotFound();
            }

            updatedRoom.Id = room.Id;

            await this._roomsService.UpdateAsync(id, updatedRoom);

            //NoContent() - creates NoContentResult object that basically returns response to server and signalling that there is empty response body
            return NoContent();
        }

        [HttpDelete("id;length(24)")]
        public async Task<IActionResult> Delete(string id)
        {
            Room room = await this._roomsService.GetAsync(id);

            if (room is null)
            {
                return NotFound();
            }

            await this._roomsService.RemoveAsync(id);

            //NoContent() - creates NoContentResult object that basically returns response to server and signalling that there is empty response body
            return NoContent();
        }


    }
}
