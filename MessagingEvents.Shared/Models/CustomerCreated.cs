namespace MessagingEvents.Shared.Models
{
    public class CustomerCreated
    {
        public CustomerCreated(int id, string fullName, string email, string phoneNumber, DateTime birthDate)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            PhoneNumber = phoneNumber;
            BirthDate = birthDate;
        }

        public int Id { get; }

        public string FullName { get; }

        public string Email { get; }

        public string PhoneNumber { get; }

        public DateTime BirthDate { get; }

    }
}
