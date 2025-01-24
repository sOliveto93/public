using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int UserID { get; set; }
    [Column("email")]
    public string UserEmail { get; set; }
    [Column("first_name")]
    public string UserFirstName {get; set; }
    [Column("last_name")]
    public string UserLastName {get; set; }
    [Column("password")]
    public string Password {get; set; }
    [Column("es_gerente")]
    public bool Role { get; set; }
    
    [Column("credencial")]
    public int Credential { get; set; }
}