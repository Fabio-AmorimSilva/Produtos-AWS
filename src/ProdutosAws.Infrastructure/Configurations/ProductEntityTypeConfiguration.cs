namespace ProdutosAws.Infrastructure.Configurations;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .ToTable("Products");

        builder
            .HasKey(p => p.Id);

        builder
            .HasIndex(p => p.Id);

        builder
            .Property(p => p.Id)
            .ValueGeneratedNever();
        
        builder
            .Property(p => p.Name)
            .HasMaxLength(Product.NameMaxLength)
            .IsRequired();
        
        builder
            .Property(p => p.Category)
            .IsRequired();

        builder
            .OwnsMany(p => p.Images, imageBuilder =>
            {
                imageBuilder
                    .Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                imageBuilder
                    .Property(i => i.Path)
                    .IsRequired();
            });
    }
}