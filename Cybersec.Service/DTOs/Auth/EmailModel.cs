using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace Cybersec.Service.DTOs.Auth;
public class EmailModel
{
    private string _email;
    [Email, Required]
    //[THouseEmail(ErrorMessage = "Email formati xato!")]
    public string Email { get => _email; set => _email = value?.ToLower(); }
}
