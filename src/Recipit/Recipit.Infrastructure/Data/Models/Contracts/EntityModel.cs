namespace Recipit.Infrastructure.Data.Models.Contracts
{
    using System.ComponentModel.DataAnnotations;

    public class EntityModel
    {
        [Key]
        public int Id { get; set; }
    }
}
