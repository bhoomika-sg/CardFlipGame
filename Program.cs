var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); // Add controller support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseDefaultFiles(); // enables default loading of index.html
app.UseStaticFiles();  // allows serving CSS/JS/images

app.UseRouting();
app.UseAuthorization();

app.MapControllers();   // Use API controllers

app.Run();
