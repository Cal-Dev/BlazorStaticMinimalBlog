using BlazorStatic;
using BlazorStaticMinimalBlog;
using BlazorStaticMinimalBlog.Components;
using Microsoft.Extensions.FileProviders;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseStaticWebAssets();

builder.Services.AddBlazorStaticService(opt => {
    //opt. //check to change the defaults
    opt.IgnoredPathsOnContentCopy.AddRange(["app.css"]);


    opt.ContentToCopyToOutput.Add(new ContentToCopy("../.github/media", "media"));
    opt.ContentToCopyToOutput.Add(new ContentToCopy("Content/Docs/media", "Content/Docs/media"));

    // add docs pages
    var docsFiles = Directory.GetFiles(Path.Combine("Content", "Blog"), "*.md").Where(x => !x.EndsWith("README.md"));//ignore readme, it is handled in Pages/Docs.razor

    foreach (var fileName in docsFiles.Select(Path.GetFileNameWithoutExtension))
    {
        opt.PagesToGenerate.Add(new PageToGenerate($"/Blogs/{fileName}", Path.Combine("Blogs", $"{fileName}.html")));
    }

    // Must add a site url to generate the Sitemap!
    opt.ShouldGenerateSitemap = true;
    opt.SiteUrl = WebsiteKeys.SiteUrl;
    opt.HotReloadEnabled = true;
}).AddBlazorStaticContentService<BlogFrontMatter>().AddBlazorStaticContentService<ProjectFrontMatter>(opt => {
    opt.MediaFolderRelativeToContentPath = null;
    opt.ContentPath = Path.Combine("Content", "Projects");
    opt.AddTagPagesFromPosts = false;
    opt.PageUrl = WebsiteKeys.ProjectsUrl;
});


// Add services to the container.
builder.Services.AddRazorComponents();
builder.Services.AddFluentUIComponents();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Content", "Blog", "media")),
    RequestPath = "/Content/Blog/media"
});

//app.UseStaticFiles(new StaticFileOptions//for readme images
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "..", ".github", "media")),
//    RequestPath = "/media"
//});



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>();

app.UseBlazorStaticGenerator(shutdownApp: !app.Environment.IsDevelopment());

app.Run();

public static class WebsiteKeys
{
    public const string GitHubRepo = "https://github.com/Cal-Dev/BlazorStaticMinimalBlog";
    public const string X = "https://x.com/";
    public const string Title = "BlazorStatic Minimal Blog";
    public const string BlogPostStorageAddress = $"{GitHubRepo}/Content/Blog";
    public const string BlogLead = "Sample blog created with BlazorStatic and TailwindCSS";
    public const string SiteUrl = "https://cal-dev.github.io/BlazorStaticMinimalBlog";

    public const string ProjectsUrl = "projects";

}
