using AutoMapper;
using CssService.Domain.Interfaces;
using CssService.Domain.Models;
using CssService.Infrastructure.Models;
using CssService.Infrastructure.Transactions;
using Dapper;
using Microsoft.Extensions.Logging;

namespace CssService.Infrastructure.Repositories
{
    public class ContactPersonRepository : IContactPersonRepository
    {
        private readonly TransactionManagerService _transactionManager;
        private readonly ILogger<ContactPersonRepository> _logger;
        private readonly IMapper _mapper;

        public ContactPersonRepository(
            TransactionManagerService transactionManager, 
            ILogger<ContactPersonRepository> logger, 
            IMapper mapper)
        {
            _transactionManager = transactionManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task AddContactPersonAsync(string name, string subject, string email, string phoneNo)
        {
            (string firstName, string lastName) = SplitContactNameAndSurname(name);

            var query = $@"
                DECLARE @anNo INT;
                DECLARE @anUserId INT;
                DECLARE @Num INT;
                DECLARE @anSubNo INT;
                
                SELECT @anNo = ISNULL(MAX(anNo), 0) FROM tHE_SetSubjContact WHERE acSubject = '{subject}'; 
                SELECT @anUserId = ISNULL(MAX(anUserId), 0) FROM tHE_SetSubjContact;
                
                SELECT @Num = COUNT(*) FROM tHE_SetSubjContact
                WHERE acSubject = '{subject}' AND acName = '{firstName}' AND acSurname = '{lastName}';
                
                IF @Num = 0
                BEGIN
                	 SET @anNo = @anNo + 1;
                	 SET @anUserId = @anUserId + 1;
                
                	 INSERT INTO tHE_SetSubjContact(acSubject, anNo, acName, acSurname, anUserId)
                	 VALUES 
                			('{subject}', @anNo, '{firstName}', '{lastName}', @anUserId);

                     SELECT @anSubNo = ISNULL(MAX(anSubNo), 0) FROM tHE_SetSubjContactAddress
                     WHERE acSubject = '{subject}'
                     AND anNo = @anNo;

	                 SET @anSubNo = @anSubNo + 1;

	                 IF '{email}' IS NOT NULL AND '{email}' <> ''
	                 BEGIN
	                 	INSERT INTO tHE_SetSubjContactAddress(acSubject, anNo, anSubno, acType, acPhone)
	                 	VALUES 
	                 			('{subject}', @anNo, @anSubNo, 'E', '{email}');

	                 		 SET @anSubNo = @anSubNo + 1;
	                 END



	                 IF '{phoneNo}' IS NOT NULL AND '{phoneNo}' <> ''
	                 BEGIN
	                 		INSERT INTO tHE_SetSubjContactAddress(acSubject, anNo, anSubno, acType, acPhone)
	                 		VALUES 
	                 				('{subject}', @anNo, @anSubNo, '6', '{phoneNo}');
	                 END
                END
            ";

            try
            {
                var transaction = _transactionManager.GetCurrentTransaction();
                var connection = transaction.Connection;

                await connection.ExecuteAsync(query, transaction: transaction);
                _logger.LogInformation($"Successfully saved contact person.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Adding Contact person failed. \n {ex}");
                _transactionManager.Rollback();
                _transactionManager.DisposeTransaction();
                throw;
            }
        }

        public async Task<IEnumerable<ContactPerson>> GetAllContactPersonsAsync()
        {
            var query = $"SELECT acSubject,anNo,acName,acSurname FROM tHE_SetSubjContact";

            var transaction = _transactionManager.GetCurrentTransaction();
            var connection = transaction.Connection;

            var contactsDbo = await connection.QueryAsync<ContactPersonDbo>(query, transaction: transaction);
            var contacts = _mapper.Map<IEnumerable<ContactPerson>>(contactsDbo);

            var acSubjectList = contacts.Select(s => s.AcSubject);
            var contactInfoList = await GetContactInfoForSubjectAsync(acSubjectList);

            foreach (var contact in contacts)
            {
                (string email, string phoneNo) = FindContactInformation(contact, contactInfoList);

                contact.EmailAddress = email;
                contact.PhoneNo = phoneNo;
            }

            return contacts;
        }

        private (string email, string phoneNo) FindContactInformation(ContactPerson contactPerson, List<ContactInfoDbo> contactInformationList)
        {
            string email = string.Empty;
            string phoneNo = string.Empty;

            var contactInfos = contactInformationList
                                .Where(x => x.AcSubject == contactPerson.AcSubject)
                                .Where(x => x.AnNo == contactPerson.AnNo);

            foreach (var contactInfo in contactInfos)
            {
                if (contactInfo.AcType == "E")
                    email = contactInfo.AcPhone;
                else if (contactInfo.AcType == "6")
                    phoneNo = contactInfo.AcPhone;
            }

            return (email, phoneNo);
        }

        private async Task<List<ContactInfoDbo>> GetContactInfoForSubjectAsync(IEnumerable<string> acSubjectList)
        {
            var query = @"
                SELECT sbj.acSubject, sbj.AnNo, acPhone,acType
                FROM tHE_SetSubjContact sbj
                LEFT JOIN tHE_SetSubjContactAddress sbjAdrs
                ON sbj.acSubject = sbjAdrs.acSubject AND sbj.AnNo = sbjAdrs.anNo
                WHERE sbj.acSubject IN @acSubject
            ";

            var transaction = _transactionManager.GetCurrentTransaction();
            var connection = transaction.Connection;

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(new
            {
                acSubject = acSubjectList
            });

            var contactInfo = await connection.QueryAsync<ContactInfoDbo>(query, parameters, transaction: transaction);
            return contactInfo.ToList();
        }

        private (string name, string surname) SplitContactNameAndSurname(string fullName)
        {
            string[] slicedNames = fullName.Split(' ');
            string firstName = slicedNames[0] ?? "";
            string lastName = slicedNames[1] ?? "";
            return (firstName, lastName);
        }
    }
}
