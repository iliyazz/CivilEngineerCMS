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
        //        Id = Guid.Parse("8058BDA4-C0FB-44D3-B3B6-E66619CEC1AB"),
        //        FirstName = "Pesho",
        //        LastName = "Peshev",
        //        PhoneNumber = "+359123456788",
        //        Address = "Plovdiv1",
        //        UserId = Guid.Parse("60376974-E414-4277-1D75-08DB7A21C396"),
        //    };
        //    clients.Add(client);
        //    client = new Client
        //    {
        //        Id = Guid.Parse("1AE4584F-5611-4870-BAA7-CB0E7EDCC572"),
        //        FirstName = "Gosho",
        //        LastName = "Goshev",
        //        PhoneNumber = "+359123456787",
        //        Address = "Plovdiv1",
        //        UserId = Guid.Parse("4F318431-568E-4CDF-9A58-08DB7A22EBD9")
        //    };
        //    clients.Add(client);
        //    return clients.ToArray();
        //}
    }
}