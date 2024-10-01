using Infrastructure.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var esSettings = new ElasticSearchSettings();
builder.Configuration.Bind(nameof(ElasticSearchSettings), esSettings);
var esSection = builder.Configuration.GetSection(nameof(ElasticSearchSettings));
builder.Services.Configure<ElasticSearchSettings>(esSection);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
