using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Core.Repositories;
using Store.Data.DAL;
using StoreApi.Admin.Dtos.CategoryDtos;
using StoreApi.Admin.Dtos.ProductDtos;
using StoreApi.Helpers;
using System.Data;

namespace StoreApi.Admin.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    [Route("admin/api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly IProductRepository _productRepository;
        private readonly ICateoryRepository _cateoryRepository;

        public ProductsController(
            IMapper mapper,
            IWebHostEnvironment env,
            IProductRepository productRepository,
            ICateoryRepository cateoryRepository
            
            )
        {
            this._mapper = mapper;
            this._env = env;
            this._productRepository = productRepository;
            this._cateoryRepository = cateoryRepository;
        }
        [HttpGet]
        public IActionResult GetAll(int page =1 )
        {
            var query = _productRepository.GetAll(x=> true);
            var productsDto = _mapper.Map<List<ProductListItemDto>>(query.Skip((page - 1) * 4).Take(4).ToList());

            PaginatedListDto<ProductListItemDto> paginatedProducs = new PaginatedListDto<ProductListItemDto>(productsDto, page, 4, query.Count());


            return Ok(productsDto);
        }
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            List<ProductListItemDto> list = _mapper.Map<List<ProductListItemDto>>(_productRepository.GetAll((s=> true), "Category"));
            return Ok(list);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product =_productRepository.GetAll((x=> x.Id == id), "Category");
            if (product == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<ProductGetDto>(product);
            //string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/uploads/products/";
            //dto.ImgUrl = baseUrl + product.ImgUrl;

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult>  Create([FromForm] ProductPostDto dto)
        {
            if (!_cateoryRepository.Any(x => x.Id == dto.CategoryId))
            {
                return BadRequest();
            }

            Product product = _mapper.Map<Product>(dto);
            product.ImgUrl = FileManager.Save(dto.ImageFile, _env.WebRootPath, "Uploads/Products");
            await _productRepository.AddAsync(product);
            await _productRepository.CommitAsync();


            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit( int id,[FromForm] ProductPutDto dto)
        {
            var product = await _productRepository.GetAsync(x => x.Id == id);
            if (product == null)
                return BadRequest();
            if (!_cateoryRepository.Any(x => x.Id == dto.CategoryId))
                return BadRequest();

            product.Name = dto.Name;
            product.SalePrice = dto.SalePrice;
            product.CostPrice = dto.CostPrice;
            product.DiscountPercent = dto.DiscountPercent;  
            product.StockStatus = dto.StockStatus;  
            product.CategoryId = dto.CategoryId;

            if(dto.ImageFile != null)
            {
                FileManager.Delete(_env.WebRootPath, "Uploads/Product", product.ImgUrl);
                product.ImgUrl = FileManager.Save(dto.ImageFile, _env.WebRootPath, "Uploads/Products");
            }

            await _productRepository.CommitAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetAsync(x=> x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _productRepository.Delete(product);
            await _productRepository.CommitAsync();

            return NoContent();
        }

    }
}
