using Reservio.Application.Common.Mappings;
using Reservio.Domain.Entities;

namespace Reservio.Application.MediatR.Citizenships.Queries.Shared;

public class CitizenshipVm : IMapWith<Citizenship> {
	public long Id { get; set; }

	public string Name { get; set; } = null!;
}

