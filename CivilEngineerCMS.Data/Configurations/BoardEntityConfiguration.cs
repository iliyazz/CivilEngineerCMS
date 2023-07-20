//namespace CivilEngineerCMS.Data.Configurations
//{
//    using Microsoft.EntityFrameworkCore;
//    using Microsoft.EntityFrameworkCore.Metadata.Builders;

//    using Board = Models.Board;
//    public class BoardEntityConfiguration : IEntityTypeConfiguration<Board>
//    {
//        public void Configure(EntityTypeBuilder<Board> builder)
//        {
//            builder
//                .HasMany(b => b.Tasks)
//                .WithOne(t => t.Board)
//                .HasForeignKey(t => t.BoardId)
//                .OnDelete(DeleteBehavior.Restrict);
//        }
//    }
//}
