using Nexam.Business;
using Nexam.Business.Interfaces;
using Nexam.DataAccess;
using Nexam.DataAccess.Interfaces;

namespace Nexam.Infrastructure;

public static class ServiceCollectionExtensions
{
    /// <summary>Registers every layer: data access (Dapper), business services and infrastructure.</summary>
    public static IServiceCollection AddNexam(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("Email"));
        services.Configure<PdfSettings>(configuration.GetSection("Pdf"));
        services.Configure<TwilioSettings>(configuration.GetSection("Twilio"));

        services.AddMemoryCache();

        // Infrastructure
        services.AddSingleton<IPdfGenerator, IronPdfGenerator>();
        services.AddSingleton<IEmailSender, SmtpEmailSender>();
        services.AddSingleton<ISmsSender, TwilioSmsSender>();

        // Data access
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IForumRepository, ForumRepository>();
        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddScoped<INtsRepository, NtsRepository>();
        services.AddScoped<IFpscRepository, FpscRepository>();
        services.AddScoped<IPpscRepository, PpscRepository>();
        services.AddScoped<ICssRepository, CssRepository>();

        // Business
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<ISubjectService, SubjectService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IForumService, ForumService>();
        services.AddScoped<IQuizService, QuizService>();
        services.AddScoped<INtsService, NtsService>();
        services.AddScoped<IFpscService, FpscService>();
        services.AddScoped<IPpscService, PpscService>();
        services.AddScoped<ICssService, CssService>();
        services.AddScoped<IAnnouncementService, AnnouncementService>();

        return services;
    }
}
