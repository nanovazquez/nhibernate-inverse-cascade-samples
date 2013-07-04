namespace NHibernateMappingsSample.CustomMapping
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual bool Discontinued { get; set; }

        public virtual Category Category { get; set; }
    }
}
