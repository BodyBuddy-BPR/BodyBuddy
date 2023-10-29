﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;

namespace BodyBuddy.Services.Implementations
{
    public class IntakeService : IIntakeService
    {
        private readonly IIntakeRepository _repo;
        private readonly IntakeMapper _mapper = new();

        public IntakeService(IIntakeRepository intakeRepository)
        {
            _repo = intakeRepository;
        }
        public async Task<IntakeDto> GetIntakeAsync()
        {
            return _mapper.MapToDto(await _repo.GetIntakeAsync());
        }

        public async Task SaveChangesAsync(IntakeDto intakeDetails)
        {
            await _repo.SaveChangesAsync(_mapper.MapToDatabase(intakeDetails));
        }
    }
}