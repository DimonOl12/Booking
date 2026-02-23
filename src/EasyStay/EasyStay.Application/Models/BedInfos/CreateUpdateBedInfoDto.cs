using AutoMapper;
using EasyStay.Application.Common.Mappings;
using EasyStay.Application.MediatR.BedInfos.Commands.Create;
using EasyStay.Application.MediatR.BedInfos.Commands.Update;

namespace EasyStay.Application.Models.BedInfos;

public class CreateUpdateBedInfoDto : IMapWith<CreateBedInfoCommand>, IMapWith<UpdateBedInfoCommand> {
	public int SingleBedCount { get; set; }

	public int DoubleBedCount { get; set; }

	public int ExtraBedCount { get; set; }

	public int SofaCount { get; set; }

	public int KingsizeBedCount { get; set; }



	public void Mapping(Profile profile) {
		profile.CreateMap<CreateUpdateBedInfoDto, CreateBedInfoCommand>();
		profile.CreateMap<CreateUpdateBedInfoDto, UpdateBedInfoCommand>();
	}
}
