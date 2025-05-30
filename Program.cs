using System.Security.Claims;
using Hotel_Management.Models;
using Hotel_Management.Services.Interfaces;
using Hotel_Management.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Areas.Admin.Services;
using Hotel_Management;
using Hotel_Management.Mapping;
using Microsoft.AspNetCore.SignalR;
using Hotel_Management.Providers;
using Hotel_Management.Hubs;

var builder = WebApplication.CreateBuilder(args);
var mailSettings = builder.Configuration.GetSection("MailSettings");

// Add services to the container.
builder.Services.AddOptions();

// Add SendMail service
builder.Services.Configure<MailSettings>(mailSettings);
builder.Services.AddTransient<SendMailService>();
builder.Services.AddTransient<ISendMailService>(sp => sp.GetRequiredService<SendMailService>());
builder.Services.AddTransient<IEmailSender>(sp => sp.GetRequiredService<SendMailService>());

// Other services
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IRoomsService, RoomsService>();
builder.Services.AddScoped<IHotelservicesService, HotelservicesService>();
// Đăng ký các dịch vụ cho các lớp trong Admin Area
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<ICustomerServices, CustomerServices>();
builder.Services.AddScoped<IStaffServices, StaffServices>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookingServices, BookingServices>();
builder.Services.AddScoped<IVNPayServices, VNPayServices>();
builder.Services.AddDbContext<HotelManagementContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'HotelManagementContext' not found."),
        new MySqlServerVersion(new Version(8, 0, 21)))
    .EnableSensitiveDataLogging()
);

// Đăng ký SignalR
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();

// Đăng ký Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<HotelManagementContext>()
    .AddDefaultTokenProviders();

// Cấu hình cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/dang-nhap"; // Đường dẫn đến trang đăng nhập
    options.LogoutPath = "/dang-xuat"; // Đường dẫn đến trang đăng xuất
    options.AccessDeniedPath = "/access-denied.html"; // Đường dẫn đến trang không có quyền truy cập
    options.SlidingExpiration = true; // Kích hoạt tính năng gia hạn thời gian hết hạn cookie
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Thời gian hết hạn cookie
});

// Xác thực bằng dịch vụ bên thứ ba (Google, Facebook, v.v.)
builder.Services.AddAuthentication()
    .AddGoogle(option =>
    {
        var g = builder.Configuration.GetSection("Authentication:Google");
        option.ClientId = g["ClientId"] ?? throw new InvalidOperationException("Google ClientId is not configured.");
        option.ClientSecret = g["ClientSecret"] ?? throw new InvalidOperationException("Google ClientSecret is not configured.");
        option.CallbackPath = "/dang-nhap-tu-google";
        option.ClaimActions.MapJsonKey("picture", "picture", "url");

        option.Events.OnCreatingTicket = ctx =>
        {
            var picture = ctx.User.GetProperty("picture").GetString();
            if (picture != null)
            {
                ctx.Identity?.AddClaim(new Claim("picture", picture));
            }
            return Task.CompletedTask;
        };
    })
    .AddFacebook(option =>
    {
        var f = builder.Configuration.GetSection("Authentication:Facebook");
        option.AppId = f["AppId"] ?? throw new InvalidOperationException("Facebook AppId is not configured.");
        option.AppSecret = f["AppSecret"] ?? throw new InvalidOperationException("Facebook AppSecret is not configured.");
        option.CallbackPath = "/dang-nhap-tu-facebook";
        option.Fields.Add("picture"); // yêu cầu thêm thông tin ảnh
        option.Events.OnCreatingTicket = ctx =>
        {
            var picture = ctx.User.GetProperty("picture").GetProperty("data").GetProperty("url").GetString();
            if (picture != null)
            {
                ctx.Identity?.AddClaim(new Claim("picture", picture));
            }
            return Task.CompletedTask;
        };
    });

// Truy cập IdentityOptions
builder.Services.Configure<IdentityOptions>(options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = false;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedAccount = false;         // Cấu hình xác thực tài khoản (tài khoản phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại

});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedRolesAndAdminAsync(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapHub<NotificationHub>("/notificationHub");

// Đăng ký các route cho areas Admin
app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.Run();
