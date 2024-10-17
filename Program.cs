using BlazorStatic;
using BlazorStaticMinimalBlog.Components;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseStaticWebAssets();

builder.Services.AddBlazorStaticService(opt => {
    //opt. //check to change the defaults
    opt.IgnoredPathsOnContentCopy.Add("app.css");
    opt.AddPagesWithoutParameters = true;
    opt.
    opt.MarkdownPipeline = new Markdig.MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

}
).AddBlazorStaticContentService<BlogFrontMatter>(opt =>
{
    opt.PageUrl = "blog";
    opt.TagsPageUrl = "tags";
}).po;

builder.Services.AddRazorComponents();

var app = builder.Build();

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

}
