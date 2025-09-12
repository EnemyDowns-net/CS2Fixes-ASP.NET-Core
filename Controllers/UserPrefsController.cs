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
        public async Task<IActionResult> Get([FromQuery] long steamid)
        {
            var prefs = await _context.Userprefs.Where(p => p.Steamid == steamid).ToListAsync();

            var result = new JsonObject();

            // cs2fixes will looking for it anyway return anything than Ok it just result to not working.
            if(prefs.Count == 0 || prefs == null)
            {
                result["steamid"] = steamid.ToString();
                return Ok(result);
            }

            foreach (var pref in prefs)
            {
                result[pref.Key] = pref.Value;
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] long steamid, [FromBody] JsonObject userPrefs)
        {
            foreach (var jsonPref in userPrefs)
            {
                var existingPref = await _context.Userprefs.FirstOrDefaultAsync(p => p.Steamid == steamid && p.Key == jsonPref.Key);

                if (existingPref != null)
                {
                    existingPref.Value = jsonPref.Value?.ToString() ?? string.Empty;
                    // No need to call Update; EF tracks changes automatically
                }
                else
                {
                    var pref = new Userpref
                    {
                        Steamid = steamid,
                        Key = jsonPref.Key,
                        Value = jsonPref.Value?.ToString() ?? string.Empty
                    };
                    await _context.Userprefs.AddAsync(pref);
                }
            }

            await _context.SaveChangesAsync();

            JsonObject results = new JsonObject
            {
                ["message"] = "Userprefs updated successfully"
            };

            return Ok(results);
        }
    }
}