﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDRM.Models;
using SDRM.Data;

namespace SDRM.Controllers{
    [ApiController]
    [Route("[controller]")]
    public class RoadMapController  :   ControllerBase{
        private readonly RoadMapItemContext _context;

        public RoadMapController (RoadMapItemContext context){
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoadMapItem>>> GetRoadMap(){
            var items = await _context.RoadMapItems.Select(i => i).ToListAsync();

            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoadMapItem>> GetRoadMapItem(int id){
            var item = await _context.RoadMapItems.FirstAsync(i => i.ID == id);

            return item;
        }

        [HttpPost]
        public async Task<ActionResult<RoadMapItem>> PostRoadMapItem(RoadMapItem item){
            _context.RoadMapItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRoadMapItem), item.ID, item);
        }
    }
}