using Entities;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Common.Commands
{
    public static class ContactCommands
    {
        public static bool HasAgreements(Contact contact)
        {
            return contact.dav_date != null;
        }
    }
}
