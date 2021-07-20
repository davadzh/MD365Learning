using Auto.Common.Commands;
using Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Auto.Plugins.Dav_invoice.Handlers
{
    public class Dav_invoiceService
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;

        public Dav_invoiceService(IOrganizationService service,
                                  ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;
        }


        public void SetDefaultInvoiceType(dav_invoice davInvoice)
        {
            InvoiceCommands.SetDefaultInvoiceType(davInvoice);
        }


        public void SetAgreementAmountDependingOnStatusOnUpdate(dav_invoice davInvoice)
        {
            if (davInvoice.dav_fact == true)
            {
                var davInvoiceEntity = _service.Retrieve(dav_invoice.EntityLogicalName, davInvoice.Id, new ColumnSet("dav_amount", "dav_dogovorid"));
                var davOldInvoice = davInvoiceEntity.ToEntity<dav_invoice>();

                var increase = davInvoice?.dav_amount?.Value ?? davOldInvoice.dav_amount.Value;

                var davAgreementEntity = _service.Retrieve(dav_agreement.EntityLogicalName, davOldInvoice.dav_dogovorid.Id, new ColumnSet("dav_amount", "dav_factsumma"));

                var davAgreement = davAgreementEntity.ToEntity<dav_agreement>();

                var newFactSumma = davAgreement.dav_factsumma.Value + increase;

                bool isPaid = false;

                if (newFactSumma > davAgreement.dav_amount.Value)
                    throw new InvalidPluginExecutionException("Общая оплаченная сумма не должна превышать сумму в договоре");
                else if (newFactSumma == davAgreement.dav_amount.Value)
                    isPaid = true;

                var agreementToUpdate = new dav_agreement
                {
                    Id = davOldInvoice.dav_dogovorid.Id,
                    dav_factsumma = new Money(newFactSumma),
                    dav_fact = isPaid
                };

                _service.Update(agreementToUpdate);
            }
        }

        public void SetAgreementAmountDependingOnStatusOnCreate(dav_invoice davInvoice)
        {
            if (davInvoice.dav_fact == true)
            {
                var increase = davInvoice?.dav_amount?.Value ?? 0;

                var davAgreementEntity = _service.Retrieve(dav_agreement.EntityLogicalName, davInvoice.dav_dogovorid.Id, new ColumnSet("dav_amount", "dav_factsumma"));

                var davAgreement = davAgreementEntity.ToEntity<dav_agreement>();

                var newFactSumma = davAgreement.dav_factsumma.Value + increase;

                bool isPaid = false;

                if (newFactSumma > davAgreement.dav_amount.Value)
                    throw new InvalidPluginExecutionException("Общая оплаченная сумма не должна превышать сумму в договоре");
                else if (newFactSumma == davAgreement.dav_amount.Value)
                    isPaid = true;

                var agreementToUpdate = new dav_agreement
                {
                    Id = davInvoice.dav_dogovorid.Id,
                    dav_factsumma = new Money(newFactSumma),
                    dav_fact = isPaid
                };

                _service.Update(agreementToUpdate);
            }
        }

    }
}
