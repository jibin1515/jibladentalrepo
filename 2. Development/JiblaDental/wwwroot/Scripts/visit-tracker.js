// Public-site visit tracker: one Firestore document per session (30 min of inactivity starts a new one).
// Loaded as a module after the page is idle so it never competes with first paint.
import { firebaseConfig, FIREBASE_VERSION, VISITS_COLLECTION } from "/Scripts/firebase-config.js";

const SESSION_MS = 30 * 60 * 1000;
const store = {
    get(k, s) { try { return (s ? sessionStorage : localStorage).getItem(k); } catch { return null; } },
    set(k, v, s) { try { (s ? sessionStorage : localStorage).setItem(k, v); } catch { /* storage blocked */ } }
};

function isBot() {
    return navigator.webdriver || /bot|crawl|spider|slurp|headless|lighthouse|pagespeed|gtmetrix|preview/i.test(navigator.userAgent);
}

function deviceOf(ua) {
    if (/iPhone|iPod/.test(ua)) return "iPhone";
    if (/iPad/.test(ua)) return "iPad";
    if (/Android/.test(ua)) return /Mobile/.test(ua) ? "Android Phone" : "Android Tablet";
    if (/Windows/.test(ua)) return "Windows PC";
    if (/Macintosh|Mac OS X/.test(ua)) return "Mac";
    if (/CrOS/.test(ua)) return "Chromebook";
    if (/Linux|X11/.test(ua)) return "Linux PC";
    return "Other";
}

function browserOf(ua) {
    if (/Edg\//.test(ua)) return "Edge";
    if (/OPR\/|Opera/.test(ua)) return "Opera";
    if (/SamsungBrowser/.test(ua)) return "Samsung Internet";
    if (/Firefox\/|FxiOS/.test(ua)) return "Firefox";
    if (/Chrome\/|CriOS/.test(ua)) return "Chrome";
    if (/Safari\//.test(ua)) return "Safari";
    return "Other";
}

function sourceOf() {
    const q = new URLSearchParams(location.search);
    if (q.get("utm_source") || q.get("utm_medium") || q.get("utm_campaign") || q.get("gclid") || q.get("fbclid")) return "Campaign";
    let host = "";
    try { host = new URL(document.referrer).hostname.replace(/^www\./, ""); } catch { /* no referrer */ }
    if (!host || host === location.hostname.replace(/^www\./, "")) return "Direct";
    if (/(^|\.)(google|bing|yahoo|duckduckgo|yandex|baidu|ecosia|brave)\./.test(host)) return "Organic Search";
    if (/(facebook|instagram|twitter|x\.com|t\.co|linkedin|tiktok|snapchat|youtube|pinterest|whatsapp)/.test(host)) return "Social";
    return "Referral";
}

async function geoByIp() {
    const cached = store.get("jd_geo", true);
    if (cached) { try { return JSON.parse(cached); } catch { /* refetch */ } }
    const r = await fetch("https://ipwho.is/");
    const d = await r.json();
    if (!d || d.success === false) throw new Error("geo lookup failed");
    const geo = { country: d.country, countryCode: d.country_code, city: d.city || d.region || "Unknown", lat: d.latitude, lng: d.longitude };
    store.set("jd_geo", JSON.stringify(geo), true);
    return geo;
}

function askGps() {
    return new Promise(resolve => {
        if (!navigator.geolocation) return resolve(null);
        navigator.geolocation.getCurrentPosition(
            p => { store.set("jd_gps", "granted"); resolve({ lat: p.coords.latitude, lng: p.coords.longitude }); },
            () => { store.set("jd_gps", "denied"); resolve(null); },
            { timeout: 15000, maximumAge: 600000 });
    });
}

async function track() {
    if (isBot() || /^\/(admin|account)(\/|$)/i.test(location.pathname)) return;

    const now = Date.now();
    const last = parseInt(store.get("jd_last") || "0", 10);
    store.set("jd_last", String(now));
    if (last && now - last < SESSION_MS) return; // same session, already logged

    let vid = store.get("jd_vid");
    const isNew = !vid;
    if (!vid) { vid = (crypto.randomUUID ? crypto.randomUUID() : String(now) + Math.random().toString(16).slice(2)); store.set("jd_vid", vid); }

    const ua = navigator.userAgent;
    const [{ initializeApp }, { getFirestore, collection, addDoc, updateDoc, serverTimestamp }] = await Promise.all([
        import(`https://www.gstatic.com/firebasejs/${FIREBASE_VERSION}/firebase-app.js`),
        import(`https://www.gstatic.com/firebasejs/${FIREBASE_VERSION}/firebase-firestore.js`)
    ]);

    let geo = {};
    try { geo = await geoByIp(); } catch { /* still record the visit without location */ }

    const db = getFirestore(initializeApp(firebaseConfig));
    const ref = await addDoc(collection(db, VISITS_COLLECTION), {
        visitorId: vid,
        isNew,
        ts: serverTimestamp(),
        page: location.pathname,
        source: sourceOf(),
        referrer: document.referrer || "",
        device: deviceOf(ua),
        browser: browserOf(ua),
        lang: navigator.language || "",
        country: geo.country || "Unknown",
        countryCode: geo.countryCode || "",
        city: geo.city || "Unknown",
        lat: geo.lat ?? null,
        lng: geo.lng ?? null,
        gps: false
    });

    // Exact location: asked on first page load; if the visitor already granted it earlier this resolves silently.
    if (store.get("jd_gps") !== "denied") {
        const pos = await askGps();
        if (pos) await updateDoc(ref, { lat: pos.lat, lng: pos.lng, gps: true });
    }
}

const start = () => track().catch(() => { /* tracking must never break the site */ });
if (document.readyState === "complete") setTimeout(start, 1500);
else addEventListener("load", () => (window.requestIdleCallback || setTimeout)(start, 1500));
