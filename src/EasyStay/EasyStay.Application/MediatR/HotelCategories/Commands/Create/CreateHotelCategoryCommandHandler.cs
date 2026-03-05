using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.HotelCategories.Commands.Create;

public class CreateHotelCategoryCommandHandler(
	IEasyStayDbContext context
) : IRequestHandler<CreateHotelCategoryCommand, long> {

	public async Task<long> Handle(CreateHotelCategoryCommand request, CancellationToken cancellationToken) {
		var entity = new HotelCategory {
			Name = request.Name
		};

		await context.HotelCategories.AddAsync(entity, cancellationToken);
		await context.SaveChangesAsync(cancellationToken);

		return entity.Id;
	}
}
