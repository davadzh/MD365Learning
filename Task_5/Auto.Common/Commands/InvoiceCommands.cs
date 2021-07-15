using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Common.Commands
{
    public static class InvoiceCommands
    {
        public static void SetDefaultInvoiceType(dav_invoice davInvoice)
        {
            if (davInvoice.dav_type == null)
                davInvoice.dav_type = dav_invoice_dav_type.__810610000; //Вручную
        }
    }
}
