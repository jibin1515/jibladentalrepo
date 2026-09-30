// Shared by build-assets.mjs and build-critical.mjs: Font Awesome icon-rule purge and embedded font subsets.
import { readFileSync } from 'node:fs';
import { dirname, join, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

const here = dirname(fileURLToPath(import.meta.url));
const fonts = resolve(here, '..', 'JiblaDental', 'wwwroot', 'assets', 'fonts', 'webfonts');
const subsets = JSON.parse(readFileSync(join(here, 'font-subsets.json'), 'utf8'));

// Drops the ~1,800 unused `.fa-<icon>::before{content:"\f..."}` rules (about 60 KB of the stylesheet): keeps every brand
// icon (any social network can be picked in the admin) and the solid icons the site uses (see subset-fonts.py).
const keepIconCodes = new Set(subsets.keepCodes);
export function purgeFaIcons(css) {
  return css.replace(/(?:\.fa-[a-z0-9-]+(?:::?before)?,?)+\{content:"\\([0-9a-f]{3,4})"\}/gi, (rule, code) =>
    keepIconCodes.has(code.toLowerCase()) ? rule : '',
  );
}

// Small subsets of the Font Awesome fonts (built by subset-fonts.py). A second @font-face with a unicode-range is
// declared after each full one, so the icons the site uses come from the tiny file and the full font is only
// downloaded if an icon outside the subset is ever rendered. The subsets are embedded as data URIs (2-8 KB): no extra
// request, and nothing that can go missing on the server.
export function addFontSubsets(css) {
  return css.replace(/@font-face\s*\{[^}]*\}/g, (rule) => {
    const match = rule.match(/fa-(solid-900|brands-400)\.woff2|uicons-regular-rounded\.woff2/);
    if (!match) return rule;
    const which = match[1] ? (match[1].startsWith('solid') ? 'solid' : 'brands') : 'uicons';
    const file = which === 'uicons' ? join(fonts, '..', 'uicons-subset.woff2') : join(fonts, `fa-${which}-subset.woff2`);
    const data = readFileSync(file).toString('base64');
    const copy = rule
      .replace(/src\s*:[^;}]*/i, `src:url(data:font/woff2;base64,${data}) format("woff2")`)
      .replace(/\}\s*$/, `;unicode-range:${subsets[which]}}`);
    return rule + copy;
  });
}
