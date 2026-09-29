using Application.Interfaces.Persistence;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml;

namespace JiblaDental.Controllers
{
    public class SitemapController : Controller
    {
        private readonly IRepository<PageSettings> _pageRepo;
        private readonly IRepository<Service> _serviceRepo;
        private readonly IRepository<Team> _teamRepo;
        private readonly IRepository<Department> _departmentRepo;
        private readonly IRepository<News> _newsRepo;
        private readonly IRepository<Partner> _partnerRepo;

        public SitemapController(
            IRepository<PageSettings> pageRepo,
            IRepository<Service> serviceRepo,
            IRepository<Team> teamRepo,
            IRepository<Department> departmentRepo,
            IRepository<News> newsRepo,
            IRepository<Partner> partnerRepo)
        {
            _pageRepo = pageRepo;
            _serviceRepo = serviceRepo;
            _teamRepo = teamRepo;
            _departmentRepo = departmentRepo;
            _newsRepo = newsRepo;
            _partnerRepo = partnerRepo;
        }

        [HttpGet("/sitemap.xml")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Index()
        {
            var baseUrl = "https://www.jibladental.com";
            var today = DateTime.UtcNow.ToString("yyyy-MM-dd");

            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                OmitXmlDeclaration = false
            };

            using var memoryStream = new MemoryStream();
            using (var writer = XmlWriter.Create(memoryStream, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
                writer.WriteAttributeString("xmlns", "xhtml", null, "http://www.w3.org/1999/xhtml");

                void WriteUrl(string path, string priority, string changefreq, DateTime? modified = null)
                {
                    var fullUrl = baseUrl + (path.StartsWith("/") ? path : "/" + path);
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", fullUrl);
                    
                    writer.WriteStartElement("xhtml", "link", null);
                    writer.WriteAttributeString("rel", "alternate");
                    writer.WriteAttributeString("hreflang", "en");
                    writer.WriteAttributeString("href", fullUrl);
                    writer.WriteEndElement();

                    writer.WriteStartElement("xhtml", "link", null);
                    writer.WriteAttributeString("rel", "alternate");
                    writer.WriteAttributeString("hreflang", "ar");
                    writer.WriteAttributeString("href", fullUrl);
                    writer.WriteEndElement();

                    writer.WriteStartElement("xhtml", "link", null);
                    writer.WriteAttributeString("rel", "alternate");
                    writer.WriteAttributeString("hreflang", "x-default");
                    writer.WriteAttributeString("href", fullUrl);
                    writer.WriteEndElement();

                    writer.WriteElementString("lastmod", (modified ?? DateTime.UtcNow).ToString("yyyy-MM-dd"));
                    writer.WriteElementString("changefreq", changefreq);
                    writer.WriteElementString("priority", priority);
                    writer.WriteEndElement();
                }

                // Main home page
                WriteUrl("/", "1.0", "daily");

                // Standard pages from PageSettings
                var staticPages = new Dictionary<string, (string priority, string freq)>
                {
                    { "about", ("0.9", "weekly") },
                    { "services", ("0.9", "weekly") },
                    { "team", ("0.9", "weekly") },
                    { "video-hub", ("0.8", "weekly") },
                    { "faq", ("0.8", "weekly") },
                    { "partners", ("0.7", "monthly") },
                    { "news", ("0.9", "daily") },
                    { "loyalty", ("0.7", "monthly") },
                    { "insurance", ("0.7", "monthly") },
                    { "careers", ("0.7", "weekly") },
                    { "contact", ("0.9", "monthly") },
                    { "policy", ("0.5", "monthly") }
                };

                foreach (var (pagePath, (prio, freq)) in staticPages)
                {
                    WriteUrl("/" + pagePath, prio, freq);
                }

                // Dynamic Services
                var services = await _serviceRepo.GetAllActive();
                foreach (var service in services.Where(s => !string.IsNullOrWhiteSpace(s.PageName)))
                {
                    WriteUrl($"/services/{service.PageName.TrimStart('/')}", "0.8", "weekly", service.CreatedOn);
                }

                // Dynamic Team/Doctors
                var team = await _teamRepo.GetAllActive();
                foreach (var doctor in team.Where(t => !string.IsNullOrWhiteSpace(t.PageName)))
                {
                    var cleanSlug = doctor.PageName;
                    if (Uri.TryCreate(cleanSlug, UriKind.Absolute, out var u))
                    {
                        cleanSlug = u.AbsolutePath.TrimStart('/');
                        if (cleanSlug.StartsWith("team/", StringComparison.OrdinalIgnoreCase))
                            cleanSlug = cleanSlug.Substring(5);
                    }
                    WriteUrl($"/team/{cleanSlug.TrimStart('/')}", "0.8", "weekly", doctor.CreatedOn);
                }

                // Dynamic Departments
                var departments = await _departmentRepo.GetAllActive();
                foreach (var dep in departments.Where(d => !string.IsNullOrWhiteSpace(d.PageName)))
                {
                    WriteUrl($"/department/{dep.PageName.TrimStart('/')}", "0.7", "monthly", dep.CreatedOn);
                }

                // Dynamic News / Smile Blogs
                var news = await _newsRepo.GetAllActive();
                foreach (var item in news.Where(n => !string.IsNullOrWhiteSpace(n.PageName)))
                {
                    WriteUrl($"/news/{item.PageName.TrimStart('/')}", "0.8", "weekly", item.CreatedOn);
                }

                // Dynamic Partners
                var partners = await _partnerRepo.GetAllActive();
                foreach (var p in partners)
                {
                    WriteUrl($"/partners/{p.Id}", "0.6", "monthly", p.CreatedOn);
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            return Content(Encoding.UTF8.GetString(memoryStream.ToArray()), "application/xml", Encoding.UTF8);
        }
    }
}
