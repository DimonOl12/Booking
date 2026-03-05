using MediatR;

namespace Reservio.Application.MediatR.Hotels.Commands.SetArchiveStatus;

public class SetArchiveStatusHotelCommand : IRequest {
	public long Id { get; set; }

	public bool IsArchived { get; set; }
}

