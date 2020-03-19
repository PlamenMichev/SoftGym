namespace SoftGym.Web.ViewModels.WorkoutPlans
{
    using System.Collections.Generic;

    using AutoMapper;

    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Services.Mapping;

    public class DetailsWorkoutPlanViewModel : IMapFrom<WorkoutPlan>, IHaveCustomMappings
    {
        public int DaysInWeek { get; set; }

        public Difficulty Difficulty { get; set; }

        public ICollection<TrainingDayViewModel> TrainingDaysExercises { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<WorkoutPlan, DetailsWorkoutPlanViewModel>().ForMember(
                m => m.TrainingDaysExercises,
                opt => opt.MapFrom(x => x.TrainingDays));
        }
    }
}
