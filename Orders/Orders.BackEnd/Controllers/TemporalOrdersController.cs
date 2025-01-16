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
    public class TemporalOrdersController : GenericController<TemporalOrder>
    {
        private readonly ITemporalOrdersUnitOfWork _temporalOrdersUnitOfWork;

        public TemporalOrdersController(IGenericUnitOfWork<TemporalOrder> unitOfWork, ITemporalOrdersUnitOfWork temporalOrdersUnitOfWork) : base(unitOfWork)
        {
            _temporalOrdersUnitOfWork = temporalOrdersUnitOfWork;
        }

        [HttpPost("full")]
        public async Task<IActionResult> PostAsync(TemporalOrderDTO temporalOrderDTO)
        {
            var response = await _temporalOrdersUnitOfWork.AddFullAsync(User.Identity!.Name!, temporalOrderDTO);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }

        [HttpGet("my")]
        public override async Task<IActionResult> GetAsync()
        {
            var response = await _temporalOrdersUnitOfWork.GetAsync(User.Identity!.Name!);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCountAsync()
        {
            var response = await _temporalOrdersUnitOfWork.GetCountAsync(User.Identity!.Name!);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }
    }
}