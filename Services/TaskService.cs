using Microsoft.Data.SqlClient;
using System.Net.NetworkInformation;
using Test1.Contracts.Requests;
using Test1.Contracts.Responses;
using Test1.Entities;
using Test1.Exceptions;
using Test1.Services.Core;
using Task = Test1.Entities.Task;

namespace Test1.Services
{
    public class TaskService : ITaskService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public TaskService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<TeamMemberResponse> GetTeamMemberAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var checkCmd = new SqlCommand("SELECT COUNT(*) FROM TeamMember WHERE IdTeamMember = @id", connection);
            checkCmd.Parameters.AddWithValue("@id", id);
            var exists = (int)await checkCmd.ExecuteScalarAsync() > 0;
            if (!exists)
                throw new TeamMemberNotFound("Team Member not found");

            var cmdGetAssigned = @"
            SELECT *
            FROM TeamMember TM
            RIGHT JOIN Task TAssigned ON TAssigned.IdAssignedTo = TM.IdTeamMember
            JOIN TaskType TAT ON TAssigned.IdTaskType = TAT.IdTaskType
            WHERE TM.IdTeamMember = @id";

            var cmdGetCreated = @"
            SELECT *
            FROM TeamMember TM
            RIGHT JOIN Task TCreator ON TCreator.IdCreator = TM.IdTeamMember
            JOIN TaskType TCT ON TCreator.IdTaskType = TCT.IdTaskType   
            WHERE TM.IdTeamMember = @id";

            using var cmdAssigned = new SqlCommand(cmdGetAssigned, connection);
            cmdAssigned.Parameters.AddWithValue("@id", id);

            using var reader = await cmdAssigned.ExecuteReaderAsync();
            TeamMemberResponse response = null!;
            var tasksAssigned = new List<TaskResponse>();
            var tasksCreated = new List<TaskResponse>();

            while (await reader.ReadAsync())
            {
                if (response == null)
                {
                    response = new TeamMemberResponse
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        TasksAssigned = tasksAssigned,
                        TasksCreated = tasksCreated
                    };
                }

                if (!reader.IsDBNull(4))
                {
                    tasksAssigned.Add(new TaskResponse
                    {
                        Name = reader.GetString(5),
                        Description = reader.GetString(6),
                        Deadline = reader.GetDateTime(7),
                        TaskType = new TaskTypeResponse
                        {
                            Name = reader.GetString(13)
                        },
                    });
                }
            }
            await reader.CloseAsync();

            using var cmdCreated = new SqlCommand(cmdGetCreated, connection);
            cmdCreated.Parameters.AddWithValue("@id", id);

            using var readerCreated = await cmdCreated.ExecuteReaderAsync();

            while (await readerCreated.ReadAsync())
            {
                if (!readerCreated.IsDBNull(4))
                {
                    tasksCreated.Add(new TaskResponse
                    {
                        Name = readerCreated.GetString(5),
                        Description = readerCreated.GetString(6),
                        Deadline = readerCreated.GetDateTime(7),
                        TaskType = new TaskTypeResponse
                        {
                            Name = readerCreated.GetString(13)
                        },
                    });
                }
            }

            return response;
        }

        public async Task<TaskAddedResponse> AddTaskAsync(TaskRequest Task)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var checkClient = new SqlCommand("SELECT COUNT(*) FROM Project WHERE IdProject = @id", connection);
            checkClient.Parameters.AddWithValue("@id", Task.IdProject);
            if ((int)await checkClient.ExecuteScalarAsync() == 0)
                throw new ProjectNotFoundException("Project not found");

            var checkTaskType = new SqlCommand("SELECT COUNT(*) FROM TaskType WHERE IdTaskType = @id", connection);
            checkTaskType.Parameters.AddWithValue("@id", Task.IdTaskType);
            if ((int)await checkTaskType.ExecuteScalarAsync() == 0)
                throw new TaskTypeNotFoundException("Task type not found");

            var checkAssigned = new SqlCommand("SELECT COUNT(*) FROM TeamMember WHERE IdTeamMember = @id", connection);
            checkAssigned.Parameters.AddWithValue("@id", Task.IdAssignedTo);
            if ((int)await checkAssigned.ExecuteScalarAsync() == 0)
                throw new TeamMemberNotFound("Asigned team member not found");

            var checkCreator = new SqlCommand("SELECT COUNT(*) FROM TeamMember WHERE IdTeamMember = @id", connection);
            checkCreator.Parameters.AddWithValue("@id", Task.IdCreator);
            if ((int)await checkCreator.ExecuteScalarAsync() == 0)
                throw new TeamMemberNotFound("Creator not found");


            var insertTask = new SqlCommand("INSERT [dbo].[Task] ([Name], [Description], [Deadline], [IdProject], [IdTaskType], [IdAssignedTo], [IdCreator]) VALUES (@name, @description, @deadline, @projectId, @taskType, @assignedId, @creatorId) SELECT SCOPE_IDENTITY();", connection);
            insertTask.Parameters.AddWithValue("@name", Task.Name);
            insertTask.Parameters.AddWithValue("@description", Task.Description);
            insertTask.Parameters.AddWithValue("@deadline", Task.Deadline);
            insertTask.Parameters.AddWithValue("@projectId", Task.IdProject);
            insertTask.Parameters.AddWithValue("@taskType", Task.IdTaskType);
            insertTask.Parameters.AddWithValue("@assignedId", Task.IdAssignedTo);
            insertTask.Parameters.AddWithValue("@creatorId", Task.IdCreator);
            var result = await insertTask.ExecuteScalarAsync();

            return new TaskAddedResponse { Id = Convert.ToInt32(result) };
        }
    }
}
