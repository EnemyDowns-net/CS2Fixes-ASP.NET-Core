using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;
using CS2Fixes_ASP_DOTNET_Core.Contexts;
using CS2Fixes_ASP_DOTNET_Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CS2Fixes_ASP_DOTNET_Core.Controllers
{
    [ApiController]
    [Route("UserPrefs")]
    // Our requests/responses can't be properly backed with a C# class, because userpref key names are dynamic
    public class UserPrefsController : ControllerBase
    {
        private readonly ILogger<UserPrefsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly UserprefDbContext _context;

        public UserPrefsController(ILogger<UserPrefsController> logger, IConfiguration configuration, UserprefDbContext context)
        {
            _logger = logger;
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<JsonObject>> Get([Required] long steamid)
        {
            return await GetUserPrefs(steamid);
        }

        [HttpPost]
        public async Task<ActionResult<JsonObject>> Post([Required] long steamid, [Required] JsonObject userPrefs)
        {
            foreach (var jsonPref in userPrefs)
            {
                Userpref pref = new Userpref()
                {
                    Steamid = steamid,
                    Key = jsonPref.Key,
                    Value = (string)jsonPref.Value
                };

                if (_context.Userprefs.Any(pref => pref.Steamid == steamid && pref.Key == jsonPref.Key))
                    _context.Userprefs.Update(pref);
                else
                    _context.Userprefs.Add(pref);
            }

            await _context.SaveChangesAsync();

            return await GetUserPrefs(steamid);
        }

        private async Task<ActionResult<JsonObject>> GetUserPrefs(long steamid)
        {
            List<Userpref> prefs = await _context.Userprefs.AsQueryable().Where(s => s.Steamid == steamid).ToListAsync();

            JsonObject json = new JsonObject();

            foreach (Userpref pref in prefs)
                json[pref.Key] = pref.Value;

            return json;
        }
    }
}
}