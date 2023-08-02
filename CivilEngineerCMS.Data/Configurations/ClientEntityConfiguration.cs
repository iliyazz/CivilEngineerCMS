namespace CivilEngineerCMS.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class ClientEntityConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder
                .HasMany(c => c.Projects)
                .WithOne(p => p.Client)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(c => c.IsActive)
                .HasDefaultValue(true);

            //builder.HasData(GenerateClient());
        }

   
        //private Client[] GenerateClient()
        //{
        //    ICollection<Client> clients = new HashSet<Client>();
        //    Client client;
        //    client = new Client
        //    {
        //        Id = Guid.Parse("EAD2D779-0A8A-4615-BF42-34B4B33F76A5"),
        //        FirstName = "Iordanka",
        //        LastName = "Iordanova",
        //        PhoneNumber = "+359123456710",
        //        Address = "Sofia, Vitosha Street 154",
        //        IsActive = true,
        //        UserId = Guid.Parse("42C8AA2F-AF8F-42B2-B69E-08DB88752865"),
        //    };
        //    clients.Add(client);
        //    client = new Client
        //    {
        //        Id = Guid.Parse("5C1C5BD8-C162-4246-BFAE-B381DA3B22B0"),
        //        FirstName = "Misho",
        //        LastName = "Mishev",
        //        PhoneNumber = "+359123456788",
        //        Address = "Vidin, Dunav Street 2",
        //        IsActive = true,
        //        UserId = Guid.Parse("09BEAA1D-0E57-4481-84AE-08DB807C5EDB"),
        //    };
        //    clients.Add(client);
        //    client = new Client
        //    {
        //        Id = Guid.Parse("F48628C3-13D3-486E-8D3C-B47BB288A89B"),
        //        FirstName = "Raicho",
        //        LastName = "Raichev",
        //        PhoneNumber = "+359123456751",
        //        Address = "Varna, Kraibrejna Street 139",
        //        IsActive = true,
        //        UserId = Guid.Parse("42C8AA2F-AF8F-42B2-B69E-08DB88752865"),
        //    };
        //    clients.Add(client);
        //    client = new Client
        //    {
        //        Id = Guid.Parse("BE032944-AD41-4DE3-81F8-DAAE394FA777"),
        //        FirstName = "Tosho",
        //        LastName = "Toshev",
        //        PhoneNumber = "+359123456780",
        //        Address = "Varna, Kraibrejna Street 5",
        //        IsActive = true,
        //        UserId = Guid.Parse("B19FA0F8-35C1-403E-84AF-08DB807C5EDB"),
        //    };
        //    clients.Add(client);
        //    client = new Client
        //    {
        //        Id = Guid.Parse("C0948B81-EF77-4A3E-AB7A-E2942087F7FD"),
        //        FirstName = "Tencho",
        //        LastName = "Tenchev",
        //        PhoneNumber = "+359123456891",
        //        Address = "Town Sliven, Alada Street 7",
        //        IsActive = true,
        //        UserId = Guid.Parse("6945C1FD-9E01-4F68-48FA-08DB86CAD14A"),
        //    };
        //    clients.Add(client);
        //    client = new Client
        //    {
        //        Id = Guid.Parse("F1E783E9-5932-429E-B755-E9115E506DC6"),
        //        FirstName = "Georgi",
        //        LastName = "Georgiev",
        //        PhoneNumber = "+359123456779",
        //        Address = "Town Dobroch, Hristo Botev Street 15",
        //        IsActive = true,
        //        UserId = Guid.Parse("3310B8C2-4D50-4FDA-570C-08DB823D71D7"),
        //    };
        //    clients.Add(client);
        //    client = new Client
        //    {
        //        Id = Guid.Parse("B90D4A57-7046-4201-80DE-ED5A41D7B6A1"),
        //        FirstName = "Angel",
        //        LastName = "Angelov",
        //        PhoneNumber = "+359123456892",
        //        Address = "Town Lovech, Klokotnica Street 38",
        //        IsActive = true,
        //        UserId = Guid.Parse("A3C05D98-F2A8-45F3-48FB-08DB86CAD14A"),
        //    };
        //    clients.Add(client);

        //    return clients.ToArray();
        //}
    }
}