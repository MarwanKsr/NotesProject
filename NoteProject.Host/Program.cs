using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NoteProject.Core.Configuration;
using NoteProject.Core.Domain.Identity;
using NoteProject.Data.Base;
using NoteProject.Data.Data;
using NoteProject.Data.Identity;
using NoteProject.Host.Extensions;
using NoteProject.Host.Services;
using NoteProject.Service.Identity;
using NoteProject.Service.Medias;
using NoteProject.Service.MessageSender;
using NoteProject.Service.Notes;
using NoteProject.Service.Seed;
using NoteProject.Service.Storage;
using System.Net.Mail;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var hostSettings = builder.Services.ConfigureStartupConfig<HostAppSetting>(builder.Configuration.GetSection(HostAppSetting.SECTION_NAME));
HostAppSetting.SetUpInstance(hostSettings);
var apiConfig = builder.Services.ConfigureStartupConfig<ApiConfig>(builder.Configuration.GetSection(ApiConfig.SECTION_NAME));
ApiConfig.SetUpInstance(apiConfig);

builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
          options =>
          {
              options.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultEmailProvider; 

              options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;

              options.Lockout.MaxFailedAccessAttempts = 4;

              options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);

              options.User.RequireUniqueEmail = false;

              options.SignIn.RequireConfirmedEmail = false;
              options.SignIn.RequireConfirmedPhoneNumber = false;

              options.Password.RequireDigit = true;
              options.Password.RequireLowercase = true;
              options.Password.RequireUppercase = true;
              options.Password.RequireNonAlphanumeric = false;
              options.Password.RequiredLength = 8;
          })
          .AddEntityFrameworkStores<IdentityModelContext>()
          .AddDefaultTokenProviders();

var key = Encoding.ASCII.GetBytes(apiConfig.SecretKey);

builder.Services
            .AddAuthentication()
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.Cookie.Name = ".NoteProject.Cookie";
                options.Cookie.MaxAge = TimeSpan.FromMinutes(hostSettings.SessionTimeout);
                options.ExpireTimeSpan = TimeSpan.FromMinutes(hostSettings.SessionTimeout);
                options.Cookie.IsEssential = true;
                options.SlidingExpiration = true;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".NoteProject.Session";
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.IdleTimeout = TimeSpan.FromMinutes(hostSettings.SessionTimeout);
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<IdentityModelContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

StorageSettings.SetUpInstance(builder.Services.ConfigureStartupConfig<StorageSettings>(builder.Configuration.GetSection(StorageSettings.SECTION_NAME)));

builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
builder.Services.AddScoped<IStorageServiceFactory, StorageServiceFactory>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<INoteCommandService, NoteCommandService>();
builder.Services.AddScoped<INoteQueryService, NoteQueryService>();
builder.Services.AddScoped<IApplicationUserCommandsService, ApplicationUserCommandsService>();
builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
builder.Services.AddScoped<IEmailMessageSender, EmailMessageSender>();
builder.Services.AddScoped<ApplicationUserManager>();
builder.Services.AddScoped<ApplicationSignInManager>();
builder.Services.AddScoped<DBInitializer>();

builder.Services.AddScoped<IAccountService, AccountService>();

var fromName = builder.Configuration["EmailSender:FromName"];
var host = builder.Configuration["EmailSender:SMTP:Host"];
var port = int.Parse(builder.Configuration["EmailSender:SMTP:Port"]);
var email = builder.Configuration["EmailSender:SMTP:UserName"];
var password = builder.Configuration["EmailSender:SMTP:Password"];

builder.Services
    .AddFluentEmail(email, fromName) 
    .AddSmtpSender(new SmtpClient()    
    {
        Host = host,
        Port = port,
        Credentials = new NetworkCredential(email, password),
        EnableSsl = true,
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetService<DBInitializer>();

    initializer.Seed().GetAwaiter().GetResult();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Notes}/{action=Index}");

app.Run();
