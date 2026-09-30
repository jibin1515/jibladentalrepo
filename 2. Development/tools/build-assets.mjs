// Builds the bundled front-end assets served by Views/Shared/_Layout.cshtml:
//   wwwroot/assets/dist/site.min.css + site.core/plugins/app.min.js        (English / LTR)
//   wwwroot/ar/assets/dist/site.min.css + site.core/plugins/app.min.js     (Arabic / RTL)
//
// Run after editing any of the source CSS/JS files listed below:
//   cd "2. Development/tools" && npm install && npm run build
//
// What it does
//  - concatenates the 12 render-blocking stylesheets into one file and minifies it
//  - rewrites relative url(...) references to absolute /assets/... paths (fonts, images)
//  - icon fonts (Font Awesome, uicons, Flaticon) use font-display:block so icons never render as fallback glyphs
//  - purges the unused utility classes from rs-spacing.css (253 KB, almost all unused)
//  - concatenates the 13 scripts into 3 files (core / plugins / app), minifying main.js / main2.js, and drops sourceMappingURL comments
import { readFileSync, writeFileSync, mkdirSync, readdirSync, statSync } from 'node:fs';
import { dirname, join, posix, relative, resolve, sep } from 'node:path';
import { fileURLToPath } from 'node:url';
import { transform } from 'esbuild';
import { PurgeCSS } from 'purgecss';
import { addFontSubsets, purgeFaIcons } from './fa-subset.mjs';

const here = dirname(fileURLToPath(import.meta.url));
const project = resolve(here, '..', 'JiblaDental');
const wwwroot = join(project, 'wwwroot');

const cssFiles = [
  'assets/css/bootstrap.min.css',
  'assets/fonts/font/font-awesome.min.css',
  'assets/css/uicons-regular-rounded.css',
  'assets/fonts/flaticon.css',
  'assets/css/owl.carousel.css',
  'assets/css/slick.css',
  'assets/css/off-canvas.css',
  'assets/css/rsmenu-main.css',
  'assets/css/rs-spacing.css',
  'assets/css/style2.min.css',
  'style.min.css',
  'assets/css/responsive.min.css',
];
const sharedCssLast = 'assets/css/site-fixes.css'; // same file for both languages (read from the English root)
const iconFontCss = new Set([
  'assets/fonts/font/font-awesome.min.css',
  'assets/css/uicons-regular-rounded.css',
  'assets/fonts/flaticon.css',
]);

// Three files instead of one so the browser runs them as separate tasks (a single 300 KB script is one long task).
const jsBundles = {
  core: ['assets/js/jquery.min.js', 'assets/js/bootstrap.min.js'],
  plugins: [
    'assets/js/jquery.nav.js',
    'assets/js/jquery.malihu.PageScroll2id.min.js',
    'assets/js/owl.carousel.min.js',
    'assets/js/slick.min.js',
    'assets/js/wow-lite.js', // replaces wow.min.js (WOW.js)
    'assets/js/imagesloaded.pkgd.min.js',
    'assets/js/jquery.appear.min.js',
    'assets/js/odometer.min.js',
    'assets/js/jquery.magnific-popup.min.js',
  ],
  app: ['assets/js/main2.js', 'assets/js/main.js'],
};
const jsToMinify = new Set(['assets/js/main2.js', 'assets/js/main.js']);

const read = (path) => readFileSync(path, 'utf8').replace(/^﻿/, '');
const toWebPath = (absolutePath) => '/' + relative(wwwroot, absolutePath).split(sep).join('/');

function walk(dir, extensions, out = []) {
  for (const name of readdirSync(dir)) {
    const full = join(dir, name);
    if (['node_modules', 'bin', 'obj', 'Uploads', 'dist'].includes(name)) continue;
    if (statSync(full).isDirectory()) walk(full, extensions, out);
    else if (extensions.some((e) => name.endsWith(e))) out.push(full);
  }
  return out;
}


/** Make url(...) references absolute so the bundle can live in a different folder than its sources. */
function rewriteUrls(css, fileDir) {
  return css.replace(/url\(\s*(['"]?)([^'")]+)\1\s*\)/g, (match, quote, url) => {
    if (/^(data:|https?:|\/|#)/i.test(url)) return match;
    const [pathPart, suffix = ''] = url.split(/(?=[?#])/);
    const absolute = posix.normalize(toWebPath(join(fileDir, pathPart)));
    return `url("${absolute}${suffix}")`;
  });
}

let purgeContent;
async function purgeSpacing(css) {
  purgeContent ??= [
    ...walk(join(project, 'Views'), ['.cshtml']),
    ...walk(join(project, 'Areas'), ['.cshtml']),
    ...walk(join(project, 'ViewComponents'), ['.cs']),
    ...walk(join(project, 'Templates'), ['.cshtml']),
    ...walk(wwwroot, ['.js', '.html']),
  ].map((f) => ({ raw: read(f), extension: f.endsWith('.js') ? 'js' : 'html' }));
  const [result] = await new PurgeCSS().purge({
    content: purgeContent,
    css: [{ raw: css }],
    keyframes: true,
    fontFace: true,
  });
  return result.css;
}

async function buildCss(root) {
  const base = join(wwwroot, root);
  const parts = [];
  for (const file of cssFiles) {
    const path = join(base, file);
    let css = read(path).replace(/@charset\s+["'][^"']*["'];?/gi, '');
    if (file === 'assets/css/rs-spacing.css') css = await purgeSpacing(css);
    if (file === 'assets/fonts/font/font-awesome.min.css') css = addFontSubsets(purgeFaIcons(css));
    else if (file === 'assets/css/uicons-regular-rounded.css') css = addFontSubsets(css);
    if (iconFontCss.has(file)) css = css.replace(/font-display\s*:\s*swap/gi, 'font-display:block');
    parts.push(`/* ${file} */\n${rewriteUrls(css, dirname(path))}`);
  }
  const fixes = join(wwwroot, sharedCssLast);
  parts.push(`/* ${sharedCssLast} */\n${rewriteUrls(read(fixes), dirname(fixes))}`);

  const { code } = await transform(parts.join('\n'), { loader: 'css', minify: true, legalComments: 'none' });
  const out = join(base, 'assets', 'dist', 'site.min.css');
  mkdirSync(dirname(out), { recursive: true });
  writeFileSync(out, code);
  console.log(`${root ? '/' + root + '/' : '/'}assets/dist/site.min.css  ${(code.length / 1024).toFixed(1)} KiB`);
}

async function buildJs(root) {
  const base = join(wwwroot, root);
  for (const [name, files] of Object.entries(jsBundles)) {
    const parts = [];
    for (const file of files) {
      let js = read(join(base, file));
      if (jsToMinify.has(file)) js = (await transform(js, { loader: 'js', minify: true, legalComments: 'none' })).code;
      js = js.replace(/\/\/[#@]\s*sourceMappingURL=.*$/gm, '');
      parts.push(js);
    }
    const code = parts.join('\n;\n');
    const out = join(base, 'assets', 'dist', `site.${name}.min.js`);
    mkdirSync(dirname(out), { recursive: true });
    writeFileSync(out, code);
    console.log(`${root ? '/' + root + '/' : '/'}assets/dist/site.${name}.min.js  ${(code.length / 1024).toFixed(1)} KiB`);
  }
}

for (const root of ['', 'ar']) {
  await buildCss(root);
  await buildJs(root);
}
