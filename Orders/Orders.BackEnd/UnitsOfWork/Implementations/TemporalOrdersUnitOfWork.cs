using Orders.BackEnd.Repositories.Interfaces;
using Orders.BackEnd.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.BackEnd.UnitsOfWork.Implementations
{
    public class TemporalOrdersUnitOfWork : GenericUnitOfWork<TemporalOrder>, ITemporalOrdersUnitOfWork
    {
        private readonly ITemporalOrdersUnitOfWork _temporalOrdersUnitOfWork;
        private readonly ITemporalOrdersRepository _temporalOrdersRepository;

        public TemporalOrdersUnitOfWork(ITemporalOrdersUnitOfWork temporalOrdersUnitOfWork, ITemporalOrdersRepository temporalOrdersRepository) : base(temporalOrdersUnitOfWork)
        {
            _temporalOrdersUnitOfWork = temporalOrdersUnitOfWork;
            _temporalOrdersRepository = temporalOrdersRepository;
        }

        public async Task<ActionResponse<TemporalOrderDTO>> AddFullAsync(string email, TemporalOrderDTO temporalOrderDTO) => await _temporalOrdersRepository.AddFullAsync(email, temporalOrderDTO);

        public async Task<ActionResponse<IEnumerable<TemporalOrder>>> GetAsync(string email) => await _temporalOrdersRepository.GetAsync(email);

        public async Task<ActionResponse<int>> GetCountAsync(string email) => await _temporalOrdersRepository.GetCountAsync(email);
    }
}