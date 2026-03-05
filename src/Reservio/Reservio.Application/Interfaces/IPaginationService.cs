using Reservio.Application.Models.Pagination;

namespace Reservio.Application.Interfaces;

public interface IPaginationService<EntityVmType, PaginationVmType> where PaginationVmType : PaginationFilterDto {
	Task<PageVm<EntityVmType>> GetPageAsync(PaginationVmType vm, CancellationToken cancellationToken = default);
}

