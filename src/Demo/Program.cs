using Application.Abstract;
using Infrastructure.Elasticsearch;
using Infrastructure.Elasticsearch.Services;
using Infrastructure.Repositories;
using Nest;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//var esSettings = new ElasticSearchSettings();
//builder.Configuration.Bind(nameof(ElasticSearchSettings), esSettings);
//var esSection = builder.Configuration.GetSection(nameof(ElasticSearchSettings));
//builder.Services.Configure<ElasticSearchSettings>(esSection);
builder.Services.AddScoped<ElasticSearchSettings>();
var es = builder.Configuration.GetSection("Elasticsearch").Get<ElasticSearchSettings>();
var settings = new ConnectionSettings(new Uri(es.Url)).DefaultIndex(es.DefaultIndex);

var client = new ElasticClient(settings);
builder.Services.AddSingleton<IElasticClient>(client);
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IElasticsearchService, ElasticsearchService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
