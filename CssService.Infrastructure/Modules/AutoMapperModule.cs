using AutoMapper;
using CssService.Domain.Models;
using CssService.Infrastructure.Models;

namespace CssService.Infrastructure.Modules
{
    public class AutoMapperModule : Profile
    {
        public AutoMapperModule() 
        {
            CreateMap<SubjectDbo, Subject>();
            CreateMap<IdentDbo, Ident>();

            CreateMap<SkladisteDbo, Skladiste>();

            CreateMap<StatusDbo, Status>()
                .ForMember(dest => dest.StatusName, src => src.MapFrom(x => x.AcStatus))
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.AcName));

            CreateMap<ReferentDbo, Referent>()
                .ForMember(dest => dest.FullName, src => src.MapFrom(x => $"{x.AcName} {(string.IsNullOrWhiteSpace(x.AcMiddle) ? "" : x.AcMiddle + " ")}{x.AcSurname}"));

            CreateMap<CityDbo, City>()
                .ForMember(dest => dest.AcCityName, src => src.MapFrom(x => x.AcName))
                .ForMember(dest => dest.AcOkrugName, src => src.MapFrom(x => x.AcRegion));

            CreateMap<UserDbo, User>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.AnUserChg))
                .ForMember(dest => dest.Username, src => src.MapFrom(x => x.AcUserId));

            CreateMap<MasinaKorisnikDbo, MasinaKorisnik>()
                .ForMember(dest => dest.MasinaId, src => src.MapFrom(x => x.Masina_id))
                .ForMember(dest => dest.SubjektId, src => src.MapFrom(x => x.Subjekt_id));

            CreateMap<MasinaDbo, Masina>()
                .ForMember(dest => dest.NazivMasine, src => src.MapFrom(x => x.Naziv_masine))
                .ForMember(dest => dest.SerijskiBroj, src => src.MapFrom(x => x.Serijski_broj))
                .ForMember(dest => dest.GarancijaOd, src => src.MapFrom(x => x.Garancija_od.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.GarancijaDo, src => src.MapFrom(x => x.Garancija_do.ToString("dd/MM/yyyy")));

            CreateMap<ContactPersonDbo, ContactPerson>()
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress ?? null))
                .ForMember(dest => dest.PhoneNo, opt => opt.MapFrom(src => src.PhoneNo ?? null));


            CreateMap<NarudzbinaDbo, Narudzbina>()
                .ForMember(dest => dest.AnClerk, opt => opt.MapFrom(src => src.AnClerk ?? 69))
                .ForMember(dest => dest.AcStatus, opt => opt.MapFrom(src => src.AcStatus ?? "1"))
                .ForMember(dest => dest.AdDate, src => src.MapFrom(x => x.AdDate.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.AdDateValid, src => src.MapFrom(x => x.AdDateValid.ToString("dd/MM/yyyy")));

            CreateMap<NarudzbinaItemDbo, NarudzbinaItem>()
                .ForMember(dest => dest.AnUMToUM2, src => src.MapFrom(x => x.AcUM2));

            CreateMap<ServiceDbo, Servis>()
                .ForMember(dest => dest.AdDate, src => src.MapFrom(x => x.AdDate.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.AdFieldDA, src => src.MapFrom(x => x.AdFieldDA.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.AdFieldDB, src => src.MapFrom(x => x.AdFieldDB.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.AdFieldDC, src => src.MapFrom(x => x.AdFieldDC.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.AdFieldDC, src => src.MapFrom(x => x.AdFieldDD.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.Signature, src => src.MapFrom(x => x.AcSignature));
        }
    }
}