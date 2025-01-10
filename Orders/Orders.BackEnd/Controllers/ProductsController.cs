using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.BackEnd.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.BackEnd.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/[controller]")]
    public class ProductsController : GenericController<Product>
    {
        private readonly IProductsUnitOfWork _productsUnitOfWork;

        public ProductsController(IGenericUnitOfWork<Product> unitOfWork, IProductsUnitOfWork productsUnitOfWork) : base(unitOfWork)
        {
            _productsUnitOfWork = productsUnitOfWork;
        }

        [HttpPost("addImages")]
        public async Task<IActionResult> PostAddImagesAsync(ImageDTO imageDTO)
        {
            var response = await _productsUnitOfWork.AddImageAsync(imageDTO);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }

        [HttpPost("removeLastImage")]
        public async Task<IActionResult> PostRemoveLastImagesAsync(ImageDTO imageDTO)
        {
            var response = await _productsUnitOfWork.RemoveLastImageAsync(imageDTO);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _productsUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }

            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _productsUnitOfWork.GetTotalPagesAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }

            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _productsUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpPost("full")]
        public async Task<IActionResult> PostFullAsync(ProductDTO productDTO)
        {
            var response = await _productsUnitOfWork.AddFullAsync(productDTO);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpPut("full")]
        public async Task<IActionResult> PutFullAsync(ProductDTO productDTO)
        {
            var response = await _productsUnitOfWork.UpdateFullAsync(productDTO);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }
    }
}