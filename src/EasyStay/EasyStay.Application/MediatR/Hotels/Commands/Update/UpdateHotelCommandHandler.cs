using AutoMapper;
using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Addresses.Commands.Update;
using EasyStay.Domain.Entities;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Hotels.Commands.Update;

public class UpdateHotelCommandHandler(
	IEasyStayDbContext context,
	IImageService imageService,
	IMediator mediator,
	IMapper mapper,
	ICurrentUserService currentUserService,
	ITimeConverter timeConverter
) : IRequestHandler<UpdateHotelCommand> {

	public async Task Handle(UpdateHotelCommand request, CancellationToken cancellationToken) {
		var entity = await context.Hotels
			.Include(h => h.Photos)
			.Include(h => h.HotelHotelAmenities)
			.Include(h => h.HotelBreakfasts)
			.Include(h => h.HotelStaffLanguages)
			.FirstOrDefaultAsync(
				h => h.Id == request.Id && h.RealtorId == currentUserService.GetRequiredUserId(),
				cancellationToken
			)
			?? throw new NotFoundException(nameof(Hotel), request.Id);

		var addressCommand = mapper.Map<UpdateAddressCommand>(request.Address);
		addressCommand.Id = entity.AddressId;
		await mediator.Send(addressCommand, cancellationToken);

		var oldPhotos = entity.Photos
			.Select(p => p.Name)
			.ToArray();

		entity.Name = request.Name;
		entity.Description = request.Description;
		entity.ArrivalTimeUtcFrom = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(request.ArrivalTimeUtcFrom);
		entity.ArrivalTimeUtcTo = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(request.ArrivalTimeUtcTo);
		entity.DepartureTimeUtcFrom = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(request.DepartureTimeUtcFrom);
		entity.DepartureTimeUtcTo = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(request.DepartureTimeUtcTo);
		entity.IsArchived = request.IsArchived;
		entity.HotelCategoryId = request.CategoryId;

		entity.Photos.Clear();
		foreach (var photo in await SaveAndPrioritizePhotosAsync(request.Photos, entity))
			entity.Photos.Add(photo);

		entity.HotelHotelAmenities.Clear();
		foreach (var hotelAmenityId in request.HotelAmenityIds ?? [])
			entity.HotelHotelAmenities.Add(new HotelHotelAmenity {
				HotelId = entity.Id,
				HotelAmenityId = hotelAmenityId
			});

		entity.HotelBreakfasts.Clear();
		foreach (var breakfastId in request.BreakfastIds ?? [])
			entity.HotelBreakfasts.Add(new HotelBreakfast {
				HotelId = entity.Id,
				BreakfastId = breakfastId
			});

		entity.HotelStaffLanguages.Clear();
		foreach (var languageId in request.StaffLanguageIds ?? [])
			entity.HotelStaffLanguages.Add(new HotelStaffLanguage {
				HotelId = entity.Id,
				LanguageId = languageId
			});

		try {
			await context.SaveChangesAsync(cancellationToken);

			imageService.DeleteImagesIfExists(oldPhotos);
		}
		catch (Exception) {
			imageService.DeleteImagesIfExists(entity.Photos.Select(p => p.Name).ToArray());
			throw;
		}
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
