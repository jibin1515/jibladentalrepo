"""
Builds small subsets of the Font Awesome fonts (solid 147 KB -> a few KB, brands 108 KB -> ~20 KB):

    wwwroot/assets/fonts/webfonts/fa-solid-subset.woff2
    wwwroot/assets/fonts/webfonts/fa-brands-subset.woff2
    (and the same under wwwroot/ar/)

plus tools/font-subsets.json (the unicode-range lists build-assets.mjs turns into extra @font-face rules).

The full fonts stay in the CSS as a fallback: a second @font-face with a `unicode-range` is declared after each full
one, so glyphs inside the subset come from the small file and the big file is only downloaded when a page uses an
icon that is not in the subset (e.g. a new social network picked in the admin).

Run after adding new Font Awesome icons to a public view:
    pip install fonttools brotli
    python subset-fonts.py
"""
import json
import re
from pathlib import Path

from fontTools import subset
from fontTools.ttLib import TTFont

HERE = Path(__file__).resolve().parent
PROJECT = HERE.parent / "JiblaDental"
WWWROOT = PROJECT / "wwwroot"

# Social networks that can be picked in the admin (SocialMedia.Name is rendered as `fab fa-<name>`)
BRAND_NAMES = """facebook facebook-f facebook-square instagram twitter x-twitter youtube tiktok snapchat snapchat-ghost
whatsapp linkedin linkedin-in telegram pinterest pinterest-p google threads github behance dribbble skype vimeo
vimeo-v viber wechat reddit tumblr medium flickr soundcloud spotify twitch discord""".split()

fa_css = (WWWROOT / "assets/fonts/font/font-awesome.min.css").read_text(encoding="utf-8")

# .fa-phone::before{content:"\f095"}  (aliases are separate rules or comma lists)
class_to_code = {}
for selectors, code in re.findall(r'((?:\.fa-[a-z0-9-]+(?:::?before)?,?)+)\{content:"\\([0-9a-f]{3,4})"\}', fa_css):
    for name in re.findall(r"\.fa-([a-z0-9-]+)", selectors):
        class_to_code.setdefault(name, code)

# --- icons used by the public site -------------------------------------------------------------------------------
sources = []
for folder in ("Views", "ViewComponents", "Templates"):
    sources += [p for p in (PROJECT / folder).rglob("*") if p.suffix in (".cshtml", ".cs")]
sources += [WWWROOT / "assets/js/main.js", WWWROOT / "assets/js/main2.js"]
text = "\n".join(p.read_text(encoding="utf-8", errors="ignore") for p in sources)
used_names = set(re.findall(r"\bfa-([a-z0-9-]+)", text))

# codepoints referenced from the site's own stylesheets (content:"\f105" ...)
own_css = ["style.min.css", "assets/css/style2.min.css", "assets/css/responsive.min.css", "assets/css/rsmenu-main.css",
           "assets/css/off-canvas.css", "assets/css/owl.carousel.css", "assets/css/slick.css",
           "assets/css/magnific-popup.css", "assets/css/odometer.min.css", "assets/css/animate.css"]
css_codes = set()
for name in own_css:
    css_codes |= set(re.findall(r'content:\s*["\']\\([fFeE][0-9a-fA-F]{3})["\']',(WWWROOT / name).read_text(encoding="utf-8", errors="ignore")))
css_codes = {c.lower() for c in css_codes}


def build(font_file: str, out_name: str, codes: set, folder: str = "assets/fonts/webfonts"):
    unicodes = sorted(int(c, 16) for c in codes)
    for root in (WWWROOT, WWWROOT / "ar"):
        source = root / folder / font_file
        if not source.exists():
            continue
        options = subset.Options()
        options.flavor = "woff2"
        options.layout_features = []
        options.hinting = False
        options.desubroutinize = True
        options.notdef_outline = True
        font = TTFont(str(source))
        subsetter = subset.Subsetter(options)
        subsetter.populate(unicodes=unicodes)
        subsetter.subset(font)
        target = root / folder / out_name
        font.flavor = "woff2"
        font.save(str(target))
        print(f"{target.relative_to(WWWROOT)}  {target.stat().st_size / 1024:.1f} KiB  ({len(unicodes)} glyphs)")
    return unicodes


solid_names = {n for n in used_names if n in class_to_code} - {f"{b}" for b in BRAND_NAMES if b in class_to_code and False}
solid_codes = {class_to_code[n] for n in solid_names} | css_codes
brand_codes = {class_to_code[n] for n in BRAND_NAMES if n in class_to_code}

# fa-solid also contains glyphs for brand names that happen to be used as solid icons; keep everything the site
# references. Extra glyphs that do not exist in the font are ignored by the subsetter.
solid = build("fa-solid-900.woff2", "fa-solid-subset.woff2", solid_codes)
brands = build("fa-brands-400.woff2", "fa-brands-subset.woff2", brand_codes)

ranges = lambda values: ",".join(f"U+{v:x}" for v in values)

# uicons (top-bar envelope/phone icons and the calendar icon): only the few used glyphs
uicons_css = (WWWROOT / "assets/css/uicons-regular-rounded.css").read_text(encoding="utf-8")
uicons_map = dict(re.findall(r'\.fi-rr-([a-z0-9-]+):before\s*\{\s*content:\s*"\\([0-9a-f]{3,4})"', uicons_css))
uicons_used = set(re.findall(r"fi-rr-([a-z0-9-]+)", text))
uicons = build("uicons-regular-rounded.woff2", "uicons-subset.woff2", {uicons_map[n] for n in uicons_used if n in uicons_map} | css_codes, "assets/fonts")  # css_codes: glyphs used by the site's own stylesheets (e.g. the tick before list items)

# Icon rules kept when the CSS bundle is built: every brand glyph (any social network can be chosen in the admin) plus
# the solid icons the site uses. All other .fa-*::before rules (about 1,800 unused icons) are dropped from the bundle.
brand_cmap = TTFont(str(WWWROOT / "assets/fonts/webfonts/fa-brands-400.woff2")).getBestCmap()
keep_codes = sorted({f"{c:x}" for c in brand_cmap} | {c.lower() for c in solid_codes})
(HERE / "font-subsets.json").write_text(
    json.dumps({"solid": ranges(solid), "brands": ranges(brands), "uicons": ranges(uicons), "keepCodes": keep_codes}, indent=2) + "\n",
    encoding="utf-8",
)
missing = sorted(n for n in used_names if n.startswith(("solid", "brands")) is False and n not in class_to_code)
print("fa-* names used but not in the css map (modifiers such as fa-2x are expected):", ", ".join(missing[:40]))
