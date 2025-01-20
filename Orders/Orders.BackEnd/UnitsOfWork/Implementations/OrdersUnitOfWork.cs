using Orders.BackEnd.Repositories.Interfaces;
using Orders.BackEnd.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;
using Org.BouncyCastle.Asn1.X500;

namespace Orders.BackEnd.UnitsOfWork.Implementations
{
    public class OrdersUnitOfWork : GenericUnitOfWork<Order>, IOrdersUnitOfWork
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrdersUnitOfWork(IGenericRepository<Order> repository, IOrdersRepository ordersRepository) : base(repository)
        {
            _ordersRepository = ordersRepository;
        }

        public override async Task<ActionResponse<Order>> GetAsync(int id) => await _ordersRepository.GetAsync(id);

        public async Task<ActionResponse<IEnumerable<Order>>> GetAsync(string email, PaginationDTO pagination) => await _ordersRepository.GetAsync(email, pagination);

        public async Task<ActionResponse<int>> GetTotalPagesAsync(string email, PaginationDTO pagination) => await _ordersRepository.GetTotalPagesAsync(email, pagination);

        public async Task<ActionResponse<Order>> UpdateFullAsync(string email, OrderDTO orderDTO) => await _ordersRepository.UpdateFullAsync(email, orderDTO);
    }
}