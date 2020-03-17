using MyCensus.Models.ReportIncident;
using System.Threading.Tasks;

namespace MyCensus.DataServices.Interfaces
{
    public interface IFeedbackService
    {
        Task SendIssueAsync(ReportedIssue issue);
    }
}
