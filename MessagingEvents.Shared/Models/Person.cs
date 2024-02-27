namespace MessagingEvents.Shared.Models
{
    public class Person
    {
        public Person(string fullName, string document, DateTime birthDate)
        {
            FullName = fullName;
            Document = document;
            BirthDate = birthDate;
        }

        public string FullName { get; set; }
        public string Document { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
