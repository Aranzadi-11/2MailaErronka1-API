using JatetxeaApi;
using JatetxeaApi.Repositorioak;
using Microsoft.OpenApi.Models;
using NHibernate;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

// Configuración de Swagger con múltiples documentos
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("inbentarioa", new OpenApiInfo
    {
        Version = "v1",
        Title = "Jatetxea API - Inbentarioa",
        Description = "Endpoints de la tabla Inbentarioa"
    });

    c.SwaggerDoc("plateren_osagaiak", new OpenApiInfo
    {
        Version = "v1",
        Title = "Jatetxea API - Plateren Osagaiak",
        Description = "Endpoints de la tabla Plateren Osagaiak"
    });

    c.SwaggerDoc("platerak", new OpenApiInfo
    {
        Version = "v1",
        Title = "Jatetxea API - Platerak",
        Description = "Endpoints de la tabla Platerak"
    });

    c.SwaggerDoc("zerbitzu_xehetasunak", new OpenApiInfo
    {
        Version = "v1",
        Title = "Jatetxea API - Zerbitzu Xehetasunak",
        Description = "Endpoints de la tabla Zerbitzu Xehetasunak"
    });

    c.SwaggerDoc("kateoria", new OpenApiInfo
    {
        Version = "v1",
        Title = "Jatetxea API - Kategoria",
        Description = "Endpoints de la tabla Kategoria"
    });

    c.SwaggerDoc("zerbitzuak", new OpenApiInfo
    {
        Version = "v1",
        Title = "Jatetxea API - Zerbitzuak",
        Description = "Endpoints de la tabla Zerbitzuak"
    });

    c.SwaggerDoc("langileak", new OpenApiInfo
    {
        Version = "v1",
        Title = "Jatetxea API - Langileak",
        Description = "Endpoints de la tabla Langileak"
    });

    c.SwaggerDoc("rolak", new OpenApiInfo
    {
        Version = "v1",
        Title = "Jatetxea API - Rolak",
        Description = "Endpoints de la tabla Rolak"
    });

    c.SwaggerDoc("mahaiak", new OpenApiInfo
    {
        Version = "v1",
        Title = "Jatetxea API - Mahaiak",
        Description = "Endpoints de la tabla Mahaiak"
    });

    c.SwaggerDoc("erreserbak", new OpenApiInfo
    {
        Version = "v1",
        Title = "Jatetxea API - Erreserbak",
        Description = "Endpoints de la tabla Erreserbak"
    });

    c.SwaggerDoc("jatetxeko_info", new OpenApiInfo
    {
        Version = "v1",
        Title = "Jatetxea API - Jatetxeko Info",
        Description = "Endpoints de la tabla Jatetxeko Info"
    });


    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        if (!apiDesc.TryGetMethodInfo(out var methodInfo))
            return false;

        var controllerName = methodInfo.DeclaringType?.Name ?? "";

        if (docName == "inbentarioa" && controllerName.Contains("InbentarioaController"))
            return true;

        if (docName == "plateren_osagaiak" && controllerName.Contains("PlaterenOsagaiakController"))
            return true;

        if (docName == "platerak" && controllerName.Contains("PlaterakController"))
            return true;

        if (docName == "zerbitzu_xehetasunak" && controllerName.Contains("ZerbitzuXehetasunakController"))
            return true;

        if (docName == "kateoria" && controllerName.Contains("KategoriaController"))
            return true;

        if (docName == "zerbitzuak" && controllerName.Contains("ZerbitzuakController"))
            return true;

        if (docName == "langileak" && controllerName.Contains("LangileakController"))
            return true;

        if (docName == "Rolak" && controllerName.Contains("RolakController"))
            return true;

        if (docName == "mahaiak" && controllerName.Contains("MahaiakController"))
            return true;

        if (docName == "erreserbak" && controllerName.Contains("ErreserbakController"))
            return true;

        if (docName == "jatetxeko_info" && controllerName.Contains("JatetxekoInfoController"))
            return true;

        return false;
    });
});

// NHibernate
builder.Services.AddSingleton<ISessionFactory>(NHibernateHelper.SessionFactory);

// Repositorios
builder.Services.AddTransient<InbentarioaRepository>();
builder.Services.AddTransient<PlaterenOsagaiakRepository>();
builder.Services.AddTransient<PlaterakRepository>();
builder.Services.AddTransient<ZerbitzuXehetasunakRepository>();
builder.Services.AddTransient<KategoriaRepository>();
builder.Services.AddTransient<ZerbitzuakRepository>();
builder.Services.AddTransient<LangileakRepository>();
builder.Services.AddTransient<RolakRepository>();
builder.Services.AddTransient<MahaiakRepository>();
builder.Services.AddTransient<ErreserbakRepository>();
builder.Services.AddTransient<JatetxekoInfoRepository>();
builder.Services.AddHostedService<JatetxeaApi.BackService.KaxaTotalaKalkulatu>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/inbentarioa/swagger.json", "JatetxeaAPI/Inbentarioa");
        c.SwaggerEndpoint("/swagger/plateren_osagaiak/swagger.json", "JatetxeaAPI/Plateren_Osagaiak");
        c.SwaggerEndpoint("/swagger/platerak/swagger.json", "JatetxeaAPI/Platerak");   
        c.SwaggerEndpoint("/swagger/zerbitzu_xehetasunak/swagger.json", "JatetxeaAPI/Zerbitzu_Xehetasunak");
        c.SwaggerEndpoint("/swagger/kateoria/swagger.json", "JatetxeaAPI/Kategoria");
        c.SwaggerEndpoint("/swagger/zerbitzuak/swagger.json", "JatetxeaAPI/Zerbitzuak");
        c.SwaggerEndpoint("/swagger/langileak/swagger.json", "JatetxeaAPI/Langileak");
        c.SwaggerEndpoint("/swagger/Rolak/swagger.json", "JatetxeaAPI/Rolak");
        c.SwaggerEndpoint("/swagger/mahaiak/swagger.json", "JatetxeaAPI/Mahaiak");
        c.SwaggerEndpoint("/swagger/erreserbak/swagger.json", "JatetxeaAPI/Erreserbak");
        c.SwaggerEndpoint("/swagger/jatetxeko_info/swagger.json", "JatetxeaAPI/Jatetxeko_Info");
        c.RoutePrefix = "swagger"; // UI en https://localhost:<puerto>/swagger
    });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.UseMiddleware<NHibernateSessionMiddleware>();
app.MapControllers();
app.Run();
