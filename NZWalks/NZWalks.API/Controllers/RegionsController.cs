using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> AddRegionAsync(AddRegionDto addRegionDto)
        {
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
        public async Task<IActionResult> UpdateRegion(Guid id,[FromBody] UpdateRegionDto updateRegionDto)
        {
            // Convert DTO to domain model, we are mapping source(updateRegionDto) on destination(our domain model) Region. 

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
    }
}
