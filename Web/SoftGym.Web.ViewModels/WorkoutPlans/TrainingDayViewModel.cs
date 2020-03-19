namespace SoftGym.Web.ViewModels.WorkoutPlans
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Services.Mapping;

    public class TrainingDayViewModel : IMapFrom<TrainingDay>, IHaveCustomMappings
    {
        public Day Day { get; set; }

        public ICollection<Exercise> ExercisesEntities { get; set; }

        public ICollection<WorkoutTrainingDay> Exercises { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<TrainingDay, TrainingDayViewModel>().ForMember(
                m => m.ExercisesEntities,
                opt => opt.MapFrom(x => x.Exercises.Select(x => x.Exercise)));
        }
    }
}
