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
                Background = new SolidColorBrush(Color.FromArgb(appointment.Background))
            };
        }
    }
}
