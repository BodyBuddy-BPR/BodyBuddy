using BodyBuddy.Dtos;
using BodyBuddy.Models;
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Mappers
{
    public class AppointmentMapper
    {
        public AppointmentDto MapToDto(AppointmentModel appointment)
        {
            if (appointment == null)
                return new AppointmentDto();

            return new AppointmentDto()
            {
                Id = appointment.Id,
                Date = DateTime.Parse(appointment.Date),
                EventName = appointment.EventName,
                From = DateTime.Parse(appointment.From),
                To = DateTime.Parse(appointment.To),
                Background = new SolidColorBrush(Color.FromArgb(appointment.Background)),
                Workout = appointment.WorkoutId != 0 ? new WorkoutDto() { Id = appointment.WorkoutId } : null
            };
        }

        public AppointmentModel MapToModel(AppointmentDto newEvent)
        {
            return new AppointmentModel()
            {
                Id = newEvent.Id,
                Date = newEvent.Date.ToString(),
                EventName = newEvent.EventName,
                From = newEvent.From.ToString(),
                To = newEvent.To.ToString(),
                Background = ConvertBrushToString(newEvent.Background),
                WorkoutId = newEvent.Workout?.Id ?? 0
            };
        }

        private string ConvertBrushToString(Brush brush)
        {
            if (brush is SolidColorBrush solidColorBrush)
            {
                // Assuming you want to store the color as a hex string
                return solidColorBrush.Color.ToHex();
            }

            // Handle other types of brushes or return a default string
            return "DefaultBackgroundString";
        }
    }
}
