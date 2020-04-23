namespace SoftGym.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using SoftGym.Common;
    using SoftGym.Data;
    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Data.Repositories;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Services.Mapping;
    using SoftGym.Web.ViewModels.Trainers.Appointments;
    using Xunit;

    public class AppointmentsServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> userManager;

        public AppointmentsServiceTests()
        {
            this.userManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
            new MapperInitializationProfile();
        }

        public AppointmentsService Before()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var appointmentsRepository = new EfDeletableEntityRepository<Appointment>(db);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var notificationsService = new Mock<INotificationsService>();

            var service = new AppointmentsService(
                appointmentsRepository,
                usersRepository,
                notificationsService.Object);

            return service;
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Some Random notes")]
        public async Task AddAppointmentShouldAddAppointmentSuccessfully(
            string notes)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var appointmentsRepository = new EfDeletableEntityRepository<Appointment>(db);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var notificationsService = new Mock<INotificationsService>();

            var service = new AppointmentsService(
                appointmentsRepository,
                usersRepository,
                notificationsService.Object);

            var client = new ApplicationUser();
            var trainer = new ApplicationUser();
            var role = new ApplicationRole(GlobalConstants.TrainerRoleName);
            var identityRole = new IdentityUserRole<string>()
            {
                RoleId = role.Id,
                UserId = trainer.Id,
            };
            trainer.Roles.Add(identityRole);

            await usersRepository.AddAsync(trainer);
            await usersRepository.AddAsync(client);
            await usersRepository.SaveChangesAsync();

            var inputModel = new AddAppointmentInputModel()
            {
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(2),
                ClientId = client.Id,
                TrainerId = trainer.Id,
                IsApproved = true,
                Notes = notes,
                Type = AppointmentType.Consultation,
            };

            var result = await service.AddAppoinmentAsync(inputModel);

            Assert.Equal(1, result.Id);
            Assert.Equal(notes, result.Notes);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("asfasfasf", "")]
        public async Task AddAppointmentShouldReturnNull(string clientId, string trainerId)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var appointmentsRepository = new EfDeletableEntityRepository<Appointment>(db);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var notificationsService = new Mock<INotificationsService>();

            var service = new AppointmentsService(
                appointmentsRepository,
                usersRepository,
                notificationsService.Object);

            var inputModel = new AddAppointmentInputModel()
            {
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(2),
                ClientId = clientId,
                TrainerId = trainerId,
                IsApproved = true,
                Notes = null,
                Type = AppointmentType.Consultation,
            };

            var result = await service.AddAppoinmentAsync(inputModel);

            Assert.Null(result);
        }

        [Theory]
        [InlineData(AppointmentType.Consultation)]
        [InlineData(AppointmentType.Training)]
        [InlineData(AppointmentType.Payment)]
        public async Task AddAppointmentShouldAddAppointmentWithAllTypes(
            AppointmentType type)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var appointmentsRepository = new EfDeletableEntityRepository<Appointment>(db);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var notificationsService = new Mock<INotificationsService>();

            var service = new AppointmentsService(
                appointmentsRepository,
                usersRepository,
                notificationsService.Object);

            var client = new ApplicationUser();
            var trainer = new ApplicationUser();
            var role = new ApplicationRole(GlobalConstants.TrainerRoleName);
            var identityRole = new IdentityUserRole<string>()
            {
                RoleId = role.Id,
                UserId = trainer.Id,
            };
            trainer.Roles.Add(identityRole);

            await usersRepository.AddAsync(trainer);
            await usersRepository.AddAsync(client);
            await usersRepository.SaveChangesAsync();

            var inputModel = new AddAppointmentInputModel()
            {
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(2),
                ClientId = client.Id,
                TrainerId = trainer.Id,
                IsApproved = true,
                Notes = null,
                Type = type,
            };

            var result = await service.AddAppoinmentAsync(inputModel);

            Assert.Equal(type, result.Type);
        }

        [Theory]
        [InlineData(null, null)]
        [MemberData(nameof(testDateTime))]
        public async Task AddAppointmentShouldThrowException(
            DateTime? startDate,
            DateTime? endDate)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var appointmentsRepository = new EfDeletableEntityRepository<Appointment>(db);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var notificationsService = new Mock<INotificationsService>();

            var service = new AppointmentsService(
                appointmentsRepository,
                usersRepository,
                notificationsService.Object);

            var client = new ApplicationUser();
            var trainer = new ApplicationUser();
            var role = new ApplicationRole(GlobalConstants.TrainerRoleName);
            var identityRole = new IdentityUserRole<string>()
            {
                RoleId = role.Id,
                UserId = trainer.Id,
            };
            trainer.Roles.Add(identityRole);

            await usersRepository.AddAsync(trainer);
            await usersRepository.AddAsync(client);
            await usersRepository.SaveChangesAsync();

            var inputModel = new AddAppointmentInputModel()
            {
                StartTime = startDate,
                EndTime = endDate,
                ClientId = client.Id,
                TrainerId = trainer.Id,
                IsApproved = true,
                Notes = null,
                Type = AppointmentType.Consultation,
            };

            await Assert.ThrowsAnyAsync<Exception>(async () => await service.AddAppoinmentAsync(inputModel));
        }

        [Fact]
        public void IsEndTimeSoonerThanStartTimeReturnsTrue()
        {
            var service = this.Before();

            var result = service.IsEndTimeSoonerThanStartTime(DateTime.Now, DateTime.Now.AddDays(2));

            Assert.False(result);
        }

        [Fact]
        public void IsEndTimeSoonerThanStartTimeReturnsFalse()
        {
            var service = this.Before();

            var result = service.IsEndTimeSoonerThanStartTime(DateTime.Now, new DateTime(19000000));

            Assert.True(result);
        }

        [Fact]
        public void IsStartTimePastReturnsTrue()
        {
            var service = this.Before();

            var result = service.IsStartTimePast(DateTime.UtcNow.AddDays(1));

            Assert.False(result);
        }

        [Fact]
        public void IsStartTimePastReturnsFalse()
        {
            var service = this.Before();

            var result = service.IsStartTimePast(DateTime.UtcNow);

            Assert.True(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DeleteAppointmentShouldDeleteAppointment(bool isTrainerDeleter)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var appointmentsRepository = new EfDeletableEntityRepository<Appointment>(db);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var notificationsService = new Mock<INotificationsService>();

            var service = new AppointmentsService(
                appointmentsRepository,
                usersRepository,
                notificationsService.Object);

            var client = new ApplicationUser();
            var trainer = new ApplicationUser();
            var role = new ApplicationRole(GlobalConstants.TrainerRoleName);
            var identityRole = new IdentityUserRole<string>()
            {
                RoleId = role.Id,
                UserId = trainer.Id,
            };
            trainer.Roles.Add(identityRole);

            await usersRepository.AddAsync(trainer);
            await usersRepository.AddAsync(client);
            await usersRepository.SaveChangesAsync();

            var inputModel = new AddAppointmentInputModel()
            {
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(3),
                ClientId = client.Id,
                TrainerId = trainer.Id,
                IsApproved = true,
                Notes = null,
                Type = AppointmentType.Consultation,
            };

            var appointment = await service.AddAppoinmentAsync(inputModel);
            await service.DeleteAppointment(client.Id, trainer.Id, appointment.Id, isTrainerDeleter);

            Assert.True(appointment.IsDeleted);
        }

        [Theory]
        [InlineData(null, null, -10)]
        [InlineData("", "123asfasfasf", 1)]
        [InlineData("adasdasd", "", 1)]
        public async Task DeleteAppointmentShouldThrow(string clientId, string trainerId, int appointmentId)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var appointmentsRepository = new EfDeletableEntityRepository<Appointment>(db);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var notificationsService = new Mock<INotificationsService>();

            var service = new AppointmentsService(
                appointmentsRepository,
                usersRepository,
                notificationsService.Object);

            var client = new ApplicationUser();
            var trainer = new ApplicationUser();
            var role = new ApplicationRole(GlobalConstants.TrainerRoleName);
            var identityRole = new IdentityUserRole<string>()
            {
                RoleId = role.Id,
                UserId = trainer.Id,
            };
            trainer.Roles.Add(identityRole);

            await usersRepository.AddAsync(trainer);
            await usersRepository.AddAsync(client);
            await usersRepository.SaveChangesAsync();

            var inputModel = new AddAppointmentInputModel()
            {
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(3),
                ClientId = client.Id,
                TrainerId = trainer.Id,
                IsApproved = true,
                Notes = null,
                Type = AppointmentType.Consultation,
            };

            var appointment = await service.AddAppoinmentAsync(inputModel);

            await Assert.ThrowsAnyAsync<Exception>(async () =>
                await service.DeleteAppointment(clientId, trainerId, appointmentId, true));
        }

        [Fact]
        public async Task ApproveAppointmentShouldApproveAppointmentSuccessfully()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var appointmentsRepository = new EfDeletableEntityRepository<Appointment>(db);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var notificationsService = new Mock<INotificationsService>();

            var service = new AppointmentsService(
                appointmentsRepository,
                usersRepository,
                notificationsService.Object);

            var client = new ApplicationUser();
            var trainer = new ApplicationUser();
            var role = new ApplicationRole(GlobalConstants.TrainerRoleName);
            var identityRole = new IdentityUserRole<string>()
            {
                RoleId = role.Id,
                UserId = trainer.Id,
            };
            trainer.Roles.Add(identityRole);

            await usersRepository.AddAsync(trainer);
            await usersRepository.AddAsync(client);
            await usersRepository.SaveChangesAsync();

            var inputModel = new AddAppointmentInputModel()
            {
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(3),
                ClientId = client.Id,
                TrainerId = trainer.Id,
                IsApproved = true,
                Notes = null,
                Type = AppointmentType.Consultation,
            };

            var appointment = await service.AddAppoinmentAsync(inputModel);
            await service.ApproveAppointment(appointment.Id, trainer.Id, client.Id);

            Assert.True(appointment.IsApproved);
        }

        [Theory]
        [InlineData(null, null, -2)]
        [InlineData(null, null, 1)]
        [InlineData("", "asdasf", -2)]
        public async Task ApproveAppointmentShouldThrow(string clientId, string trainerId, int appointmentId)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var appointmentsRepository = new EfDeletableEntityRepository<Appointment>(db);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var notificationsService = new Mock<INotificationsService>();

            var service = new AppointmentsService(
                appointmentsRepository,
                usersRepository,
                notificationsService.Object);

            var client = new ApplicationUser();
            var trainer = new ApplicationUser();
            var role = new ApplicationRole(GlobalConstants.TrainerRoleName);
            var identityRole = new IdentityUserRole<string>()
            {
                RoleId = role.Id,
                UserId = trainer.Id,
            };
            trainer.Roles.Add(identityRole);

            await usersRepository.AddAsync(trainer);
            await usersRepository.AddAsync(client);
            await usersRepository.SaveChangesAsync();

            var inputModel = new AddAppointmentInputModel()
            {
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(3),
                ClientId = client.Id,
                TrainerId = trainer.Id,
                IsApproved = true,
                Notes = null,
                Type = AppointmentType.Consultation,
            };

            var appointment = await service.AddAppoinmentAsync(inputModel);

            await Assert.ThrowsAnyAsync<InvalidOperationException>(async () =>
                await service.ApproveAppointment(appointmentId, trainerId, clientId));
        }

        [Fact]
        public async Task GetAppointmentRequestsForTrainerShouldReturnRightModel()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var appointmentsRepository = new EfDeletableEntityRepository<Appointment>(db);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var notificationsService = new Mock<INotificationsService>();

            var service = new AppointmentsService(
                appointmentsRepository,
                usersRepository,
                notificationsService.Object);

            var client = new ApplicationUser();
            var trainer = new ApplicationUser();
            var role = new ApplicationRole(GlobalConstants.TrainerRoleName);
            var identityRole = new IdentityUserRole<string>()
            {
                RoleId = role.Id,
                UserId = trainer.Id,
            };
            trainer.Roles.Add(identityRole);

            await usersRepository.AddAsync(trainer);
            await usersRepository.AddAsync(client);
            await usersRepository.SaveChangesAsync();

            var inputModel = new AddAppointmentInputModel()
            {
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(2),
                ClientId = client.Id,
                TrainerId = trainer.Id,
                IsApproved = false,
                Notes = "Plamen",
                Type = AppointmentType.Consultation,
            };

            var appointment = await service.AddAppoinmentAsync(inputModel);
            var result = await service.GetAppointmentRequestsForTrainer<TestAppointmentModel>(trainer.Id);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetAppointmentRequestsForTrainerShouldNotThrow()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var appointmentsRepository = new EfDeletableEntityRepository<Appointment>(db);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var notificationsService = new Mock<INotificationsService>();

            var service = new AppointmentsService(
                appointmentsRepository,
                usersRepository,
                notificationsService.Object);

            var client = new ApplicationUser();
            var trainer = new ApplicationUser();
            var role = new ApplicationRole(GlobalConstants.TrainerRoleName);
            var identityRole = new IdentityUserRole<string>()
            {
                RoleId = role.Id,
                UserId = trainer.Id,
            };
            trainer.Roles.Add(identityRole);

            await usersRepository.AddAsync(trainer);
            await usersRepository.AddAsync(client);
            await usersRepository.SaveChangesAsync();

            var inputModel = new AddAppointmentInputModel()
            {
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(2),
                ClientId = client.Id,
                TrainerId = trainer.Id,
                IsApproved = true,
                Notes = "Plamen",
                Type = AppointmentType.Consultation,
            };

            var appointment = await service.AddAppoinmentAsync(inputModel);
            var result = await service.GetAppointmentRequestsForTrainer<TestAppointmentModel>(trainer.Id);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAppointmentRequestsForTrainerShouldThrow()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var appointmentsRepository = new EfDeletableEntityRepository<Appointment>(db);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var notificationsService = new Mock<INotificationsService>();

            var service = new AppointmentsService(
                appointmentsRepository,
                usersRepository,
                notificationsService.Object);

            var client = new ApplicationUser();
            var trainer = new ApplicationUser();
            var role = new ApplicationRole(GlobalConstants.TrainerRoleName);
            var identityRole = new IdentityUserRole<string>()
            {
                RoleId = role.Id,
                UserId = trainer.Id,
            };
            trainer.Roles.Add(identityRole);

            await usersRepository.AddAsync(trainer);
            await usersRepository.AddAsync(client);
            await usersRepository.SaveChangesAsync();

            var inputModel = new AddAppointmentInputModel()
            {
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(2),
                ClientId = client.Id,
                TrainerId = trainer.Id,
                IsApproved = false,
                Notes = "Plamen",
                Type = AppointmentType.Consultation,
            };

            var appointment = await service.AddAppoinmentAsync(inputModel);

            await Assert.ThrowsAnyAsync<Exception>(async () =>
            await service.GetAppointmentRequestsForTrainer<TestAppointmentModel>(null));
        }

        public class TestAppointmentModel : IMapFrom<Appointment>
        {
            public int Id { get; set; }

            public DateTime StartTime { get; set; }
        }

        public static object[][] testDateTime =
        {
            new object[] { DateTime.Now, null },
            new object[] { null, DateTime.Now },
        };
    }
}
