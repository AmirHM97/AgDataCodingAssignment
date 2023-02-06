
namespace AgDataCodingAssignment.Application.Features.User.Queries
{
    public class GetOneUserQueryResponseModel
    {
        public AgDataCodingAssignment.Domain.Entities.User User { get; set; }

        public GetOneUserQueryResponseModel(Domain.Entities.User user)
        {
            User = user;
        }
    }
}
