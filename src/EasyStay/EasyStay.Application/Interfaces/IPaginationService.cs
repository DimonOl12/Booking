using EasyStay.Application.Models.Pagination;

namespace EasyStay.Application.Interfaces;

public interface IPaginationService<EntityVmType, PaginationVmType> where PaginationVmType : PaginationFilterDto {
	Task<PageVm<EntityVmType>> GetPageAsync(PaginationVmType vm, CancellationToken cancellationToken = default);
}
