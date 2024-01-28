namespace CSS_Service.API.Models
{
    public record ContactPersonDto(
        string AcSubject,
        int AnNo,
        string AcName,
        string AcSurname,
        string EmailAddress,
        string PhoneNo
    );
}
