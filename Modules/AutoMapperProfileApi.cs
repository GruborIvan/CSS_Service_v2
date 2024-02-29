using AutoMapper;
using CSS_Service.API.Models;
using CSS_Service.API.Models.NarudzbinaDTOs;
using CSS_Service.API.Models.ServiceDTOs;
using CSS_Service.Domain.Commands;
using CSS_Service.Domain.Models;
using CSS_Service.Domain.Models.CollectionModels;
using CSS_Service.Domain.Models.PostModels;

namespace CSS_Service.API.Modules
{
    public class AutoMapperProfileApi : Profile
    {
        public AutoMapperProfileApi()
        {
            CreateMap<Subject, SubjectDto>();
            CreateMap<Ident, IdentDto>()
                .ForMember(dest => dest.AcWarehouse, opt => opt.NullSubstitute(String.Empty));

            CreateMap<Status, StatusDto>();
            CreateMap<User, UserDto>();
            CreateMap<City, CityDto>();
            CreateMap<Skladiste, SkladisteDto>();
            CreateMap<Referent, ReferentDto>();
            CreateMap<Masina, MasinaDto>();
            CreateMap<Service, ServiceDto>();
            CreateMap<ContactPerson, ContactPersonDto>();
            CreateMap<MasinaKorisnik, MasinaKorisnikDto>();
            CreateMap<Narudzbina, NarudzbinaDto>();
            CreateMap<NarudzbinaItem, NarudzbinaItemDto>();

            // Collections.
            CreateMap<AllDataNarudzbinaModel, NarudzbinaReturnDto>();
            CreateMap<AllDataServiceModel, ServiceReturnDto>();

            CreateMap<NarudzbinaPostModelDto, StartCreateNarudzbina>()
                .ForCtorParam("cmdBase", opt => opt.MapFrom(_ => new CmdSeed(Guid.NewGuid())));
            
            CreateMap<NarudzbinaPostDto, NarudzbinaPost>();
            CreateMap<NarudzbinaItemPostDto, NarudzbinaItemPost>();
        }
    }
}