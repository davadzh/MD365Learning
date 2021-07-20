using Auto.Common.Commands;
using Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Auto.Plugins.Dav_agreement.Handlers
{
    public class Dav_agreementService
    {
        private readonly IOrganizationService _service;

        public Dav_agreementService(IOrganizationService service)
        {
            _service = service;
        }

        public void TrySetContactFirstAgreement(dav_agreement davAgreement)
        {
            var result = _service.Retrieve(Contact.EntityLogicalName, 
                                          davAgreement.dav_contact.Id, 
                                          new ColumnSet("dav_date"));
            var contact = result.ToEntity<Contact>();

            if (!ContactCommands.HasAgreements(contact))
            {
                Contact contactToUpdate = new Contact
                {
                    Id = contact.Id,
                    dav_date = davAgreement.dav_date
                };

                _service.Update(contactToUpdate);
            }
        }
    }
}
