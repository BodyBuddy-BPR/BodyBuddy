using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Models.Supabase;
using Supabase;

namespace BodyBuddy.Repositories.Supabase.Implementation
{
    public class IntakeSbRepository : IIntakeSbRepository
    {
        private readonly Client _supabase;

        public IntakeSbRepository(Client client)
        {
            _supabase = client;
        }
        public async Task<List<IntakeSbModel>> GetAllForProfile()
        {
            try
            {
                var userId = SecureStorage.GetAsync("UserUID").Result;

                var intakeModel = await _supabase.From<IntakeSbModel>().Where(x => x.UserId == userId).Get();

                return intakeModel.Models;
            }
            catch (Exception ex)
            {
                return new List<IntakeSbModel>();
            }
        }

        public async Task AddOrUpdateIntake(IntakeSbModel intakeSbModel)
        {
            intakeSbModel.UserId = SecureStorage.GetAsync("UserUID").Result;

            await _supabase.From<IntakeSbModel>().Upsert(intakeSbModel);
        }
    }
}
