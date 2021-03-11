﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FrontDesk.Infrastructure.Data.Sync
{
  public class CreateDoctorCommand : IRequest
  { 
    public int Id { get; set; }
    public string Name { get; set; }
  }

  public class CreateDoctorHandler : IRequestHandler<CreateDoctorCommand>
  {
    private readonly AppDbContext _dbContext;

    public CreateDoctorHandler(AppDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public async Task<Unit> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
      string insertSQLFormat = "SET IDENTITY_INSERT {0} ON\nINSERT INTO {0} (Id, Name) VALUES (@Id, @Name)\nSET IDENTITY_INSERT {0} OFF";

      string command = string.Format(insertSQLFormat, "Doctors");
      var idParam = new SqlParameter("@Id", request.Id);
      var nameParam = new SqlParameter("@Name", request.Name);
      await _dbContext.Database.ExecuteSqlRawAsync(command, idParam, nameParam);

      return Unit.Value;
    }
  }
}