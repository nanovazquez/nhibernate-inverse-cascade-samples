namespace NHibernateMappingSample.NotNullableColumnMapping.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<Product> Products { get; set; }
    }
}
