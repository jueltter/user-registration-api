using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace user_registration_api.DefaultDomain.Models;

[Table("users", Schema = "public")]
public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("id")]
    public int? IdUser { get; set; }
    
    [MaxLength(short.MaxValue)]
    [Column("username")]
    public required string Username { get; set; }
    
    [MaxLength(short.MaxValue)]
    [Column("email")]
    public required string Email { get; set; }
    
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
}