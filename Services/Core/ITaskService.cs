using Test1.Contracts.Requests;
using Test1.Contracts.Responses;

namespace Test1.Services.Core
{
    public interface ITaskService
    {
        Task<TeamMemberResponse> GetTeamMemberAsync(int id);
        Task<TaskAddedResponse> AddTaskAsync(TaskRequest Task);
    }
}
