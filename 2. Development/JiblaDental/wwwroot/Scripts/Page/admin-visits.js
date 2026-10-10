import { firebaseConfig, FIREBASE_VERSION, VISITS_COLLECTION } from "/Scripts/firebase-config.js";

const { initializeApp } = await import(`https://www.gstatic.com/firebasejs/${FIREBASE_VERSION}/firebase-app.js`);
const { getFirestore, collection, query, where, orderBy, limit, getDocs, Timestamp } =
    await import(`https://www.gstatic.com/firebasejs/${FIREBASE_VERSION}/firebase-firestore.js`);

const db = getFirestore(initializeApp(firebaseConfig));
const $ = id => document.getElementById(id);
const PAGE = 25;
const MAX_DOCS = 5000;

// ---------- filters ----------
const hourLabel = h => (h === 0 ? "12 AM" : h < 12 ? h + " AM" : h === 12 ? "12 PM" : (h - 12) + " PM");
$("hfrom").innerHTML = '<option value="0">From: All hours</option>' + Array.from({ length: 24 }, (_, h) => `<option value="${h}">From ${hourLabel(h)}</option>`).join("");
$("hto").innerHTML = '<option value="24">To: All hours</option>' + Array.from({ length: 24 }, (_, h) => `<option value="${h + 1}">To ${hourLabel((h + 1) % 24)}</option>`).join("");

function rangeStart(key) {
    const d = new Date(); d.setHours(0, 0, 0, 0);
    switch (key) {
        case "today": return [d, null];
        case "yesterday": { const e = new Date(d); d.setDate(d.getDate() - 1); return [d, e]; }
        case "week": d.setDate(d.getDate() - ((d.getDay() + 6) % 7)); return [d, null]; // Monday
        case "month": d.setDate(1); return [d, null];
        case "lastmonth": { const e = new Date(d.getFullYear(), d.getMonth(), 1); return [new Date(d.getFullYear(), d.getMonth() - 1, 1), e]; }
        case "3months": d.setMonth(d.getMonth() - 3); return [d, null];
        case "year": return [new Date(d.getFullYear(), 0, 1), null];
        default: return [null, null];
    }
}

let rows = [];     // visits of the selected range
let shown = PAGE;
let map, cluster;

async function load() {
    $("status").textContent = "Loading…";
    const [from, to] = rangeStart($("range").value);
    const cons = [];
    if (from) cons.push(where("ts", ">=", Timestamp.fromDate(from)));
    if (to) cons.push(where("ts", "<", Timestamp.fromDate(to)));
    try {
        const snap = await getDocs(query(collection(db, VISITS_COLLECTION), ...cons, orderBy("ts", "desc"), limit(MAX_DOCS)));
        rows = snap.docs.map(d => { const v = d.data(); v.date = v.ts?.toDate?.() ?? new Date(); return v; });
        $("status").textContent = rows.length >= MAX_DOCS ? `Showing the latest ${MAX_DOCS} visits.` : "";
    } catch (e) {
        rows = [];
        $("status").textContent = "Could not load visits: " + e.message;
    }
    render();
}

function filtered() {
    const hf = +$("hfrom").value, ht = +$("hto").value;
    if (hf === 0 && ht === 24) return rows;
    return rows.filter(v => {
        const h = v.date.getHours();
        return hf <= ht ? (h >= hf && h < ht) : (h >= hf || h < ht); // supports overnight windows, e.g. 10 PM -> 6 AM
    });
}

// ---------- render ----------
const esc = s => String(s ?? "").replace(/[&<>"']/g, c => ({ "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;" }[c]));

function bars(el, counts, color, flag) {
    const total = counts.reduce((a, [, n]) => a + n, 0);
    el.innerHTML = counts.length ? counts.slice(0, 8).map(([k, n]) => {
        const pct = Math.round(n / total * 100);
        const label = flag ? `<span class="cc">${esc(flag(k) || "")}</span>${esc(k)}` : esc(k);
        return `<div class="bar-row"><div class="top"><span>${label}</span><span>${n} · ${pct}%</span></div><div class="track"><div class="fill" style="width:${pct}%;background:${color}"></div></div></div>`;
    }).join("") : '<div class="empty">No data</div>';
}

const tally = (list, key) => Object.entries(list.reduce((m, v) => (m[v[key] || "Unknown"] = (m[v[key] || "Unknown"] || 0) + 1, m), {})).sort((a, b) => b[1] - a[1]);

function render() {
    const list = filtered();
    const unique = new Set(list.map(v => v.visitorId));
    const newIds = new Set(list.filter(v => v.isNew).map(v => v.visitorId));
    $("sTotal").textContent = list.length;
    $("sUnique").textContent = unique.size;
    $("sNew").textContent = newIds.size;
    $("sRet").textContent = [...unique].filter(id => !newIds.has(id)).length;

    bars($("bSources"), tally(list, "source"), "#e91e8c");
    bars($("bBrowsers"), tally(list, "browser"), "#17a2e0");
    bars($("bDevices"), tally(list, "device"), "#f5a142");
    const codeOf = Object.fromEntries(list.map(v => [v.country, v.countryCode]));
    bars($("bCountries"), tally(list, "country"), "#0b6fa8", c => codeOf[c]);

    renderMap(list);
    shown = PAGE;
    renderTable(list);
}

function renderTable(list) {
    const located = list.filter(v => v.lat != null && v.lng != null).length;
    $("tCount").textContent = `(${list.length} total, ${located} located)`;
    const fmt = new Intl.DateTimeFormat("en-US", { month: "short", day: "numeric", hour: "2-digit", minute: "2-digit", hour12: false });
    $("tbody").innerHTML = list.slice(0, shown).map(v => `<tr>
        <td><span class="cc">${esc(v.countryCode)}</span> <strong style="color:#0b6fa8">${esc(v.city)}</strong><br><small class="text-muted">${esc(v.country)}${v.gps ? " · 📍 exact GPS" : ""}</small></td>
        <td>${esc(v.device)} · ${esc(v.browser)}</td><td>${esc(v.source)}</td><td>${esc(v.page)}</td>
        <td><span class="${v.isNew ? "badge-new" : "badge-ret"}">${v.isNew ? "New" : "Returning"}</span></td>
        <td class="text-end">${fmt.format(v.date)}</td></tr>`).join("") || '<tr><td colspan="6" class="empty">No visits in this period.</td></tr>';
    $("more").classList.toggle("d-none", shown >= list.length);
}
$("more").addEventListener("click", () => { shown += PAGE; renderTable(filtered()); });

// ---------- map ----------
function initMap() {
    map = L.map("map", { worldCopyJump: true, minZoom: 2 }).setView([25, 20], 2);
    L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", { maxZoom: 18, attribution: "&copy; OpenStreetMap contributors" }).addTo(map);
    cluster = L.markerClusterGroup({
        showCoverageOnHover: false,
        zoomToBoundsOnClick: false, // we show the place list ourselves, then zoom
        iconCreateFunction: c => {
            const ms = c.getAllChildMarkers();
            const n = ms.reduce((a, m) => a + m.options.count, 0);
            const size = Math.min(70, 34 + String(n).length * 8 + Math.log2(n) * 4);
            return L.divIcon({ html: `<div class="cluster${ms.every(m => m.options.gps) ? " gps" : ""}" style="width:${size}px;height:${size}px">${n}</div>`, className: "", iconSize: [size, size] });
        }
    });
    cluster.on("clusterclick", e => {
        const places = placeTotals(e.layer.getAllChildMarkers());
        const total = places.reduce((a, p) => a + p.count, 0);
        L.popup({ maxWidth: 280 }).setLatLng(e.latlng).setContent(
            `<strong>${total} visits · ${places.length} place${places.length > 1 ? "s" : ""}</strong><ul class="pop-list">` +
            places.map(p => `<li><span>${esc(p.city)}, ${esc(p.country)}</span><strong>${p.count}</strong></li>`).join("") + "</ul>").openOn(map);
        e.layer.zoomToBounds({ padding: [40, 40] });
    });
    map.addLayer(cluster);
}

function placeTotals(markers) {
    const m = {};
    markers.forEach(k => { const o = k.options; const key = o.city + "|" + o.country; (m[key] ??= { city: o.city, country: o.country, count: 0 }).count += o.count; });
    return Object.values(m).sort((a, b) => b.count - a.count);
}

function renderMap(list) {
    if (!map) initMap();
    cluster.clearLayers();
    // one marker per place (city, rounded to ~1 km); a place with any exact-GPS visit is flagged
    const places = {};
    list.filter(v => v.lat != null && v.lng != null).forEach(v => {
        const key = v.city + "|" + v.country + "|" + v.lat.toFixed(2) + "|" + v.lng.toFixed(2);
        const p = places[key] ??= { city: v.city, country: v.country, lat: v.lat, lng: v.lng, count: 0, gps: true };
        p.count++;
        if (!v.gps) p.gps = false;
    });
    const markers = Object.values(places).map(p => {
        const mk = L.marker([p.lat, p.lng], {
            count: p.count, city: p.city, country: p.country, gps: p.gps,
            icon: L.divIcon({ html: `<div class="cluster${p.gps ? " gps" : ""}" style="width:34px;height:34px">${p.count}</div>`, className: "", iconSize: [34, 34] })
        });
        mk.bindPopup(`<strong>${esc(p.city)}</strong><br>${esc(p.country)}<br>${p.count} visit${p.count > 1 ? "s" : ""}`);
        return mk;
    });
    cluster.addLayers(markers);
    if (markers.length) map.fitBounds(cluster.getBounds(), { padding: [40, 40], maxZoom: 6 });
    setTimeout(() => map.invalidateSize(), 100);
}

$("range").addEventListener("change", load);
$("hfrom").addEventListener("change", render);
$("hto").addEventListener("change", render);
load();
