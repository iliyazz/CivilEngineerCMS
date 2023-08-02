namespace CivilEngineerCMS.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class InteractEntityConfiguration : IEntityTypeConfiguration<Interaction>
    {
        public void Configure(EntityTypeBuilder<Interaction> builder)
        {
            builder
                .Property(i => i.Date)
                .HasDefaultValueSql("GETDATE()");
            builder
                .HasOne(i => i.Project)
                .WithMany(p => p.Interactions)
                .HasForeignKey(i => i.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.HasData(GenerateInteractions());
        }

        //private Interaction[] GenerateInteractions()
        //{
        //    ICollection<Interaction> interactions = new HashSet<Interaction>();
        //    Interaction interaction;

        //    interaction = new Interaction
        //    {
        //        Id = Guid.Parse("2B3DDA61-6817-49B1-B11D-11FE256471A5"),
        //        ProjectId = Guid.Parse("EEA8E425-92D8-4BD9-B8A7-638620F070E0"),
        //        Type = "Phone call",
        //        Description = "Talk about the roof type",
        //        Message = "The customer wants a flat usable roof  over whole house",
        //        UrlPath = null,
        //        Date = DateTime.Parse("2023-07-21 16:07:47.0524274"),
        //    };
        //    interactions.Add(interaction);
        //    interaction = new Interaction
        //    {
        //        Id = Guid.Parse("242E35DD-D1C1-4877-8DAD-1BDDAACE389E"),
        //        ProjectId = Guid.Parse("EEA8E425-92D8-4BD9-B8A7-638620F070E0"),
        //        Type = "Viber Message",
        //        Description = "A conversation about the color of the facades",
        //        Message = "The customer will send an inventory of his preferred colors.",
        //        UrlPath = null,
        //        Date = DateTime.Parse("2023-07-25 17:41:26.6123144"),
        //    };
        //    interactions.Add(interaction);
        //    interaction = new Interaction
        //    {
        //        Id = Guid.Parse("ACB187AC-FF3B-44E4-BABA-8ACF7DA718D4"),
        //        ProjectId = Guid.Parse("EEA8E425-92D8-4BD9-B8A7-638620F070E0"),
        //        Type = "Email",
        //        Description = "We received the colors for the facades from the client.",
        //        Message = "rgb(244, 224, 185), rgb(168, 161, 151)",
        //        UrlPath = null,
        //        Date = DateTime.Parse("2023-07-25 04:18:34.6622549"),
        //    };
        //    interactions.Add(interaction);
        //    interaction = new Interaction
        //    {
        //        Id = Guid.Parse("237E4CC2-8166-40A5-B69F-9723D6C13F3F"),
        //        ProjectId = Guid.Parse("EEA8E425-92D8-4BD9-B8A7-638620F070E0"),
        //        Type = "TestType",
        //        Description = "TestDescription13",
        //        Message = "Test Message",
        //        UrlPath = null,
        //        Date = DateTime.Parse("2023-07-25 07:15:31.7533333"),
        //    };
        //    interactions.Add(interaction);
        //    interaction = new Interaction
        //    {
        //        Id = Guid.Parse("311697B6-770A-4B23-8979-CF0A2C94814E"),
        //        ProjectId = Guid.Parse("EEA8E425-92D8-4BD9-B8A7-638620F070E0"),
        //        Type = "Email",
        //        Description = "Talk about the roof type",
        //        Message = "The client prefers the building to be located at the rear of the property",
        //        UrlPath = null,
        //        Date = DateTime.Parse("2023-07-25 07:23:01.7733333"),
        //    };
        //    interactions.Add(interaction);
        //    interaction = new Interaction
        //    {
        //        Id = Guid.Parse("97F586B4-7D51-4DBC-B89D-D46FB0C6C1AF"),
        //        ProjectId = Guid.Parse("EEA8E425-92D8-4BD9-B8A7-638620F070E0"),
        //        Type = "Viber message",
        //        Description = "We received the colors for the facades from the client.",
        //        Message = "rgb(244, 224, 185), rgb(168, 161, 150)",
        //        UrlPath = null,
        //        Date = DateTime.Parse("2023-07-25 07:17:40.3300000"),
        //    };
        //    interactions.Add(interaction);
        //    interaction = new Interaction
        //    {
        //        Id = Guid.Parse("1078332F-C25E-4B73-A143-FEF81C7F3703"),
        //        ProjectId = Guid.Parse("C54BC710-EFA4-4A2C-8160-812D276D5F3F"),
        //        Type = "Viber message",
        //        Description = "Telephone conversation regarding the location of the building on the property",
        //        Message = "The client prefers the building to be located at the rear of the property",
        //        UrlPath = null,
        //        Date = DateTime.Parse("2023-07-25 19:08:50.3140022"),
        //    };
        //    interactions.Add(interaction);

        //    return interactions.ToArray();
        //}
    }
}

