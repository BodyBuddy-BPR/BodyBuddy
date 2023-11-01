﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Models;

namespace BodyBuddy.Mappers
{
    public class ExerciseRecordsMapper
    {
        public ExerciseRecordsModel MapToDatabase(ExerciseRecordsDto exerciseRecordsDto)
        {
            return new ExerciseRecordsModel
            {
                Id = exerciseRecordsDto.Id,
                ExerciseId = exerciseRecordsDto.ExerciseId,
                Set = exerciseRecordsDto.Set,
                Weight = exerciseRecordsDto.Weight,
                Reps = exerciseRecordsDto.Reps,

            };
        }

        public ExerciseRecordsDto MapToDto(ExerciseRecordsModel exerciseRecordsModel)
        {
            if (exerciseRecordsModel == null)
                return new ExerciseRecordsDto();

            return new ExerciseRecordsDto()
            {
                Id = exerciseRecordsModel.Id,
                ExerciseId = exerciseRecordsModel.ExerciseId,
                Set = exerciseRecordsModel.Set,
                Weight = exerciseRecordsModel.Weight,
                Reps = exerciseRecordsModel.Reps,
            };
        }
    }
}