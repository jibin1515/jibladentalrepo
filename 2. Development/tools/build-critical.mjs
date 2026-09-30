// Generates wwwroot/assets/dist/critical.min.css: the CSS needed to paint the home page above the fold (mobile + desktop).
// _Layout.cshtml inlines it on "/" and loads the full site.min.css without blocking rendering.
//
// Needs the site running (any URL that renders the home page) and Google Chrome:
//   cd "2. Development/tools" && npm run build           (rebuilds site.min.css first)
//   node build-critical.mjs http://localhost:5000/
//
// Env: CHROME_PATH overrides the Chrome location.
import { readFileSync, writeFileSync } from 'node:fs';
import { dirname, join, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';
import penthouse from 'penthouse';
import puppeteer from 'puppeteer-core';
import { transform } from 'esbuild';
import { addFontSubsets } from './fa-subset.mjs';

const here = dirname(fileURLToPath(import.meta.url));
const dist = resolve(here, '..', 'JiblaDental', 'wwwroot', 'assets', 'dist');
const url = process.argv[2] ?? 'http://localhost:5000/';
const chrome = process.env.CHROME_PATH ?? 'C:/Program Files/Google/Chrome/Application/chrome.exe';
const css = readFileSync(join(dist, 'site.min.css'), 'utf8');

const viewports = [
  { width: 412, height: 823 }, // phone
  { width: 820, height: 1180 }, // tablet
  { width: 1350, height: 900 }, // desktop
];
const parts = [];
for (const viewport of viewports) {
  parts.push(
    await penthouse({
      url,
      cssString: css,
      ...viewport,
      timeout: 120000,
      renderWaitTime: 1500,
      // pre-JS state: the CSS has to paint the page without help from owl/slick
      blockJSRequests: true,
      keepLargerMediaQueries: false,
      forceInclude: ['.visually-hidden', '.cv-auto', /^\.rs-slider/, /^\.owl-carousel/, /^\.rs-carousel/, /^\.slider-direction/],
      // penthouse closes the browser after each run, so hand it a fresh one every time
      puppeteer: { getBrowser: () => puppeteer.launch({ executablePath: chrome, headless: true, args: ['--no-sandbox'] }) },
    }),
  );
}

// each viewport run repeats the @font-face rules; keep one copy
const fontFaces = new Set();
const merged = parts
  .map((part) =>
    part.replace(/@font-face\s*\{[^}]*\}/g, (rule) => {
      fontFaces.add(rule);
      return '';
    }),
  )
  .join('\n');
// Font Awesome: add the embedded subsets so the full 108/147 KB fonts are not fetched for the header icons
// (the legacy "FontAwesome" / "Font Awesome 5" families are not used above the fold; the full stylesheet still defines them)
const criticalFontFaces = [...fontFaces].filter((rule) => !/font-family:\s*["']?(FontAwesome|Font Awesome 5)/i.test(rule));
const { code } = await transform(addFontSubsets(criticalFontFaces.join('\n')) + '\n' + merged, { loader: 'css', minify: true, legalComments: 'none' });
writeFileSync(join(dist, 'critical.min.css'), code);
console.log(`critical.min.css  ${(code.length / 1024).toFixed(1)} KiB (site.min.css ${(css.length / 1024).toFixed(1)} KiB)`);
