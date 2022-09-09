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
        private readonly IRegionRepository _regionRepository;
        private readonly IWalkDifficultyRepository _walkDifficultyRepository;
        private readonly IMapper _mapper;

        public WalksController(IWalkRepository walkRepository,IRegionRepository regionRepository,IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            _walkRepository = walkRepository;
            _regionRepository = regionRepository;
            _walkDifficultyRepository = walkDifficultyRepository;
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
            //Validate incoming request

            if (!await ValidateAddWalkAsync(addWalkDto))
            {
                return BadRequest(ModelState);
            }

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

            //Validate request 

            if (!await ValidateUpdateWalkAsync(updateWalkDto))
            {
                return BadRequest(ModelState);
            }
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

        #region Private Methods
        private async Task<bool> ValidateAddWalkAsync(AddWalkDto addWalkDto)
        {

            if (addWalkDto == null)
            {
                ModelState.AddModelError(nameof(addWalkDto), "Add Walk Data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalkDto.Name))
            {
                ModelState.AddModelError(nameof(addWalkDto.Name), $"{nameof(addWalkDto.Name)} cannot be null or white space");
            }

            if (addWalkDto.Length<=0)
            {
                ModelState.AddModelError(nameof(addWalkDto.Length), $"{nameof(addWalkDto.Length)} cannot be 0 or negative");
            }

            var region = await _regionRepository.GetAsync(addWalkDto.RegionId);

            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkDto.RegionId), $"{nameof(addWalkDto.RegionId)} Region id is invalid");
            }

            var walkDificultyId = await _walkDifficultyRepository.GetAsync(addWalkDto.WalkDifficultyId);

            if (walkDificultyId == null)
            {
                ModelState.AddModelError(nameof(addWalkDto.WalkDifficultyId), $"{nameof(addWalkDto.WalkDifficultyId)} Walk diff id is invalid");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }
        private async Task<bool> ValidateUpdateWalkAsync(UpdateWalkDto updateWalkDto)
        {

            if (updateWalkDto == null)
            {
                ModelState.AddModelError(nameof(updateWalkDto), "Add Walk Data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkDto.Name))
            {
                ModelState.AddModelError(nameof(updateWalkDto.Name), $"{nameof(updateWalkDto.Name)} cannot be null or white space");
            }

            if (updateWalkDto.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkDto.Length), $"{nameof(updateWalkDto.Length)} cannot be 0 or negative");
            }

            var region = await _regionRepository.GetAsync(updateWalkDto.RegionId);

            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkDto.RegionId), $"{nameof(updateWalkDto.RegionId)} Region id is invalid");
            }

            var walkDificultyId = await _walkDifficultyRepository.GetAsync(updateWalkDto.WalkDifficultyId);

            if (walkDificultyId == null)
            {
                ModelState.AddModelError(nameof(updateWalkDto.WalkDifficultyId), $"{nameof(updateWalkDto.WalkDifficultyId)} Walk diff id is invalid");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }
        #endregion


    }
}
