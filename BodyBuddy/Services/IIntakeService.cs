using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IIntakeService
    {
        Task<IntakeDto> GetIntakeAsync();

        Task SaveChangesAsync(IntakeDto intakeDetails);
    }
}
