using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SDRM.Models;
using SDRM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SDRM.Controllers{
    [ApiController]
    [Route("/api/[controller]")]
    public class RoadMapItemController : ControllerBase{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserContext _userContext;
        private readonly ILogger<RoadMapItemController> _logger;
        public RoadMapItemController(UserManager<ApplicationUser> userManager, 
                                     UserContext userContext,
                                     ILogger<RoadMapItemController> logger
        ){
            _userManager = userManager;
            _userContext = userContext;
            _logger = logger;
        }

        public class Item{
            public int id { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public bool isComplete { get; set; }
        }
        public int[] ItemIDs { get; set; }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetRoadMapItems(){
            _logger.LogInformation("Get: RoadMapItems");

            if (HttpContext.User == null){
                throw new ArgumentNullException("HttpContextUser.User", "User Does Not Exist");
            }
            
            string id = null;
            var claims = HttpContext.User.Claims.ToList();
            
            foreach(Claim claim in claims){
                if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"){
                    _logger.LogInformation($"ID: {claim.Value} Found");

                    id = claim.Value;
                    
                    break;
                }
            }

            var roadMapItems = _userContext.RoadMapItems.Where(u => u.UserID == id).OrderBy(u => u.ID).ToList();

            _logger.LogInformation($"roadMapItems:");
            foreach(RoadMapItem i in roadMapItems){
                _logger.LogInformation($"{i.Title}: {i.Content}");
            }
            
            return Ok(roadMapItems);
        }

        [HttpPost("AddRoadMapItem")]
        public async Task<ActionResult> AddRoadMapItem(Item item){
            _logger.LogInformation($"Post: AddRoadMapItem");
            _logger.LogInformation($"{item.title}: {item.description}");

            var claim = HttpContext.User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").SingleOrDefault();
            var userID = claim.Value;

            _logger.LogInformation($"ID Found: {userID}");
            var user = await _userContext.Users.Where(u => u.ID == userID).SingleOrDefaultAsync();

            RoadMapItem roadMapItem = new RoadMapItem(){
                Title = item.title,
                Content = item.description,
                IsComplete = false
            };

            user.RoadMapItems.Add(roadMapItem);
            _userContext.RoadMapItems.Add(roadMapItem);
            var result = await _userContext.SaveChangesAsync();

            if (result > 0){
                _logger.LogInformation($"Item Added");
                return Ok(200);
            }

            _logger.LogInformation("unable to add item");
            return BadRequest();
        }
        [HttpPost("DeleteRoadMapItem")]
        public async Task<ActionResult> DeleteRoadMapItem(Item item){
            _logger.LogInformation($"Post: DeleteRoadMapItem");
            _logger.LogInformation($"{item.id}");

            var claim = HttpContext.User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").SingleOrDefault();

            var user = await _userContext.Users.Where(u => u.ID == claim.Value).SingleOrDefaultAsync();
            var targetItem = await _userContext.RoadMapItems.Where(i => i.ID == item.id).SingleOrDefaultAsync(); 

            user.RoadMapItems.Remove(targetItem);
            _userContext.RoadMapItems.Remove(targetItem);
            var result = await _userContext.SaveChangesAsync();

            var _Items = _userContext.RoadMapItems.Where(u => u.UserID == claim.Value).ToList();

            _logger.LogInformation("_Items: ");
            foreach (RoadMapItem i in _Items){
                _logger.LogInformation($"{i.Title}: {i.Content}");
            }

            if (result > 0){
                _logger.LogInformation($"{item.id}: {item.title} - Successfully Deleted");
                return Ok(200);
            }
            
            _logger.LogInformation($"Failed to Delete Item");
            return BadRequest();
        }
        
        [HttpPost("UpdateRoadMapItem")]
        public async Task<IActionResult> UpdateRoadMapItem(Item item){
            _logger.LogInformation($"Item to Update: {item.id}");

            var itemEdit = await _userContext.RoadMapItems.FindAsync(item.id);

            itemEdit.Title = item.title;
            itemEdit.Content = item.description;

            _userContext.RoadMapItems.Update(itemEdit);
            var results = await _userContext.SaveChangesAsync();

            if (results > 0){
                _logger.LogInformation($"Item Successfully Updated!");
                return Ok(200);
            }
            
            _logger.LogInformation($"Failed to Update Item");
            return BadRequest();
        }
    }
}