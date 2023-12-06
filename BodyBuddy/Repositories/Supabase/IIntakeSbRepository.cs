using BodyBuddy.Models.Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Repositories.Supabase
{
    public interface IIntakeSbRepository
    {
        Task<List<IntakeSbModel>> GetAllForProfile();
        Task AddOrUpdateIntake(IntakeSbModel intakeSbModel);
    }
}
