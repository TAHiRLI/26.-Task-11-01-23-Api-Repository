using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Entities;
using Store.Core.Repositories;
using Store.Data.DAL;
using Store.Data.Migrations;
using StoreApi.Admin.Dtos.CategoryDtos;
using System.Diagnostics.Contracts;

namespace StoreApi.Admin.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    //[Authorize(Roles ="SuperAdmin, Admin")]
    [Route("admin/api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICateoryRepository _cateoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController( ICateoryRepository cateoryRepository, IMapper mapper)
        {
            this._cateoryRepository = cateoryRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll(int page = 1)
        {
            var query = _cateoryRepository.GetAll(x=> true);
            var categoriesDto = _mapper.Map<List<CategoryListItemDto>>(query.Skip((page - 1) * 4).Take(4).ToList());

            PaginatedListDto<CategoryListItemDto> model = new PaginatedListDto<CategoryListItemDto>(categoriesDto, page, 4, query.Count());
            return Ok(model);
        }
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            List<CategoryListItemDto> list = _mapper.Map<List<CategoryListItemDto>>(_cateoryRepository.GetAll(x => true).ToList());
            return Ok(list);
        }
        [HttpGet("{id}")]

        public async Task<IActionResult>  Get(int id)
        {
            var category = await _cateoryRepository.GetAsync(x=> x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            CategoryGetDto dto = _mapper.Map<CategoryGetDto>(category);

            return Ok(dto);
        }
        [HttpPost]
        public async  Task<IActionResult> Create(CategoryPostDto dto)
        {
            if (_cateoryRepository.Any(x => x.Name.ToUpper() == dto.Name.ToUpper()) )
                return BadRequest();

            Category category = _mapper.Map<Category>(dto);
            await _cateoryRepository.AddAsync(category);
            await _cateoryRepository.CommitAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult>  Edit(int id, CategoryPostDto dto)
        {
            var category = await _cateoryRepository.GetAsync(x=> x.Id == id);
            if (category == null)
            {
                return BadRequest();
            }

            if (_cateoryRepository.Any(x => x.Id != id&&x.Name.ToUpper() == dto.Name.ToUpper()))
                return BadRequest();

            category.Name = dto.Name;
            await _cateoryRepository.CommitAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult>  Delete(int id)
        {
            var category = await _cateoryRepository.GetAsync(x=> x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            _cateoryRepository.Delete(category);
            _cateoryRepository.Commit();
            return NoContent();
        }
    }
}
