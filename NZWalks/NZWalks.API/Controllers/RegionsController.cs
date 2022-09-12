using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles ="reader")]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await _regionRepository.GetAllAsync();

            // return DTO regions
            // var regionsDto = new List<RegionDto>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDto = new RegionDto()
            //    {
            //        Id= region.Id,
            //        Name = region.Name,
            //        Code = region.Code,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population,
            //    };
            //    regionsDto.Add(regionDto);
            //});

            //dest -> source

            var regionsDto = _mapper.Map<List<RegionDto>>(regions);

            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("GetRegionByIdAsync/{Id:guid}")]
        [ActionName("GetRegionByIdAsync")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetRegionByIdAsync(Guid Id)
        { 
            var region = await _regionRepository.GetAsync(Id);

            if (region==null)
            {
                return NotFound();
            }
            var regionDto = _mapper.Map<RegionDto>(region);

            return Ok(regionDto);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddRegionAsync(AddRegionDto addRegionDto)
        {

            //Validate Request
            //if (!ValidateAddRegionAsync(addRegionDto))
            //{
            //    return BadRequest(ModelState);
            //}

            // Request to Domain model

            var region = _mapper.Map<Region>(addRegionDto);
            //Pass details to repository

           region = await _regionRepository.AddAsync(region);

            //Convert back to Dto

            var regionDto = _mapper.Map<RegionDto>(region);

            return CreatedAtAction(nameof(GetRegionByIdAsync), new { id = regionDto.Id },regionDto);
           // return Ok(regionDto);
        }

        [HttpDelete]
        [Authorize(Roles = "writer")]
        [Route("DeleteRegionById/{Id:guid}")]
        public async Task<IActionResult> DeleteRegionById(Guid Id)
        {
            // Get region from database

            var region = await _regionRepository.DeleteAsync(Id);

            // If null NotFound

            if (region == null)
            {
                return NotFound();
            };

            //Convert response to DTO

            var regionDto = _mapper.Map<RegionDto>(region);

            //Return Ok response

            return Ok(regionDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateRegion(Guid id,[FromBody] UpdateRegionDto updateRegionDto)
        {
            // Convert DTO to domain model, we are mapping source(updateRegionDto) on destination(our domain model) Region.
            // 
            //if (!ValidateUpdateRegionAsync(updateRegionDto))
            //{
            //    return BadRequest(ModelState);
            //}

            var updateRegion = _mapper.Map<Region>(updateRegionDto);

            // Update region using repository

            updateRegion = await _regionRepository.UpdateAsync(id, updateRegion);

            //If null return NotFound

            if (updateRegion==null)
            {
                return NotFound();
            }

            //Convert back to DTO
            var regionDto = _mapper.Map<RegionDto>(updateRegion);
            // Return Ok response

            return Ok(regionDto);
        }

        #region Private Methods
        private bool ValidateAddRegionAsync(AddRegionDto addRegionDto)
        {
            if (addRegionDto == null)
            {
                ModelState.AddModelError(nameof(addRegionDto), "Add Region Data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addRegionDto.Code))
            {
                ModelState.AddModelError(nameof(addRegionDto.Code),$"{nameof(addRegionDto.Code)} cannot be null or white space");
            }

            if (string.IsNullOrWhiteSpace(addRegionDto.Name))
            {
                ModelState.AddModelError(nameof(addRegionDto.Name), $"{nameof(addRegionDto.Name)} cannot be null or white space");
            }

            if (addRegionDto.Area<=0)
            {
                ModelState.AddModelError(nameof(addRegionDto.Area), $"{nameof(addRegionDto.Area)} cannot be less or 0");
            }

            if (addRegionDto.Long <= 0)
            {
                ModelState.AddModelError(nameof(addRegionDto.Long), $"{nameof(addRegionDto.Long)} cannot be less or 0");
            }

            if (addRegionDto.Lat <= 0)
            {
                ModelState.AddModelError(nameof(addRegionDto.Lat), $"{nameof(addRegionDto.Lat)} cannot be less or 0");
            }

            if (addRegionDto.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionDto.Population), $"{nameof(addRegionDto.Population)} cannot be  0");
            }

            if (ModelState.ErrorCount>0)
            {
                return false;
            }

            return true;
        }

        private bool ValidateUpdateRegionAsync(UpdateRegionDto updateRegionDto)
        {
            if (updateRegionDto == null)
            {
                ModelState.AddModelError(nameof(updateRegionDto), "Add Region Data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateRegionDto.Code))
            {
                ModelState.AddModelError(nameof(updateRegionDto.Code), $"{nameof(updateRegionDto.Code)} cannot be null or white space");
            }

            if (string.IsNullOrWhiteSpace(updateRegionDto.Name))
            {
                ModelState.AddModelError(nameof(updateRegionDto.Name), $"{nameof(updateRegionDto.Name)} cannot be null or white space");
            }

            if (updateRegionDto.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionDto.Area), $"{nameof(updateRegionDto.Area)} cannot be less or 0");
            }

            if (updateRegionDto.Long <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionDto.Long), $"{nameof(updateRegionDto.Long)} cannot be less or 0");
            }

            if (updateRegionDto.Lat <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionDto.Lat), $"{nameof(updateRegionDto.Lat)} cannot be less or 0");
            }

            if (updateRegionDto.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionDto.Population), $"{nameof(updateRegionDto.Population)} cannot be  0");
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
