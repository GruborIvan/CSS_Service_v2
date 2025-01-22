using AutoMapper;
using CssService.API.Models;
using CssService.API.Models.Authentication;
using CssService.API.Models.NarudzbinaDTOs;
using CssService.API.Models.ServiceDTOs;
using CssService.API.Models.ServisniNalog;
using CssService.Domain.Commands.Authentication;
using CssService.Domain.Commands.Narudzbina;
using CssService.Domain.Commands.Servis;
using CssService.Domain.Commands.UpdateNarudzbina;
using CssService.Domain.Models;
using CssService.Domain.Models.NarudzbinaCollections;
using CssService.Domain.Models.NarudzbinaReturn;
using CssService.Domain.Models.ServisCollections;

namespace CssService.API.Modules
{
    public class AutoMapperProfileApi : Profile
    {
        public AutoMapperProfileApi() 
        {
            CreateMap<Subject, SubjectDto>();
            CreateMap<Ident, IdentDto>();

            CreateMap<User, UserDto>();

            CreateMap<Status, StatusDto>();
            CreateMap<City, CityDto>();
            CreateMap<Referent, ReferentDto>();
            CreateMap<Skladiste, SkladisteDto>();

            CreateMap<ContactPerson, ContactPersonDto>();

            CreateMap<Masina, MasinaDto>();
            CreateMap<MasinaKorisnik, MasinaKorisnikDto>();

            CreateMap<Narudzbina, NarudzbinaDto>();

            CreateMap<NarudzbinaItem, NarudzbinaItemDto>();

            CreateMap<Servis, ServiceDto>();

            CreateMap<NarudzbinaPostModelDto, NarudzbinaPost>();
            

            CreateMap<NarudzbinaReturn, NarudzbinaReturnDto>();
            CreateMap<ServisReturn, ServiceReturnDto>();

            // Authentication mappings
            CreateMap<AuthenticationCredentials, AuthenticationCommand>();

            // Service POST mappings
            CreateMap<ServicePostDto, InsertServisCommand>();

            CreateMap<ServisAddDto, ServisAdd>()
                .ForMember(dest => dest.AdDate, src => src.MapFrom(x => DateTime.Parse(x.AdDate)))
                .ForMember(dest => dest.AdFieldDA, src => src.MapFrom(x => DateTime.Parse(x.AdFieldDA)))
                .ForMember(dest => dest.AdFieldDB, src => src.MapFrom(x => DateTime.Parse(x.AdFieldDB)))
                .ForMember(dest => dest.AdFieldDC, src => src.MapFrom(x => DateTime.Parse(x.AdFieldDC)))
                .ForMember(dest => dest.AdFieldDD, src => src.MapFrom(x => DateTime.Parse(x.AdFieldDD)));

            // DELETE?
            CreateMap<NarudzbinaPostDto, ServisAdd>();

            CreateMap<ServisItemAddDto, NarudzbinaItemPost>();

            // Narudzbina POST mappings.
            CreateMap<NarudzbinaPostModelDto, InsertNarudzbineDataCommand>();

            CreateMap<NarudzbinaPostDto, NarudzbinaPost>()
                .ForMember(dest => dest.AdDate, src => src.MapFrom(x => DateTime.Parse(x.AdDate)));

            CreateMap<NarudzbinaItemPostDto, NarudzbinaItemPost>();

            // Narudzbina update
            CreateMap<ServisniNalogDto, UpdateNarudzbinaCommand>();
        }
    }
}