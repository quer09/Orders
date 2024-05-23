﻿using Microsoft.AspNetCore.Mvc;
using Orders.BackEnd.UnitsOfWork.Interfaces;
using Orders.Shared.Entities;

namespace Orders.BackEnd.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CountriesController : GenericController<Country>
    {
        public CountriesController(IGenericUnitOfWork<Country> unitOfWork) : base(unitOfWork)
        {
        }
    }
}