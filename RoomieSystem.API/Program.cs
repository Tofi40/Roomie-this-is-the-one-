using RoomieSystem.Model.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();   // required for Swagger
builder.Services.AddSwaggerGen();             // no custom OpenApiModels

builder.Services.AddScoped<RoomRepository, RoomRepository>();
builder.Services.AddScoped<UserRepository, UserRepository>();
builder.Services.AddScoped<RoomPhotoRepository, RoomPhotoRepository>();
builder.Services.AddScoped<RoomSwipeRepository, RoomSwipeRepository>();
builder.Services.AddScoped<UserLikeRepository, UserLikeRepository>();
builder.Services.AddScoped<MatchRepository, MatchRepository>();
builder.Services.AddScoped<MessageRepository, MessageRepository>();

var app = builder.Build();

// Do NOT use HTTPS redirection (your teacher removes it)
// app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // serves /swagger, auto endpoint /swagger/v1/swagger.json
}

app.MapControllers();
app.Run();
