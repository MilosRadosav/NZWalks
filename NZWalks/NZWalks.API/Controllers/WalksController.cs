using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            var walks =  await _walkRepository.GetAllAsync();

            var walksDto = _mapper.Map<List<WalksDto>>(walks);

            return Ok(walksDto);
        }

        [HttpGet]
        [Route("{Id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid Id)
        {
            var walk = await _walkRepository.GetAsync(Id);

            if (walk==null)
            {
                return NotFound();
            }
            var walkDto = _mapper.Map<WalksDto>(walk);

            return Ok(walkDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody]AddWalkDto addWalkDto)
        {
            // Request to Domain model

            var walk = _mapper.Map<Walk>(addWalkDto);

            //Pass details to repository

            walk = await _walkRepository.AddAsync(walk);

            //Convert back to Dto

            var walkDto = _mapper.Map<WalksDto>(walk);

            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDto.Id }, walkDto);
            // return Ok(regionDto);
        }
        [HttpPut]
        //[Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync(Guid Id, [FromBody] UpdateWalkDto updateWalkDto)
        {
            var updateWalk = _mapper.Map<Walk>(updateWalkDto);

            // Update region using repository

            updateWalk = await _walkRepository.UpdateAsync(Id, updateWalk);

            //If null return NotFound

            if (updateWalk == null)
            {
                return NotFound();
            }

            //Convert back to DTO
            var updateDto = _mapper.Map<WalksDto>(updateWalk);
            // Return Ok response

            return Ok(updateWalk);
        }

        [HttpDelete]
        [Route("DeleteWalkById/{Id:guid}")]
        public async Task<IActionResult> DeleteWalkById(Guid Id)
        {
            // Get region from database

            var walk = await _walkRepository.DeleteAsync(Id);

            // If null NotFound

            if (walk == null)
            {
                return NotFound();
            };

            //Convert response to DTO

            var walkDto = _mapper.Map<WalksDto>(walk);

            //Return Ok response

            return Ok(walkDto);
        }
    }
}
