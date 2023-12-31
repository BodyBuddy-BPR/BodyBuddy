﻿using BodyBuddy.ViewModels.Calendar;
using Syncfusion.Maui.Scheduler;

namespace BodyBuddy.Views.Calendar
{
    internal class CalendarBehavior : Behavior<CalenderPage>
    {
        /// <summary>
        /// The no events label.
        /// </summary>
        private Label? noEventsLabel;

        /// <summary>
        /// The appointment list view
        /// </summary>
        private ListView? appointmentListView;

        /// <summary>
        /// schedule initialize
        /// </summary>
        private SfScheduler? scheduler;

        protected override void OnAttachedTo(CalenderPage bindable)
        {
            base.OnAttachedTo(bindable);
            this.scheduler = bindable.Content.FindByName<SfScheduler>("Scheduler");
            this.noEventsLabel = bindable.Content.FindByName<Label>("noEventsLabel");
            this.appointmentListView = bindable.Content.FindByName<ListView>("appointmentListView");

            if (scheduler != null)
            {
                scheduler.ViewChanged += this.OnSchedulerViewChanged;
                scheduler.Tapped += Scheduler_Tapped;
                scheduler.SelectionChanged += Scheduler_SelectionChanged;
            }
        }

        private async void Scheduler_SelectionChanged(object? sender, SchedulerSelectionChangedEventArgs e)
        {
            var viewModel = this.scheduler.BindingContext as CalendarViewModel;

            if (e.NewValue != null && !viewModel.IsBusy && !viewModel.IsUpdatingData)
            {
                viewModel.IsUpdatingData = true;
                this.UpdateMonthAgendaViewDetails(e.NewValue.Value);
                viewModel.IsUpdatingData = false;
            }
        }

        private void Scheduler_Tapped(object? sender, SchedulerTappedEventArgs e)
        {
            if (e.Element != SchedulerElement.SchedulerCell)
            {
                return;
            }

            if (scheduler != null && e.Date != null && e.Date != scheduler.SelectedDate)
            {
                this.UpdateMonthAgendaViewDetails(e.Date.Value);
            }
        }

        public void UpdateMonthAgendaViewDetails(DateTime? tappedDate)
        {
            if (this.scheduler == null || this.noEventsLabel == null || this.appointmentListView == null || tappedDate == null)
            {
                return;
            }

            var viewModel = this.scheduler.BindingContext as CalendarViewModel;
            if (viewModel == null || tappedDate == viewModel.SelectedDate)
            {
                return;
            }

            if (tappedDate.Value.Date == DateTime.Now.Date)
            {
                viewModel.IsToday = true;
                viewModel.DateTextColor = Colors.White;
            }
            else
            {
                viewModel.IsToday = false;
                viewModel.DateTextColor = Colors.Black;
            }

            if (tappedDate != viewModel.SelectedDate)
            {
                viewModel.SelectedDate = tappedDate.Value.Date;
            }

            var appointments = viewModel.GetSelectedDateAppointments(tappedDate.Value.Date);

            if (appointments != null && appointments.Count > 0)
            {
                viewModel.SelectedDateMeetings = appointments;
                if (DeviceInfo.Platform != DevicePlatform.Android)
                {
                    this.appointmentListView.IsVisible = true;
                    this.noEventsLabel.IsVisible = false;
                }
                else
                {
                    this.appointmentListView.WidthRequest = this.scheduler.Width * 0.8;
                    this.noEventsLabel.WidthRequest = 0;
                }
            }
            else
            {
                if (DeviceInfo.Platform != DevicePlatform.Android)
                {
                    this.appointmentListView.IsVisible = false;
                    this.noEventsLabel.IsVisible = true;
                }
                else
                {
                    this.appointmentListView.WidthRequest = 0;
                    this.noEventsLabel.WidthRequest = this.scheduler.Width * 0.8;
                }
            }
        }

        private void OnSchedulerViewChanged(object? sender, SchedulerViewChangedEventArgs e)
        {
            if (e.NewView != SchedulerView.Month && this.scheduler != null)
            {
                this.scheduler.View = SchedulerView.Month;
                return;
            }

            if (this.scheduler != null && this.scheduler.SelectedDate != null && e.NewVisibleDates != null && e.NewVisibleDates.Count > 0)
            {
                //scheduler.SelectedDate = e.NewVisibleDates[0];
                scheduler.SelectedDate = DateTime.Now.Date;
            }
        }

        protected override void OnDetachingFrom(CalenderPage bindable)
        {
            base.OnDetachingFrom(bindable);

            if (this.scheduler != null)
            {
                scheduler.ViewChanged -= this.OnSchedulerViewChanged;
                scheduler.Tapped -= this.Scheduler_Tapped;
                scheduler.SelectionChanged -= this.Scheduler_SelectionChanged;
                this.scheduler = null;
            }

            if (this.noEventsLabel != null)
            {
                this.noEventsLabel = null;
            }

            if (this.appointmentListView != null)
            {
                this.appointmentListView = null;
            }
        }
    }
}
