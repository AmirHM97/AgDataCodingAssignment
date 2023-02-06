using System.ComponentModel.DataAnnotations;

namespace AgDataCodingAssignment.Web.Api.ApiModels.User
{
    public class CreateUserViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address{ get; set; }
    }
}
