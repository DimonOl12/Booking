using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EasyStay.Application.MediatR.Hotels.Commands.Create;

public class CreateHotelCommandHandler(
	IEasyStayDbContext context,
	IImageService imageService,
	IMediator mediator,
	ICurrentUserService currentUserService,
	ITimeConverter timeConverter
) : IRequestHandler<CreateHotelCommand, long> {

	public async Task<long> Handle(CreateHotelCommand request, CancellationToken cancellationToken) {
		var entity = new Hotel {
			Name = request.Name,
			Description = request.Description,
			ArrivalTimeUtcFrom = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(request.ArrivalTimeUtcFrom),
			ArrivalTimeUtcTo = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(request.ArrivalTimeUtcTo),
			DepartureTimeUtcFrom = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(request.DepartureTimeUtcFrom),
			DepartureTimeUtcTo = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(request.DepartureTimeUtcTo),
			IsArchived = request.IsArchived ?? false,
			HotelCategoryId = request.CategoryId,
			RealtorId = currentUserService.GetRequiredUserId(),
		};
		entity.Photos = await SaveAndPrioritizePhotosAsync(request.Photos, entity);
		entity.AddressId = await mediator.Send(request.Address, cancellationToken);

		entity.HotelHotelAmenities = (request.HotelAmenityIds ?? [])
			.Select(haId => new HotelHotelAmenity {
				Hotel = entity,
				HotelAmenityId = haId
			})
			.ToArray();

		entity.HotelBreakfasts = (request.BreakfastIds ?? [])
			.Select(bId => new HotelBreakfast {
				Hotel = entity,
				BreakfastId = bId
			})
			.ToArray();

		entity.HotelStaffLanguages = (request.StaffLanguageIds ?? [])
			.Select(lId => new HotelStaffLanguage {
				Hotel = entity,
				LanguageId = lId
			})
			.ToArray();

		await context.Hotels.AddAsync(entity, cancellationToken);

		try {
			await context.SaveChangesAsync(cancellationToken);
		}
		catch (Exception) {
			imageService.DeleteImagesIfExists(entity.Photos.Select(p => p.Name).ToArray());
			throw;
		}

		return entity.Id;
	}

	private async Task<ICollection<HotelPhoto>>
		SaveAndPrioritizePhotosAsync(IEnumerable<IFormFile> photos, Hotel hotel) {
		var images = await imageService.SaveImagesAsync(photos);

		int index = 0;
		return images
			.Select(i => new HotelPhoto {
				Hotel = hotel,
				Name = i,
				Priority = index++
			})
			.ToArray();
	}
}
