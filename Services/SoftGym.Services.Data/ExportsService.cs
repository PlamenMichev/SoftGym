namespace SoftGym.Services.Data
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Microsoft.EntityFrameworkCore;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Exports;

    public class ExportsService : IExportsService
    {
        private readonly IDeletableEntityRepository<WorkoutPlan> workoutRepository;

        public ExportsService(IDeletableEntityRepository<WorkoutPlan> workoutRepository)
        {
            this.workoutRepository = workoutRepository;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                return stream.ToArray();
            }
        }

        public async Task<ExportInputModel> GetExportModel(string id)
        {
            var result = await this.workoutRepository
                .All()
                .Where(x => x.Id == id)
                .Select(x => new ExportInputModel()
                {
                    Days = x.TrainingDays
                    .OrderBy(x => x.Day)
                    .Select(x => new ExportDayInputModel
                    {
                        Day = x.Day.ToString(),
                        Exercises = x.Exercises
                        .OrderBy(x => x.Exercise.Type)
                        .ThenByDescending(x => x.Exercise.Difficulty)
                        .Select(x => new ExportExerciseInputModel
                        {
                            Name = x.Exercise.Name,
                            BodyPart = x.Exercise.Type.ToString(),
                            Difficulty = x.Exercise.Difficulty.ToString(),
                            MinReps = x.MinRepsCount.ToString(),
                            MaxReps = x.MaxRepsCount.ToString(),
                        })
                        .ToList(),
                    })
                    .ToList(),
                })
                .FirstAsync();

            return result;
        }

        public byte[] Process(ExportInputModel inputModel)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                // Create Document
                using (WordprocessingDocument wordDocument =
                    WordprocessingDocument.Create(
                        mem,
                        WordprocessingDocumentType.Document))
                {
                    // Add a main document part.
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                    // Create the document structure and add some text.
                    mainPart.Document = new Document();
                    Body docBody = mainPart.Document.AppendChild(new Body());

                    var html = new StringBuilder();

                    html.AppendLine("<html>");
                    html.AppendLine("<h1 style=\"text-align: center; display: block; color: midnightblue\">Workout Plan</h1>");
                    html.AppendLine("<ol>");
                    foreach (var day in inputModel.Days)
                    {
                        html.AppendLine($"<li style=\"font-size: 25px; \">{day.Day}</li>");
                        html.AppendLine($"<ul style=\"display: block\">");
                        foreach (var exercise in day.Exercises)
                        {
                            if (exercise.BodyPart.ToLower() == "cardio")
                            {
                                html.AppendLine($"<li style=\"font-size: 20px; list-style-type: bullet\">" +
                                $"{exercise.Name}({exercise.BodyPart}) - {exercise.MinReps} - {exercise.MaxReps} minutes ({exercise.Difficulty})</li>");
                            }
                            else
                            {
                                html.AppendLine($"<li style=\"font-size: 20px; list-style-type: bullet\">" +
                                $"{exercise.Name}({exercise.BodyPart}) - {exercise.MinReps} - {exercise.MaxReps} reps ({exercise.Difficulty})</li>");
                            }
                        }

                        html.AppendLine($"</ul>");
                    }

                    html.AppendLine("</ol>");
                    html.AppendLine("</html>");

                    // Add your docx content here
                    // Create a memory stream with the HTML required
                    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(html.ToString()));

                    // Create an alternative format import part on the MainDocumentPart
                    AlternativeFormatImportPart altformatImportPart = wordDocument.MainDocumentPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html);

                    // Add the HTML data into the alternative format import part
                    altformatImportPart.FeedData(ms);

                    // Create a new altChunk and link it to the id of the AlternativeFormatImportPart
                    AltChunk altChunk = new AltChunk();
                    altChunk.Id = wordDocument.MainDocumentPart.GetIdOfPart(altformatImportPart);

                    // add the altChunk to the document
                    wordDocument.MainDocumentPart.Document.Body.Append(altChunk);

                    wordDocument.Save();
                }

                // Download File
                return mem.ToArray();
            }
        }
    }
}
