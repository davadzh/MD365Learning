using Entities;

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
