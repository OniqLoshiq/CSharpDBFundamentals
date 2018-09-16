namespace SoftJail
{
    using AutoMapper;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ExportDto;
    using System.Globalization;
    using System.Linq;

    public class SoftJailProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public SoftJailProfile()
        {
            CreateMap<Prisoner, PrisonerDto>()
                .ForMember(conf => conf.IncarcerationDate,
                opt => opt.MapFrom(p => p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InstalledUICulture)));

            CreateMap<Mail, MailDto>()
                .ForMember(conf => conf.Description,
                opt => opt.MapFrom(m => new string(m.Description.ToCharArray().Reverse().ToArray())));
        }
    }
}
