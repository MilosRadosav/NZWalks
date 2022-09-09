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
    public class WalkDifficultyController : ControllerBase
    {
        private readonly IWalkDifficultyRepository _walkDifficultyRepository;
        private readonly IMapper _mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            _walkDifficultyRepository = walkDifficultyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walkDifficulties = await  _walkDifficultyRepository.GetAllAsync();

            var walkDifficultyDto = _mapper.Map<List<WalkDifficultyDto>>(walkDifficulties);

            return Ok(walkDifficultyDto);
        }

        [HttpGet]
        [Route("{Id:guid}")]
        [ActionName("GetDifById")]
        public async Task<IActionResult> GetDifById(Guid Id)
        { 
            var walkDifficulty = await _walkDifficultyRepository.GetAsync(Id);

            if (walkDifficulty==null)
            {
                NotFound();
            }

            var walkDifficultyDto = _mapper.Map<WalkDifficultyDto>(walkDifficulty);

            return Ok(walkDifficultyDto.Code);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateWalkDifficultyDto updateWalkDifficultyDto)
        {
            // Check request

            if ( !ValidateUpdateWalkDifficulty(updateWalkDifficultyDto))
            {
                return BadRequest(ModelState);
            }

            // Convert DTO to domain model, we are mapping source(updateRegionDto) on destination(our domain model) Region. 

            var updateWalkDifficulty = _mapper.Map<WalkDifficulty>(updateWalkDifficultyDto);

            // Update region using repository

            updateWalkDifficulty = await _walkDifficultyRepository.UpdateAsync(id, updateWalkDifficulty);

            //If null return NotFound

            if (updateWalkDifficulty == null)
            {
                return NotFound();
            }

            //Convert back to DTO
            var WalkDifficultyDto = _mapper.Map<WalkDifficultyDto>(updateWalkDifficulty);
            // Return Ok response

            return Ok(WalkDifficultyDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(AddWalkDifficultyDto addWalkDifficultyDto)
        {
            // Request check
            if (!ValidateAddWalkDifficulty(addWalkDifficultyDto))
            {
                return BadRequest(ModelState);
            }

            // Request to Domain model

            var addWalkDifficulty = _mapper.Map<WalkDifficulty>(addWalkDifficultyDto);
            //Pass details to repository

            addWalkDifficulty = await _walkDifficultyRepository.AddAsync(addWalkDifficulty);

            //Convert back to Dto

            var walkingDifDto = _mapper.Map<WalkDifficultyDto>(addWalkDifficulty);

            return CreatedAtAction(nameof(GetDifById), new { id = walkingDifDto.Id }, walkingDifDto);
            // return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{Id:guid}")]
        public async Task<IActionResult> DeleteById(Guid Id)
        { 
            var deleteWalkDif = await _walkDifficultyRepository.DeleteAsync(Id);

            if (deleteWalkDif == null)
            {
                return NotFound();
            }

            var walkDifDto = _mapper.Map<WalkDifficultyDto>(deleteWalkDif);

            return Ok(walkDifDto);
        }


        #region Private Methods
        private bool ValidateAddWalkDifficulty(AddWalkDifficultyDto addWalkDifficultyDto)
        {
            if (addWalkDifficultyDto == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyDto), "Add Walk Data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalkDifficultyDto.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyDto.Code), "Code cannot be null");
            }

            if (ModelState.Count>0)
            {
                return false;
            }

            return true;

        }

        private bool ValidateUpdateWalkDifficulty(UpdateWalkDifficultyDto updateWalkDifficultyDto)
        {
            if (updateWalkDifficultyDto == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyDto), "Add Walk Data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkDifficultyDto.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyDto.Code), "Code cannot be null");
            }

            if (ModelState.Count > 0)
            {
                return false;
            }

            return true;

        }

        #endregion
    }

}

